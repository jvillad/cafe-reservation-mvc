using BeanScene.Data;
using BeanScene.Models;
using BeanScene.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace BeanScene.Controllers
{
    [Authorize(Roles = "Manager")]
    public class SittingsController : Controller
    {
        private readonly BeanSceneApplicationDbContext _context;

        public SittingsController(BeanSceneApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Sittings
        public async Task<IActionResult> Index()
        {
            var beanSceneApplicationDbContext = _context.Sitting.Include(s => s.EndTime).Include(s => s.Schedule).Include(s => s.SittingType).Include(s => s.StartTime);
            return View(await beanSceneApplicationDbContext.ToListAsync());
        }
        // GET: Sittings/Details/1/2023-06-06
        [HttpGet("Sittings/Details/{sittingTypeId}/{dateString}")]
        public async Task<IActionResult> Details(int sittingTypeId, string dateString)
        {
            // Convert date string DateTime
            DateTime sittingDate;
            if (!DateTime.TryParse(dateString, out sittingDate)) return BadRequest("Invalid sitting date!");

            // Check SittingType exists
            SittingType? sittingType = await _context.SittingType.FindAsync(sittingTypeId);
            if (sittingType == null) return NotFound("Sitting Type not found.");

            // Check Sitting exists
            //Sitting? sitting = await _context.Sitting.FindAsync(sittingDate, sittingTypeId);
            Sitting? sitting = await _context.Sitting
                .Include(s => s.EndTime)
                .Include(s => s.StartTime)
                .Include(s => s.Schedule)
                .FirstOrDefaultAsync(s => s.Date == sittingDate && s.SittingTypeId == sittingTypeId);
            if (sitting == null) return NotFound("Sitting not found.");

            return View(sitting);
        }

        // GET: Sittings/Create
        public IActionResult Create()
        {
            // View model with default data (e.g. Timeslot list items)
            SittingViewModel viewModel = GenerateDefaultViewModel();

            // Load view with the view model
            return View(viewModel);
        }

        // POST: Sittings/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Date,SittingTypeId,StartTimeId,EndTimeId,ScheduleId,Status")] Sitting sitting)
        {
            // Find related entities based on their foreign key values (IDs)
            SittingType? sittingType = await _context.SittingType.FindAsync(sitting.SittingTypeId);
            Timeslot? startTime = await _context.Timeslot.FindAsync(sitting.StartTimeId);
            Timeslot? endTime = await _context.Timeslot.FindAsync(sitting.EndTimeId);

            // Check if related entities don't exist - throw 404 error
            if (sittingType == null) return NotFound("Sitting Type not found");
            if (startTime == null) return NotFound("Start time not found");
            if (endTime == null) return NotFound("End time not found");

            // Assign related entities to the model
            sitting.SittingType = sittingType;
            sitting.StartTime = startTime;
            sitting.EndTime = endTime;

            // Manually revalidate the model (to include related entities)
            ModelState.Clear();
            TryValidateModel(sitting);

            if (ModelState.IsValid)
            {
                _context.Add(sitting);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["EndTimeId"] = new SelectList(_context.Timeslot, "Time", "Time", sitting.EndTimeId);
            ViewData["ScheduleId"] = new SelectList(_context.SittingSchedule, "Id", "Id", sitting.ScheduleId);
            ViewData["SittingTypeId"] = new SelectList(_context.SittingType, "Id", "Name", sitting.SittingTypeId);
            ViewData["StartTimeId"] = new SelectList(_context.Timeslot, "Time", "Time", sitting.StartTimeId);
            return View(sitting);
        }

        // GET: Sittings/Edit/1/2023-06-06
        [HttpGet("Sittings/Edit/{sittingTypeId}/{dateString}")]
        public async Task<IActionResult> Edit(int sittingTypeId, string dateString)
        {
            // Convert date string DateTime
            DateTime sittingDate;
            if (!DateTime.TryParse(dateString, out sittingDate)) return BadRequest("Invalid sitting date!");

            // Check SittingType exists
            SittingType? sittingType = await _context.SittingType.FindAsync(sittingTypeId);
            if (sittingType == null) return NotFound("Sitting Type not found.");

            // Check Sitting exists
            Sitting? sitting = await _context.Sitting.FindAsync(sittingDate, sittingTypeId);
            if (sitting == null) return NotFound("Sitting not found.");

            // Generate view model
            SittingViewModel viewModel = GenerateDefaultViewModel();
            viewModel.Sitting = sitting;

            // Show view using view model
            return View(viewModel);
        }

        // POST: Sittings/Edit/1/2023-06-06
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost("Sittings/Edit/{sittingTypeId}/{dateString}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int sittingTypeId, string dateString, SittingViewModel viewModel)
        {
            // TODO: Custom validation (e.g. make sure query string matches the ViewModel)

            // Populate the associated entities of the view model using the foreign key values
            viewModel = PopulateViewModelEntities(viewModel);

            // Manually revalidate the model
            ModelState.Clear();
            TryValidateModel(viewModel);

            if (ModelState.IsValid)
            {
                // Extract the sitting model from the view model
                Sitting sitting = viewModel.Sitting;

                try
                {
                    _context.Update(sitting);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SittingExists(sitting.Date, sitting.SittingTypeId))
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

        // GET: Sittings/Delete/1/2023-06-06
        [HttpGet("Sittings/Delete/{sittingTypeId}/{dateString}")]
        public async Task<IActionResult> Delete(int sittingTypeId, string dateString)
        {
            // Convert date string DateTime
            DateTime sittingsDate;
            if (!DateTime.TryParse(dateString, out sittingsDate)) return BadRequest("Invalid sittings date!");

            // Check SittingType exists
            SittingType? sittingsType = await _context.SittingType.FindAsync(sittingTypeId);
            if (sittingsType == null) return NotFound("Sitting Type not found.");

            // Check Sitting exists
            //Sitting? sittings = await _context.Sitting.FindAsync(sittingsDate, sittingsTypeId);
            Sitting? sittings = await _context.Sitting
                .Include(s => s.EndTime)
                .Include(s => s.StartTime)
                .Include(s => s.Schedule)
                .FirstOrDefaultAsync(s => s.Date == sittingsDate && s.SittingTypeId == sittingTypeId);
            if (sittings == null) return NotFound("Sitting not found.");

            return View(sittings);
        }

        // POST: Sittings/Delete/1/2023-06-06
        [HttpPost("Sittings/Delete/{sittingTypeId}/{dateString}"), ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int sittingTypeId, string dateString)
        {
            // Convert date string DateTime
            DateTime sittingDate;
            if (!DateTime.TryParse(dateString, out sittingDate)) return BadRequest("Invalid sittings date!");

            // Check SittingType exists
            SittingType? sittingType = await _context.SittingType.FindAsync(sittingTypeId);
            if (sittingType == null) return NotFound("Sitting Type not found.");

            // Check Sitting exists
            Sitting? sitting = await _context.Sitting.FindAsync(sittingDate, sittingTypeId);
            if (sitting == null) return NotFound("Sitting not found.");

            // Delete the sittings
            _context.Sitting.Remove(sitting);
            await _context.SaveChangesAsync();

            // Redirect back to sittings listing
            return RedirectToAction(nameof(Index));
        }

        private bool SittingExists(DateTime date, int sittingTypeId)
        {
            return _context.Sitting.Find(date, sittingTypeId) != null;
        }

        private SittingViewModel GenerateDefaultViewModel(SittingViewModel? viewModel = null)
        {
            // Check if no view model passed in
            if (viewModel == null)
            {
                viewModel = new SittingViewModel();
                viewModel.Sitting = null!;
            }


            // Timeslot list items
            viewModel.Timeslots = _context.Timeslot.ToList();

            // SittingType list items
            viewModel.SittingTypeListItems = _context.SittingType
                .Select(st => new SelectListItem
                {
                    Value = st.Id.ToString(),
                    Text = st.Name
                }).ToList();

            return viewModel;
        }

        /// <summary>
        /// Populate the associated entities of the view model using the foreign key values (e.g. StartTimeId -> StartTime).
        /// </summary>
        /// <param name="viewModel"></param>
        /// <returns></returns>
        private SittingViewModel PopulateViewModelEntities(SittingViewModel viewModel)
        {
            // Sitting type
            viewModel.Sitting.SittingType = _context.SittingType.Find(viewModel.Sitting.SittingTypeId) ?? null!;

            // Start time
            viewModel.Sitting.StartTime = _context.Timeslot.Find(viewModel.Sitting.StartTimeId) ?? null!;

            // End time
            viewModel.Sitting.EndTime = _context.Timeslot.Find(viewModel.Sitting.EndTimeId) ?? null!;

            return viewModel;
        }
    }
}
