using BeanScene.Data;
using BeanScene.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace BeanScene.Controllers
{
    [Authorize(Roles = "User,Staff,Manager")]
    public class FoodsController : Controller
    {
        private readonly BeanSceneApplicationDbContext _context;

        public FoodsController(BeanSceneApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Foods
        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            var beanSceneApplicationDbContext = _context.Food.Include(f => f.Category);
            return View(await beanSceneApplicationDbContext.ToListAsync());
        }

        // GET: Foods/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Food == null)
            {
                return NotFound();
            }

            var food = await _context.Food
                .Include(f => f.Category)
                .FirstOrDefaultAsync(m => m.FoodId == id);
            if (food == null)
            {
                return NotFound();
            }

            return View(food);
        }

        // GET: Foods/Create
        public IActionResult Create()
        {
            ViewData["CategoryId"] = new SelectList(_context.Category, "CategoryId", "CategoryName");
            return View();
        }

        // POST: Foods/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("FoodId,Name,ShortDescription,LongDescription,AllergyInformation,Price,ImageUrl,ImageThumbnailUrl,WeeklySpecial,CategoryId")] Food food)
        {
            // Assign a category to the book object (turn CategoryId into a Category)
            Category? category = await _context.Category.FindAsync(food.CategoryId);
            // Check if category not null (valid category ID), otherwise go and grab the first category from the DB

            // Check if category doesn't exist - throw 404 error
            if (category == null)
            {
                return NotFound();
            }

            // Manually revalidate the model (to include the added category)
            food.Category = category;
            ModelState.Clear();
            TryValidateModel(food);

            if (ModelState.IsValid)
            {
                _context.Add(food);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryId"] = new SelectList(_context.Category, "CategoryId", "CategoryName", food.CategoryId);
            return View(food);
        }

        // GET: Foods/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Food == null)
            {
                return NotFound();
            }

            var food = await _context.Food.FindAsync(id);
            if (food == null)
            {
                return NotFound();
            }
            ViewData["CategoryId"] = new SelectList(_context.Category, "CategoryId", "CategoryName", food.CategoryId);
            return View(food);
        }

        // POST: Foods/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("FoodId,Name,ShortDescription,LongDescription,AllergyInformation,Price,ImageUrl,ImageThumbnailUrl,WeeklySpecial,CategoryId")] Food food)
        {

            // Check if FoodID doesn't match ID
            if (id != food.FoodId)
            {
                return NotFound();
            }

            // Assign a category to the book object (turn CategoryId into a Category)
            Category? category = await _context.Category.FindAsync(food.CategoryId);

            // Check if category doesn't exist - throw 404 error
            if (category == null) return NotFound();

            // Manually revalidate the model (to include the added category)
            food.Category = category;
            ModelState.Clear();
            TryValidateModel(food);


            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(food);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FoodExists(food.FoodId))
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
            ViewData["CategoryId"] = new SelectList(_context.Category, "CategoryId", "CategoryName", food.CategoryId);
            return View(food);
        }

        // GET: Foods/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Food == null)
            {
                return NotFound();
            }

            var food = await _context.Food
                .Include(f => f.Category)
                .FirstOrDefaultAsync(m => m.FoodId == id);
            if (food == null)
            {
                return NotFound();
            }

            return View(food);
        }

        // POST: Foods/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Food == null)
            {
                return Problem("Entity set 'BeanSceneApplicationDbContext.Food'  is null.");
            }
            var food = await _context.Food.FindAsync(id);
            if (food != null)
            {
                _context.Food.Remove(food);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FoodExists(int id)
        {
            return (_context.Food?.Any(e => e.FoodId == id)).GetValueOrDefault();
        }

        [AllowAnonymous]
        public async Task<IActionResult> ViewAll(string category)
        {
            IEnumerable<Food> food;

            if (string.IsNullOrEmpty(category) || category == "All Food")
            {
                var beanSceneApplicationDbContext = _context.Food.Include(f => f.Category);
                food = await beanSceneApplicationDbContext.ToListAsync();

                /*return View(await beanSceneApplicationDbContext.ToListAsync());*/
            }
            else
            {
                var beanSceneApplicationDbContext = _context.Food.Where(p => p.Category.CategoryName == category)
                    .OrderBy(p => p.FoodId);
                food = await beanSceneApplicationDbContext.ToListAsync();
            }

            return View(food);
        }


        // GET: Foods/ViewDetails/5
        [AllowAnonymous]
        public async Task<IActionResult> FoodDetails(int? id)
        {
            if (id == null || _context.Food == null)
            {
                return NotFound();
            }

            var food = await _context.Food
                .Include(f => f.Category)
                .FirstOrDefaultAsync(m => m.FoodId == id);
            if (food == null)
            {
                return NotFound();
            }

            return View(food);
        }
    }
}
