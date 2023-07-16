using BeanScene.Data;
using BeanScene.Models;
using BeanScene.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Text.RegularExpressions;

namespace BeanScene.Controllers
{
    [Authorize(Roles = "User,Staff,Manager")]
    public class ReservationsController : Controller
    {
        private readonly BeanSceneApplicationDbContext _context;
        private readonly UserManager<BeanSceneApplicationUser> _userManager;

        public ReservationsController(BeanSceneApplicationDbContext context, UserManager<BeanSceneApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Reservations
        [Authorize(Roles = "Staff,Manager")]
        public async Task<IActionResult> Index(bool? current, string? status)
        {
            // Get all reservations
            var reservations = _context.Reservation
                .Include(r => r.Area)
                .Include(r => r.EndTime)
                .Include(r => r.Sitting)
                .Include(r => r.StartTime)
                .Include(r => r.User)
                .Include(r => r.Sitting.SittingType)
                .OrderBy(r => r.SittingDate)
                .ThenBy(r => r.StartTimeId);

            // If user, filter reservations to ONLY show their own
            BeanSceneApplicationUser user = await GetLoggedInUserAsync();
            if (user.IsUser)
            {
                // Load view for User
                return View(await reservations.Where(r => r.UserId == user.Id).ToListAsync());
            }
            // Input defaults
            current = current ?? false;
            status = status ?? null;
            // Convert status to proper enum value
            Reservation.StatusEnum statusEnum;
            if (!Enum.TryParse<Reservation.StatusEnum>(status, out statusEnum)) status = null;
            // Filter reservations by current and status
            List<Reservation> reservationList = await reservations
                .Where(r => current == false || r.SittingDate >= DateTime.Now.Date)
                .Where(r => status == null || r.Status == statusEnum)
                .ToListAsync();
            // Load view for Staff/Manager
            return View(reservationList);

        }

        // GET: Reservations/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Reservation == null)
            {
                return NotFound();
            }

            var reservation = await _context.Reservation
                .Include(r => r.Area)
                .Include(r => r.EndTime)
                .Include(r => r.Sitting)
                .Include(r => r.StartTime)
                .Include(r => r.User)
                .Include(r => r.Sitting.SittingType)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (reservation == null)
            {
                return NotFound();
            }

            return View(reservation);
        }


        // GET: Reservations/Create
        [AllowAnonymous]
        public async Task<IActionResult> Create()
        {

            ReservationViewModel viewModel = GenerateDefaultViewModel();

            // Get the current logged-in user (and pass through the UserManager context)
            BeanSceneApplicationUser user = await GetLoggedInUserAsync();

            // Check if user is logged in and has "User" role (to pre-fill their data)
            if (user.IsUser)
            {
                viewModel.Reservation.FirstName = user.FirstName;
                viewModel.Reservation.LastName = user.LastName;
                viewModel.Reservation.Email = user.Email;
                viewModel.Reservation.Phone = user.PhoneNumber;

            }
            return View(viewModel);

        }

