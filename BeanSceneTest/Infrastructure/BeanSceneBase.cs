using BeanScene.Data;
using BeanScene.Models;
using Microsoft.EntityFrameworkCore;

namespace BeanScene.Tests.Infrastructure
{
    public class BeanSceneBase : IDisposable
    {
        protected readonly BeanSceneApplicationDbContext _context;

        public BeanSceneBase()
        {
            // DB context options - use in-memory database with random name (each unit test uses different DB)
            var options = new DbContextOptionsBuilder<BeanSceneApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            // Store DB context
            _context = new BeanSceneApplicationDbContext(options);

            // Cleanup
            _context.Database.EnsureCreated();

            // Exit if DB already has data
            if (_context.Category.Any()) return;

            // Seed data
            SeedCategoryData();
            SeedFoodData();
        }

        private void SeedCategoryData()
        {
            // Category collection
            var categories = new[]
            {
                new Category { CategoryId = 1, CategoryName = "Main" },
                new Category { CategoryId = 2, CategoryName = "Entree" },
                new Category { CategoryId = 3, CategoryName = "Desserts" },
                new Category { CategoryId = 4, CategoryName = "Drinks" },
                new Category { CategoryId = 5, CategoryName = "Specials" },
                new Category { CategoryId = 6, CategoryName = "Sides" },

            };

            _context.Category.AddRange(categories);
            _context.SaveChanges();
        }

        private void SeedFoodData()
        {
            // Food collection
            var foods = new[]
            {
                new Food {
                    FoodId = 1,
                    Name = "Margherita Pizza",
                    ShortDescription = "Classic pizza topped with fresh basil and mozzarella cheese.",
                    LongDescription = "Enjoy the traditional flavors of Italy with our Margherita Pizza. Made with hand-stretched dough, topped with rich tomato sauce, fresh basil leaves, and melted mozzarella cheese.",
                    AllergyInformation = null,
                    Price = 12.99m,

                    WeeklySpecial = false,
                    CategoryId = 1
                },
                new Food {
                    FoodId = 2,
                    Name = "Grilled Salmon",
                    ShortDescription = "Freshly grilled salmon fillet served with steamed vegetables.",
                    LongDescription = "Indulge in our Grilled Salmon dish featuring a perfectly grilled salmon fillet seasoned with herbs and served with a side of steamed vegetables. A healthy and delicious choice.",
                    AllergyInformation = "Contains fish",
                    Price = 19.99m,
                    WeeklySpecial = true,
                    CategoryId = 2,
                },
                new Food {
                    FoodId = 3,
                    Name = "Chocolate Brownie Sundae",
                    ShortDescription = "Decadent chocolate brownie topped with vanilla ice cream and drizzled with hot fudge.",
                    LongDescription = "Treat yourself to our irresistible Chocolate Brownie Sundae. It features a warm and gooey chocolate brownie served with a scoop of creamy vanilla ice cream and a generous drizzle of hot fudge sauce.",
                    AllergyInformation = null,
                    Price = 8.99m,
                    WeeklySpecial = false,
                    CategoryId = 3,
                }
            };

            _context.Food.AddRange(foods);
            _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }
    }
}
