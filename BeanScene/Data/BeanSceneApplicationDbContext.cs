using BeanScene.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BeanScene.Data
{
    public class BeanSceneApplicationDbContext : IdentityDbContext<BeanSceneApplicationUser>
    {
        public BeanSceneApplicationDbContext(DbContextOptions<BeanSceneApplicationDbContext> options) : base(options) { }

        public DbSet<Category> Category { get; set; }
        public DbSet<Food> Food { get; set; }
        public DbSet<Area> Area { get; set; }
        public DbSet<Table> Table { get; set; }
        public DbSet<Timeslot> Timeslot { get; set; }
        public DbSet<SittingType> SittingType { get; set; }
        public DbSet<Sitting> Sitting { get; set; }
        public DbSet<SittingSchedule> SittingSchedule { get; set; }
        public DbSet<Reservation> Reservation { get; set; }
        public DbSet<Image> Image { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {

            // seeding for Timeslot
            builder.Entity<Timeslot>().HasData(
             new Timeslot { Time = new DateTime(2000, 1, 1, 8, 0, 0) }, // 8:00AM
             new Timeslot { Time = new DateTime(2000, 1, 1, 8, 30, 0) },
             new Timeslot { Time = new DateTime(2000, 1, 1, 9, 0, 0) },
             new Timeslot { Time = new DateTime(2000, 1, 1, 9, 30, 0) },
             new Timeslot { Time = new DateTime(2000, 1, 1, 10, 0, 0) },
             new Timeslot { Time = new DateTime(2000, 1, 1, 10, 30, 0) },
             new Timeslot { Time = new DateTime(2000, 1, 1, 11, 0, 0) },
             new Timeslot { Time = new DateTime(2000, 1, 1, 11, 30, 0) },
             new Timeslot { Time = new DateTime(2000, 1, 1, 12, 0, 0) },
             new Timeslot { Time = new DateTime(2000, 1, 1, 12, 30, 0) },
             new Timeslot { Time = new DateTime(2000, 1, 1, 13, 0, 0) },
             new Timeslot { Time = new DateTime(2000, 1, 1, 13, 30, 0) },
             new Timeslot { Time = new DateTime(2000, 1, 1, 14, 0, 0) },
             new Timeslot { Time = new DateTime(2000, 1, 1, 14, 30, 0) },
             new Timeslot { Time = new DateTime(2000, 1, 1, 15, 0, 0) },
             new Timeslot { Time = new DateTime(2000, 1, 1, 15, 30, 0) },
             new Timeslot { Time = new DateTime(2000, 1, 1, 16, 0, 0) },
             new Timeslot { Time = new DateTime(2000, 1, 1, 16, 30, 0) },
             new Timeslot { Time = new DateTime(2000, 1, 1, 17, 0, 0) },
             new Timeslot { Time = new DateTime(2000, 1, 1, 17, 30, 0) },
             new Timeslot { Time = new DateTime(2000, 1, 1, 18, 0, 0) },
             new Timeslot { Time = new DateTime(2000, 1, 1, 18, 30, 0) },
             new Timeslot { Time = new DateTime(2000, 1, 1, 19, 0, 0) },
             new Timeslot { Time = new DateTime(2000, 1, 1, 19, 30, 0) },
             new Timeslot { Time = new DateTime(2000, 1, 1, 20, 0, 0) },
             new Timeslot { Time = new DateTime(2000, 1, 1, 20, 30, 0) },
             new Timeslot { Time = new DateTime(2000, 1, 1, 21, 0, 0) },
             new Timeslot { Time = new DateTime(2000, 1, 1, 21, 30, 0) },
             new Timeslot { Time = new DateTime(2000, 1, 1, 22, 0, 0) }
         );
            // seeding for Timeslot
            builder.Entity<SittingType>().HasData(
            new SittingType { Id = 1, Name = "Breakfast" },
            new SittingType { Id = 2, Name = "Lunch" },
            new SittingType { Id = 3, Name = "Dinner" }
        );
            // Add customisation for our models/entities
            base.OnModelCreating(builder);
        }
    }

}
