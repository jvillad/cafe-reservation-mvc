
using BeanScene.Controllers;
using BeanScene.Models;
using BeanScene.Tests.Infrastructure;
using Microsoft.AspNetCore.Mvc;

namespace BeanScene.Tests.Controllers
{
    public class FoodsControllerTests : BeanSceneBase
    {
        // Unit test method naming convention: class_method_other-info_expected-result
        [Fact]
        public async void FoodsController_Index_ReturnsView()
        {
            // Arrange (get everything ready)
            var controller = new FoodsController(_context);

            // Act (run the code being tested)
            var result = await controller.Index();

            // Assert (check/assert the result)
            // Make sure a View is returned
            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public async void FoodsController_Details_ReturnsView()
        {
            // Arrange
            var controller = new FoodsController(_context);
            const int EXISTING_FOOD_ID = 2;

            // Act
            var result = await controller.Details(id: EXISTING_FOOD_ID);

            // Assert
            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public async void FoodsController_Details_NoFoodId_ReturnsNotFound()
        {
            // Arrange
            var controller = new FoodsController(_context);

            // Act
            var result = await controller.Details(id: null!);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async void FoodsController_Details_NonExistingFood_ReturnsNotFound()
        {
            // Arrange
            var controller = new FoodsController(_context);
            const int NON_EXISTING_FOOD_ID = 9999999;

            // Act
            var result = await controller.Details(id: NON_EXISTING_FOOD_ID);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async void FoodsController_Create_Success_ReturnsRedirect()
        {
            // Arrange - create controller and add ObjectValidator to stop null exceptions in TryValidateModel()
            var controller = new FoodsController(_context);
            controller.ObjectValidator = new ObjectValidator();

            // Valid food
            Food food = new Food
            {
                FoodId = 101,
                Name = "BBQ Chicken Wings",
                ShortDescription = "Tender chicken wings coated in smoky BBQ sauce.",
                LongDescription = "Get your fingers messy with our delicious BBQ Chicken Wings. These tender chicken wings are marinated in a smoky BBQ sauce, grilled to perfection, and served with a side of tangy ranch dip. Finger-licking good!",
                AllergyInformation = null,
                Price = 10.99m,
                ImageUrl = "https://example.com/bbq_chicken_wings.jpg",
                ImageThumbnailUrl = "https://example.com/bbq_chicken_wings_thumb.jpg",
                WeeklySpecial = false,
                CategoryId = 5,
            };

            // Act
            var result = await controller.Create(food);

            // Assert
            // Should be redirected to the index page (same controller)
            // (controller name = null, controller action = "Index")
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Null(redirectToActionResult.ControllerName);
            Assert.Equal("Index", redirectToActionResult.ActionName);
        }

        [Fact]
        public async void FoodsController_Create_InvalidCategory_ReturnsNotFound()
        {
            // Arrange
            var controller = new FoodsController(_context);
            controller.ObjectValidator = new ObjectValidator();

            // Invalid food (bad genre)
            Food food = new Food
            {
                FoodId = 99,
                Name = "Mojito",
                ShortDescription = "Refreshing cocktail made with rum, fresh mint, lime juice, and soda water.",
                LongDescription = "Quench your thirst with our signature Mojito cocktail. It combines the vibrant flavors of rum, muddled fresh mint leaves, zesty lime juice, and fizzy soda water. Perfect for a sunny day or a night out!",
                AllergyInformation = null,
                Price = 7.99m,
                WeeklySpecial = true,
                CategoryId = 99,
            };

            // Act
            var result = await controller.Create(food);

            // Assert - Not Found
            Assert.IsType<NotFoundResult>(result);
        }
    }
}