        // POST: Reservations/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task<IActionResult> Create(ReservationViewModel viewModel)
        {
            /*// Get the current logged-in user (and pass through the UserManager context)
            BeanSceneApplicationUser user = await _userManager.GetUserAsync(this.User) ?? new BeanSceneApplicationUser();
            user.UserManager = _userManager;*/


            // Sitting ID pattern (sittingDate:sittingTypeId, e.g.2023-01-01:1)
            Regex regexSittingId = new Regex(@"^(\d{4}-\d{2}-\d{2}):(\d)$");

            // Match sitting ID against regex pattern
            Match match = regexSittingId.Match(viewModel.SittingId);

            // Check if sitting ID is not valid
            if (!match.Success) return BadRequest("Invalid Sitting ID, should be in the format: 2023-01-01:1");

            // Extract the sitting date and sitting type ID
            string sittingDateString = match.Groups[1].Value;
            int sittingTypeId = int.Parse(match.Groups[2].Value);

            // Convert date string DateTime
            DateTime sittingDate;
            if (!DateTime.TryParse(sittingDateString, out sittingDate)) return BadRequest("Invalid sitting date!");

            // Check Sitting exists
            //Sitting? sitting = await _context.Sitting.FindAsync(sittingDate, sittingTypeId);
            Sitting? sitting = await _context.Sitting
                .Include(s => s.EndTime)
                .Include(s => s.StartTime)
                .Include(s => s.SittingType)
                .Include(s => s.Schedule)
                .FirstOrDefaultAsync(s => s.Date == sittingDate && s.SittingTypeId == sittingTypeId);
            if (sitting == null) return NotFound("Sitting not found.");

            // Get the current logged-in user
            BeanSceneApplicationUser user = await GetLoggedInUserAsync();

            // Get the model from the view model
            Reservation reservation = viewModel.Reservation;

            // check if user is in role
            string[] rolesToCheck = { "Staff", "Manager", "User" };
            bool userIsInAnyRole = false;

            foreach (string role in rolesToCheck)
            {
                if (await _userManager.IsInRoleAsync(user, role))
                {
                    userIsInAnyRole = true;
                    break;
                }
            }
            if (userIsInAnyRole)
            {
                // assign the userId manually if it is in role
                reservation.UserId = user.Id;
            }

            // Attach the sitting to the reservation
            reservation.SittingDate = sitting.Date;
            reservation.SittingSittingTypeId = sitting.SittingTypeId;
            reservation.Sitting = sitting;

            // Find related entities based on their foreign key values (IDs)
            Area? area = await _context.Area.FindAsync(reservation.AreaId);
            Timeslot? startTime = await _context.Timeslot.FindAsync(reservation.StartTimeId);
            Timeslot? endTime = await _context.Timeslot.FindAsync(reservation.EndTimeId);

            // Check if related entities don't exist - throw 404 error
            if (area == null) return NotFound("Room Type not found");
            if (startTime == null) return NotFound("Start time Timeslot not found");
            if (endTime == null) return NotFound("End time Timeslot not found");

            // Assign related entities to the model
            reservation.Area = area;
            reservation.StartTime = startTime;
            reservation.EndTime = endTime;

            // Populate the list items of the view model
            viewModel = GenerateDefaultViewModel(viewModel);

            // Manually revalidate the model (to include related entities)
            ModelState.Clear();
            TryValidateModel(viewModel);

            /*
             * Custom ModelState validation
             */

            // Validate start time & end time
            if (!reservation.IsValidDuration())
            {
                ModelState.AddModelError("Reservation.EndTimeId", $"Booking must be {reservation.MinBookingLengthMinutes} & {reservation.MaxBookingLengthMinutes} minutes long.");
            }
            if (!reservation.IsValidStartTime()) ModelState.AddModelError("Reservation.StartTimeId", $"Must be within the sitting time.");
            if (!reservation.IsValidEndTime()) ModelState.AddModelError("Reservation.EndTimeId", $"Must be within the sitting time.");

            // Set the reservation status to "pending"
            reservation.Status = Reservation.StatusEnum.Pending;


            // Check for invalid data
            if (ModelState.IsValid)
            {
                _context.Add(reservation);
                await _context.SaveChangesAsync();
                if (user.IsStaff || user.IsManager)
                    return RedirectToAction(nameof(Index));
                else
                {
                    TempData["Message"] = "Reservation sent";
                    /*return RedirectToAction(nameof(Create));*/
                    // User not logged in
                    return View("CreateConfirmed", viewModel.Reservation);
                }
            }

            // Pass view model back
            return View(viewModel);
        }

        // GET: Reservations/Edit/5
        [Authorize(Roles = "Staff,Manager")]
        public async Task<IActionResult> Edit(int? id)
        {

            if (id == null || _context.Reservation == null)
            {
                return NotFound();
            }

            var reservation = await _context.Reservation.FindAsync(id);

            if (reservation == null)
            {
                return NotFound();
            }

            ReservationViewModel viewModel = GenerateDefaultViewModel();
            viewModel.SittingId = $"{reservation.SittingDate:yyyy-MM-dd}:{reservation.SittingSittingTypeId}";
            viewModel.Reservation = reservation;

            return View(viewModel);
        }

