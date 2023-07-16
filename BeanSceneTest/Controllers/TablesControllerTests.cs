using BeanScene.Controllers;
using BeanScene.Data;
using BeanScene.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BeanScene.Tests.Controllers
{
    public class TablesControllerTests
    {
        private TablesController _controller;
        private DbContextOptions<BeanSceneApplicationDbContext> _options;

        public TablesControllerTests()
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
                context.Area.Add(new Area { Id = 1, Name = "Test Area 1" });
                context.Area.Add(new Area { Id = 2, Name = "Test Area 2" });
                context.Table.Add(new Table { Id = 1, Code = "X01", AreaId = 1 });
                context.Table.Add(new Table { Id = 2, Code = "X02", AreaId = 2 });
                context.SaveChanges();

                _controller = new TablesController(context);

                var result = await _controller.Index();

                var viewResult = Assert.IsType<ViewResult>(result);
                var model = Assert.IsAssignableFrom<IEnumerable<Table>>(viewResult.ViewData.Model);
                Assert.Equal(2, model.Count());
            }
        }

        [Fact]
        public async Task Details_ValidId_ReturnsViewResult()
        {
            using (var context = CreateDbContext())
            {
                context.Area.Add(new Area { Id = 1, Name = "Test Area 1" });
                context.Table.Add(new Table { Id = 1, Code = "X01", AreaId = 1 });
                context.SaveChanges();

                _controller = new TablesController(context);

                var result = await _controller.Details(1);

                var viewResult = Assert.IsType<ViewResult>(result);
                var model = Assert.IsAssignableFrom<Table>(viewResult.ViewData.Model);
                Assert.Equal(1, model.Id);
            }
        }

        [Fact]
        public async Task Details_InvalidId_ReturnsNotFound()
        {
            using (var context = CreateDbContext())
            {
                _controller = new TablesController(context);

                var result = await _controller.Details(99);

                Assert.IsType<NotFoundResult>(result);
            }
        }

    }
}
