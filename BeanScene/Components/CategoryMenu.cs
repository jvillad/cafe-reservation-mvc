using BeanScene.Data;
using Microsoft.AspNetCore.Mvc;

namespace BeanScene.Components
{
	public class CategoryMenu : ViewComponent
	{
		private readonly BeanSceneApplicationDbContext _context;
		public CategoryMenu(BeanSceneApplicationDbContext context)
		{
			_context = context;
		}
		public IViewComponentResult Invoke()
		{
			return View(_context.Category.OrderBy(p => p.CategoryName));

		}
	}
}
