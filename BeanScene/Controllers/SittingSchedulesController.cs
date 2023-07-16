using BeanScene.Data;
using BeanScene.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace BeanScene.Controllers
{
    [Authorize(Roles = "Manager")]
    public class SittingSchedulesController : Controller
    {
        private readonly BeanSceneApplicationDbContext _context;

        public SittingSchedulesController(BeanSceneApplicationDbContext context)
        {
            _context = context;
        }

        // GET: SittingSchedules
        public async Task<IActionResult> Index()
        {
            var beanSceneApplicationDbContext = _context.SittingSchedule.Include(s => s.EndTime).Include(s => s.SittingType).Include(s => s.StartTime);
            return View(await beanSceneApplicationDbContext.ToListAsync());
        }

        // GET: SittingSchedules/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.SittingSchedule == null)
            {
                return NotFound();
            }

            var sittingSchedule = await _context.SittingSchedule
                .Include(s => s.EndTime)
                .Include(s => s.SittingType)
                .Include(s => s.StartTime)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (sittingSchedule == null)
            {
                return NotFound();
            }

            return View(sittingSchedule);
        }

        // GET: SittingSchedules/Create
        public IActionResult Create()
        {
            ViewData["EndTimeId"] = new SelectList(_context.Timeslot, "Time", "Time");
            ViewData["SittingTypeId"] = new SelectList(_context.SittingType, "Id", "Name");
            ViewData["StartTimeId"] = new SelectList(_context.Timeslot, "Time", "Time");
            return View();
        }

        // POST: SittingSchedules/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,StartDate,EndDate,SittingTypeId,StartTimeId,EndTimeId,Monday,Tuesday,Wednesday,Thursday,Friday,Saturday,Sunday")] SittingSchedule sittingSchedule)
        {
            if (ModelState.IsValid)
            {
                _context.Add(sittingSchedule);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["EndTimeId"] = new SelectList(_context.Timeslot, "Time", "Time", sittingSchedule.EndTimeId);
            ViewData["SittingTypeId"] = new SelectList(_context.SittingType, "Id", "Name", sittingSchedule.SittingTypeId);
            ViewData["StartTimeId"] = new SelectList(_context.Timeslot, "Time", "Time", sittingSchedule.StartTimeId);
            return View(sittingSchedule);
        }

        // GET: SittingSchedules/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.SittingSchedule == null)
            {
                return NotFound();
            }

            var sittingSchedule = await _context.SittingSchedule.FindAsync(id);
            if (sittingSchedule == null)
            {
                return NotFound();
            }
            ViewData["EndTimeId"] = new SelectList(_context.Timeslot, "Time", "Time", sittingSchedule.EndTimeId);
            ViewData["SittingTypeId"] = new SelectList(_context.SittingType, "Id", "Name", sittingSchedule.SittingTypeId);
            ViewData["StartTimeId"] = new SelectList(_context.Timeslot, "Time", "Time", sittingSchedule.StartTimeId);
            return View(sittingSchedule);
        }

        // POST: SittingSchedules/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,StartDate,EndDate,SittingTypeId,StartTimeId,EndTimeId,Monday,Tuesday,Wednesday,Thursday,Friday,Saturday,Sunday")] SittingSchedule sittingSchedule)
        {
            if (id != sittingSchedule.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(sittingSchedule);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SittingScheduleExists(sittingSchedule.Id))
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
            ViewData["EndTimeId"] = new SelectList(_context.Timeslot, "Time", "Time", sittingSchedule.EndTimeId);
            ViewData["SittingTypeId"] = new SelectList(_context.SittingType, "Id", "Name", sittingSchedule.SittingTypeId);
            ViewData["StartTimeId"] = new SelectList(_context.Timeslot, "Time", "Time", sittingSchedule.StartTimeId);
            return View(sittingSchedule);
        }

        // GET: SittingSchedules/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.SittingSchedule == null)
            {
                return NotFound();
            }

            var sittingSchedule = await _context.SittingSchedule
                .Include(s => s.EndTime)
                .Include(s => s.SittingType)
                .Include(s => s.StartTime)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (sittingSchedule == null)
            {
                return NotFound();
            }

            return View(sittingSchedule);
        }

        // POST: SittingSchedules/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.SittingSchedule == null)
            {
                return Problem("Entity set 'BeanSceneApplicationDbContext.SittingSchedule'  is null.");
            }
            var sittingSchedule = await _context.SittingSchedule.FindAsync(id);
            if (sittingSchedule != null)
            {
                _context.SittingSchedule.Remove(sittingSchedule);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SittingScheduleExists(int id)
        {
            return (_context.SittingSchedule?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