        // POST: Reservations/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Staff,Manager")]
        public async Task<IActionResult> Edit(int id, ReservationViewModel viewModel)
        {
            viewModel = PopulateViewModelEntities(viewModel);
            // Sitting ID pattern (sittingDate:sittingTypeId, e.g.2023-01-01:1)
            Regex regexSittingId = new Regex(@"^(\d{4}-\d{2}-\d{2}):(\d)$");

            // Match sitting ID against regex pattern
            Match match = regexSittingId.Match(viewModel.SittingId);

            // Check if sitting ID is not valid
            if (!match.Success) return BadRequest("Invalid Sitting ID, should be in the format: 2023-01-01:1");

            // Extract the sitting date and sitting type ID
            string sittingDateString = match.Groups[1].Value;
            int sittingTypeId = int.Parse(match.Groups[2].Value);

            // Convert date string DateTime
            DateTime sittingDate;
            if (!DateTime.TryParse(sittingDateString, out sittingDate)) return BadRequest("Invalid sitting date!");

            // Check Sitting exists
            //Sitting? sitting = await _context.Sitting.FindAsync(sittingDate, sittingTypeId);
            Sitting? sitting = await _context.Sitting
                .Include(s => s.EndTime)
                .Include(s => s.StartTime)
                .Include(s => s.SittingType)
                .Include(s => s.Schedule)
                .FirstOrDefaultAsync(s => s.Date == sittingDate && s.SittingTypeId == sittingTypeId);
            if (sitting == null) return NotFound("Sitting not found.");

            // Get the current logged-in user
            BeanSceneApplicationUser user = await GetLoggedInUserAsync();

            // Get the model from the view model
            Reservation reservation = viewModel.Reservation;

            // check if user is in role
            string[] rolesToCheck = { "Staff", "Manager", "User" };
            bool userIsInAnyRole = false;

            foreach (string role in rolesToCheck)
            {
                if (await _userManager.IsInRoleAsync(user, role))
                {
                    userIsInAnyRole = true;
                    break;
                }
            }
            if (userIsInAnyRole)
            {
                // assign the userId manually if it is in role
                reservation.UserId = user.Id;
            }

            // Attach the sitting to the reservation
            reservation.SittingDate = sitting.Date;
            reservation.SittingSittingTypeId = sitting.SittingTypeId;
            reservation.Sitting = sitting;

            // Find related entities based on their foreign key values (IDs)
            Area? area = await _context.Area.FindAsync(reservation.AreaId);
            Timeslot? startTime = await _context.Timeslot.FindAsync(reservation.StartTimeId);
            Timeslot? endTime = await _context.Timeslot.FindAsync(reservation.EndTimeId);

            // Check if related entities don't exist - throw 404 error
            if (area == null) return NotFound("Room Type not found");
            if (startTime == null) return NotFound("Start time Timeslot not found");
            if (endTime == null) return NotFound("End time Timeslot not found");

            // Assign related entities to the model
            reservation.Area = area;
            reservation.StartTime = startTime;
            reservation.EndTime = endTime;

            // Populate the list items of the view model
            viewModel = GenerateDefaultViewModel(viewModel);
            // Manually revalidate the model
            ModelState.Clear();
            TryValidateModel(viewModel.Reservation);

            // Validate start time & end time
            if (!reservation.IsValidDuration())
            {
                ModelState.AddModelError("Reservation.EndTimeId", $"Booking must be {reservation.MinBookingLengthMinutes} & {reservation.MaxBookingLengthMinutes} minutes long.");
            }
            if (!reservation.IsValidStartTime()) ModelState.AddModelError("Reservation.StartTimeId", $"Must be within the sitting time.");
            if (!reservation.IsValidEndTime()) ModelState.AddModelError("Reservation.EndTimeId", $"Must be within the sitting time.");

            if (ModelState.IsValid)
            {

                try
                {
                    _context.Update(reservation);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ReservationExists(reservation.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(viewModel);
        }

        // GET: Reservations/Delete/5
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Reservation == null)
            {
                return NotFound();
            }

            var reservation = await _context.Reservation
                .Include(r => r.Area)
                .Include(r => r.EndTime)
                .Include(r => r.Sitting)
                .Include(r => r.StartTime)
                .Include(r => r.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (reservation == null)
            {
                return NotFound();
            }

            return View(reservation);
        }

        // POST: Reservations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Reservation == null)
            {
                return Problem("Entity set 'BeanSceneApplicationDbContext.Reservation'  is null.");
            }
            var reservation = await _context.Reservation.FindAsync(id);
            if (reservation != null)
            {
                _context.Reservation.Remove(reservation);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: Reservations/MyReservations
        public async Task<IActionResult> MyReservations()
        {
            // Get the current logged-in user (and pass through the UserManager context)
            BeanSceneApplicationUser user = await _userManager.GetUserAsync(this.User) ?? new BeanSceneApplicationUser();
            user.UserManager = _userManager;

            if (user == null)
            {
                // User is not logged in, redirect to login page or display an appropriate message
                return Redirect("/Identity/Account/Login");
            }

            // Retrieve the reservations made by the user
            var reservations = await _context.Reservation
                .Include(r => r.Area)
                .Include(r => r.EndTime)
                .Include(r => r.Sitting)
                .Include(r => r.StartTime)
                .Include(r => r.User)
                .Include(r => r.Sitting.SittingType)
                .Where(r => r.Email == user.Email)
                .ToListAsync();

            return View(reservations);
        }

        // GET: Reservations/Edit/5/Confirmed
        [Authorize(Roles = "Staff,Manager")]
        [HttpGet("Reservations/Edit/{id}/{status}")]
        public async Task<IActionResult> Edit(int id, string status)
        {
            // Find a reservation by ID
            var reservation = await _context.Reservation.FindAsync(id);
            if (reservation == null) return NotFound();

            // Validate status
            Reservation.StatusEnum statusEnum;
            if (!Enum.TryParse<Reservation.StatusEnum>(status, out statusEnum))
                return BadRequest("Status not valid.");

            // Change the status
            reservation.Status = statusEnum;

            // Update the database
            _context.Update(reservation);
            await _context.SaveChangesAsync();

            // Redirect to Index view
            /*return RedirectToAction(nameof(Index), new { id = reservation.Id });*/
            return RedirectToAction(nameof(Index));
        }
        private bool ReservationExists(int id)
        {
            return (_context.Reservation?.Any(e => e.Id == id)).GetValueOrDefault();
        }
        private ReservationViewModel GenerateDefaultViewModel(ReservationViewModel? viewModel = null)
        {

            // Check if no view model passed in
            if (viewModel == null)
            {
                viewModel = new ReservationViewModel();
                viewModel.Reservation = new Reservation();
            }


            // Timeslot list items
            viewModel.Timeslots = _context.Timeslot.ToList();

            // Area list items
            viewModel.AreasListItems = _context.Area
                .Select(rt => new SelectListItem
                {
                    Value = rt.Id.ToString(),
                    Text = rt.Name
                }).ToList();

            // User list items
            viewModel.UserListItems = _context.Users
                .OrderBy(u => (string.IsNullOrEmpty(u.FirstName) ? "--" : u.FirstName) + " (" + u.UserName + ")")
                .Select(u => new SelectListItem
                {
                    Value = u.UserName,
                    Text = (string.IsNullOrEmpty(u.FirstName) ? "--" : u.FirstName) + " (" + u.UserName + ")"
                }).ToList();

            // Session list items
            viewModel.SittingsListItems = _context.Sitting
                .OrderBy(s => s.Date.ToString() + s.SittingType.Name)
                .Select(s => new SelectListItem
                {
                    Value = s.Date.ToString("yyyy-MM-dd") + ":" + s.SittingTypeId.ToString(),
                    /*Text = s.Date.ToShortDateString() + " - " + s.SittingType.Name*/
                    Text = $"{s.Date.ToShortDateString()} - {s.SittingType.Name} ({s.StartTime.TimeFormatted}-{s.EndTime.TimeFormatted})"
                }).ToList();

            return viewModel;
        }

        /// <summary>
        /// Populate the associated entities of the view model using the foreign key values (e.g. StartTimeId -> StartTime).
        /// </summary>
        /// <param name="viewModel"></param>
        /// <returns></returns>
        private ReservationViewModel PopulateViewModelEntities(ReservationViewModel viewModel)
        {
            // Room type
            viewModel.Reservation.Area = _context.Area.Find(viewModel.Reservation.AreaId) ?? null!;

            // Start time
            viewModel.Reservation.StartTime = _context.Timeslot.Find(viewModel.Reservation.StartTimeId) ?? null!;

            // End time
            viewModel.Reservation.EndTime = _context.Timeslot.Find(viewModel.Reservation.EndTimeId) ?? null!;

            return viewModel;
        }
        private async Task<BeanSceneApplicationUser> GetLoggedInUserAsync()
        {
            // Get the current logged-in user (and pass through the UserManager context)
            BeanSceneApplicationUser user = await _userManager.GetUserAsync(this.User) ?? new BeanSceneApplicationUser();
            user.UserManager = _userManager;
            return user;
        }
    }
}
