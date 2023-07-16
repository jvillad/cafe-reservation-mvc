using BeanScene.Controllers;
using BeanScene.Data;
using BeanScene.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BeanScene.Tests.Controllers
{
    public class CategoriesControllerTests
    {
        private CategoriesController _controller;
        private DbContextOptions<BeanSceneApplicationDbContext> _options;

        public CategoriesControllerTests()
        {
            _options = new DbContextOptionsBuilder<BeanSceneApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
        }

        private BeanSceneApplicationDbContext CreateDbContext()
        {
            return new BeanSceneApplicationDbContext(_options);
        }

        [Fact]
        public async Task Index_ReturnsViewResult()
        {
            using (var context = CreateDbContext())
            {
                context.Category.Add(new Category { CategoryId = 1, CategoryName = "Test Category 1", Description = "Test Description 1" });
                context.Category.Add(new Category { CategoryId = 2, CategoryName = "Test Category 2", Description = "Test Description 2" });
                context.SaveChanges();

                _controller = new CategoriesController(context);

                var result = await _controller.Index();

                var viewResult = Assert.IsType<ViewResult>(result);
                var model = Assert.IsAssignableFrom<IEnumerable<Category>>(viewResult.ViewData.Model);
                Assert.Equal(2, model.Count());
            }
        }

        [Fact]
        public void CreateDbContext_ReturnsNewInstance()
        {
            var context1 = CreateDbContext();
            var context2 = CreateDbContext();

            Assert.NotSame(context1, context2);
            Assert.IsType<BeanSceneApplicationDbContext>(context1);
            Assert.IsType<BeanSceneApplicationDbContext>(context2);
        }

        [Fact]
        public async Task Index_RetrievesCorrectCategories()
        {
            using (var context = CreateDbContext())
            {
                context.Category.Add(new Category { CategoryId = 1, CategoryName = "Test Category 1", Description = "Test Description 1" });
                context.Category.Add(new Category { CategoryId = 2, CategoryName = "Test Category 2", Description = "Test Description 2" });
                context.SaveChanges();

                _controller = new CategoriesController(context);

                var result = await _controller.Index();

                var viewResult = Assert.IsType<ViewResult>(result);
                var model = Assert.IsAssignableFrom<IEnumerable<Category>>(viewResult.ViewData.Model);
                var categories = model.ToList();
                Assert.Equal(2, categories.Count);
                Assert.Equal("Test Category 1", categories[0].CategoryName);
                Assert.Equal("Test Description 1", categories[0].Description);
                Assert.Equal("Test Category 2", categories[1].CategoryName);
                Assert.Equal("Test Description 2", categories[1].Description);
            }
        }
    }
}
