using BeanScene.Data;
using BeanScene.Models;
using BeanScene.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace BeanScene.Controllers
{
    [Authorize(Roles = "Manager")]
    public class ManagementController : Controller
    {

        private readonly UserManager<BeanSceneApplicationUser> _userManager;
        private readonly BeanSceneApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ManagementController(BeanSceneApplicationDbContext context, UserManager<BeanSceneApplicationUser> userManager, IWebHostEnvironment webHostEnvironment)
        {
            _userManager = userManager;
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }
        public async Task<IActionResult> Index()
        {
            /* SELECT r.*, st.*
             FROM Reservation r
             LEFT JOIN SittingType st ON st.Id = r.SittingSittingTypeId*/

            // Retrieve all reservations from the database
            var reservations = await _context.Reservation.
                Select(r => new
                {
                    Reservation = r,
                    SittingType = _context.SittingType.FirstOrDefault(st => st.Id == r.SittingSittingTypeId)
                })
                .ToListAsync();


            var events = reservations.Select(r => new CalendarViewModel
            {
                Title = $"{r.Reservation.Status} ({r.Reservation.EndTimeId.Subtract(r.Reservation.StartTimeId).TotalMinutes} min)",
                Start = r.Reservation.SittingDate.Date.Add(r.Reservation.StartTimeId.TimeOfDay),
                End = r.Reservation.SittingDate.Date.Add(r.Reservation.EndTimeId.TimeOfDay),
                Url = Url.Action("Details", "Reservations", new { id = r.Reservation.Id }),
                ClassName = r.Reservation.Status == Reservation.StatusEnum.Pending ? "highlight-pending" : ""
            }).ToList();




            // Convert the reservations into calendar view models
            /*var events = reservations.Select(r => new CalendarViewModel
            {
                Title = $"{r.SittingSittingTypeId} ({r.EndTimeId.Subtract(r.StartTimeId).TotalMinutes} min) - {r.Status}",
                Start = r.SittingDate.Date.Add(r.StartTimeId.TimeOfDay),
                End = r.SittingDate.Date.Add(r.EndTimeId.TimeOfDay),
                Url = Url.Action("Details", "Reservations", new { id = r.Id })
            }).ToList();*/



            return View(events);
        }


        /* public async Task<IActionResult> Index()
         {
             // Retrieve all pending reservations from the database and include the associated Sitting and SittingType objects
             var reservations = await _context.Reservation
                 .Where(r => r.Status == Reservation.StatusEnum.Pending)
                 .Include(r => r.Sitting)
                 .ThenInclude(s => s.SittingType)
                 .ToListAsync();

             // Group the reservations by sitting type
             var reservationsBySittingType = reservations
                 .GroupBy(r => r.Sitting.SittingType.Name)
                 .ToDictionary(g => g.Key, g => g.ToList());

             // Pass the grouped reservations to the view
             return View(reservationsBySittingType);
         }
 */
        public async Task<IActionResult> Users()
        {

            var users = await _userManager.Users.ToListAsync();


            return View(users);
        }


    }
}
