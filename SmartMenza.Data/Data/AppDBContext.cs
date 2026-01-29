using Microsoft.EntityFrameworkCore;
using SmartMenza.Core.Enums;
using SmartMenza.Data.Entities;

namespace SmartMenza.Data.Data
{
    public class AppDBContext : DbContext
    {
        
        public AppDBContext(DbContextOptions<AppDBContext> options)
            : base(options)
        {
        }

        public DbSet<Role> Roles { get; set; } = null!;
        public DbSet<User> Users { get; set; } = null!;
        public DbSet<Dish> Dishes { get; set; } = null!;
        public DbSet<Ingredient> Ingredients { get; set; } = null!;
        public DbSet<NutricionGoal> NutricionGoals { get; set; } = null!;
        public DbSet<DailyFoodIntake> DailyFoodIntakes { get; set; } = null!;
        public DbSet<DailyMenu> DailyMenus { get; set; } = null!;

        
        public DbSet<DishIngredient> DishIngredients { get; set; } = null!;
        public DbSet<FavoriteDish> FavoriteDishes { get; set; } = null!;
        public DbSet<DishRating> DishRatings { get; set; } = null!;
        public DbSet<DailyMenuDish> DailyMenuDishes { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            // DECIMAL PRECISION CONFIGURATION
            modelBuilder.Entity<Dish>(entity =>
            {
                entity.Property(x => x.Price).HasPrecision(18, 2);
                entity.Property(x => x.Protein).HasPrecision(18, 2);
                entity.Property(x => x.Fat).HasPrecision(18, 2);
                entity.Property(x => x.Carbohydrates).HasPrecision(18, 2);
                entity.Property(x => x.Fiber).HasPrecision(18, 2);
            });

            modelBuilder.Entity<NutricionGoal>(entity =>
            {
                entity.Property(x => x.ProteinsGoal).HasPrecision(18, 2);
                entity.Property(x => x.FatsGoal).HasPrecision(18, 2);
                entity.Property(x => x.CarbohydratesGoal).HasPrecision(18, 2);
            });


            modelBuilder.Entity<Role>().HasKey(r => r.RoleId);
            modelBuilder.Entity<User>().HasKey(u => u.UserId);
            modelBuilder.Entity<Ingredient>().HasKey(i => i.IngredientId);
            modelBuilder.Entity<Dish>().HasKey(d => d.DishId);
            modelBuilder.Entity<DishRating>().HasKey(dr => dr.DishRatingId);
            modelBuilder.Entity<NutricionGoal>().HasKey(ng => ng.NutricionalGoalId);
            modelBuilder.Entity<DailyFoodIntake>().HasKey(dfi => dfi.DailyFoodIntakeId);
            modelBuilder.Entity<DailyMenu>().HasKey(dm => dm.DailyMenuId);

            
            modelBuilder.Entity<DishIngredient>()
                .HasKey(di => new { di.DishId, di.IngredientId });


            modelBuilder.Entity<FavoriteDish>(entity =>
            {
                entity.HasKey(fd => new { fd.UserId, fd.DishId });

                entity.HasOne(fd => fd.User)
                      .WithMany(u => u.FavoriteDishes)
                      .HasForeignKey(fd => fd.UserId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(fd => fd.Dish)
                      .WithMany(d => d.FavoriteDishes)
                      .HasForeignKey(fd => fd.DishId)
                      .OnDelete(DeleteBehavior.Cascade);
            });


            modelBuilder.Entity<DailyMenuDish>()
                .HasKey(dmd => new { dmd.DailyMenuId, dmd.DishId });

           
            modelBuilder.Entity<DailyMenuDish>()
                .HasOne(dmd => dmd.DailyMenu)
                .WithMany(dm => dm.DailyMenuDishes)
                .HasForeignKey(dmd => dmd.DailyMenuId);

            modelBuilder.Entity<DailyMenuDish>()
                .HasOne(dmd => dmd.Dish)
                .WithMany(d => d.DailyMenuDishes)
                .HasForeignKey(dmd => dmd.DishId);

            




            SeedRoles(modelBuilder);
            SeedUsers(modelBuilder);
            SeedIngredients(modelBuilder);
            SeedDishes(modelBuilder);
            SeedDishIngredients(modelBuilder);
            SeedFavoriteDishes(modelBuilder);
            SeedDishRatings(modelBuilder);
            SeedNutricionGoals(modelBuilder);
            SeedDailyFoodIntakes(modelBuilder);
            SeedDailyMenus(modelBuilder);
            SeedDailyMenuDishes(modelBuilder);
        }

        
        private void SeedRoles(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Role>().HasData(
                new Role { RoleId = 1, RoleTitle = "Employee" },
                new Role { RoleId = 2, RoleTitle = "Student" }
            );
        }

        private void SeedUsers(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasData(
                new User
                {
                    UserId = 1,
                    FirstName = "Test",
                    LastName = "Employee",
                    Email = "employee@test.com",
                    PasswordHash = "pass123",
                    RoleId = (int)UserRole.Employee
                },
                new User
                {
                    UserId = 2,
                    FirstName = "Test",
                    LastName = "Student",
                    Email = "student@test.com",
                    PasswordHash = "pass123",
                    RoleId = (int)UserRole.Student
                }
            );
        }

        private void SeedIngredients(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Ingredient>().HasData(
                new Ingredient { IngredientId = 1, Name = "Chicken breast" },
                new Ingredient { IngredientId = 2, Name = "Rice" },
                new Ingredient { IngredientId = 3, Name = "Pasta" },
                new Ingredient { IngredientId = 4, Name = "Mixed vegetables" }
            );
        }

        private void SeedDishes(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Dish>().HasData(
                new Dish
                {
                    DishId = 1,
                    Title = "Chicken with rice",
                    Description = "Boiled chicken with white rice",
                    Price = 3.20m,
                    Calories = 540,
                    Protein = 35m,
                    Carbohydrates = 50m,
                    Fiber = 30m,
                    Fat = 12m,
                    ImgPath = null
                },
                new Dish
                {
                    DishId = 2,
                    Title = "Vegetarian pasta",
                    Description = "Pasta with vegetables",
                    Price = 2.80m,
                    Calories = 450,
                    Protein = 15m,
                    Carbohydrates = 70m,
                    Fiber = 30m,
                    Fat = 8m,
                    ImgPath = null
                },
                new Dish
                {
                    DishId = 3,
                    Title = "Grilled fish",
                    Description = "Fresh grilled fish with lemon",
                    Price = 4.50m,
                    Calories = 320,
                    Protein = 40m,
                    Carbohydrates = 5m,
                    Fiber = 0m,
                    Fat = 10m,
                    ImgPath = null
                }
            );
        }

        private void SeedDishIngredients(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DishIngredient>().HasData(
                new DishIngredient { DishId = 1, IngredientId = 1 },
                new DishIngredient { DishId = 1, IngredientId = 2 },
                new DishIngredient { DishId = 2, IngredientId = 3 },
                new DishIngredient { DishId = 2, IngredientId = 4 }
            );
        }

        private void SeedFavoriteDishes(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<FavoriteDish>().HasData(
                new FavoriteDish { UserId = 2, DishId = 1 },
                new FavoriteDish { UserId = 2, DishId = 2 }
            );
        }

        private void SeedDishRatings(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DishRating>().HasData(
                new DishRating { DishRatingId = 1, DishId = 1, Rating = 5 },
                new DishRating { DishRatingId = 2, DishId = 2, Rating = 4 },
                new DishRating { DishRatingId = 3, DishId = 3, Rating = 3 },
                new DishRating { DishRatingId = 4, DishId = 3, Rating = 4 },
                new DishRating { DishRatingId = 5, DishId = 3, Rating = 4 }
            );
        }

        private void SeedNutricionGoals(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<NutricionGoal>().HasData(
                new NutricionGoal
                {
                    NutricionalGoalId = 1,
                    UserId = 2,
                    CaloriesGoal = 2000,
                    ProteinsGoal = 120m,
                    FatsGoal = 70m,
                    CarbohydratesGoal = 250m,
                    GoalSetDate = new DateTime(2025, 1, 1)
                }
            );
        }

        private void SeedDailyFoodIntakes(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DailyFoodIntake>().HasData(
                new DailyFoodIntake
                {
                    DailyFoodIntakeId = 1,
                    UserId = 2,
                    DishId = 1,
                    Date = new DateTime(2025, 1, 1)
                },
                new DailyFoodIntake
                {
                    DailyFoodIntakeId = 2,
                    UserId = 2,
                    DishId = 2,
                    Date = new DateTime(2025, 1, 1)
                }
            );
        }

        private void SeedDailyMenus(ModelBuilder modelBuilder)
        {
            var seedMenus = new List<DailyMenu>();
            int menuId = 1;

            // start i end date u kojem ce se nasumicno generirat menus
            var startDate = new DateOnly(2025, 12, 3);
            var endDate = new DateOnly(2025, 12, 27);

            for (var date = startDate; date <= endDate; date = date.AddDays(1))
            {
                
                seedMenus.Add(new DailyMenu
                {
                    DailyMenuId = menuId++,
                    Date = date,
                    Category = (int)MenuCategory.Lunch
                });

               
                seedMenus.Add(new DailyMenu
                {
                    DailyMenuId = menuId++,
                    Date = date,
                    Category = (int)MenuCategory.Dinner
                });
            }

            modelBuilder.Entity<DailyMenu>().HasData(seedMenus.ToArray());
        }

        private void SeedDailyMenuDishes(ModelBuilder modelBuilder)
        {
            var seedRelations = new List<DailyMenuDish>();

            int menuId = 1; // rucak
            var startDate = new DateOnly(2025, 12, 3);
            var endDate = new DateOnly(2025, 12, 27);

            int patternDay = 0;
            for (var date = startDate; date <= endDate; date = date.AddDays(1))
            {
                
                switch (patternDay % 5)
                {
                    case 0: 
                        seedRelations.Add(new DailyMenuDish { DailyMenuId = menuId, DishId = 1 });
                        seedRelations.Add(new DailyMenuDish { DailyMenuId = menuId, DishId = 2 });
                        seedRelations.Add(new DailyMenuDish { DailyMenuId = menuId, DishId = 3 });
                        break;
                    case 1: 
                        seedRelations.Add(new DailyMenuDish { DailyMenuId = menuId, DishId = 1 });
                        seedRelations.Add(new DailyMenuDish { DailyMenuId = menuId, DishId = 2 });
                        break;
                    case 2: 
                        seedRelations.Add(new DailyMenuDish { DailyMenuId = menuId, DishId = 2 });
                        seedRelations.Add(new DailyMenuDish { DailyMenuId = menuId, DishId = 3 });
                        break;
                    case 3: 
                        seedRelations.Add(new DailyMenuDish { DailyMenuId = menuId, DishId = 1 });
                        seedRelations.Add(new DailyMenuDish { DailyMenuId = menuId, DishId = 3 });
                        break;
                    case 4: 
                        seedRelations.Add(new DailyMenuDish { DailyMenuId = menuId, DishId = 1 });
                        seedRelations.Add(new DailyMenuDish { DailyMenuId = menuId, DishId = 2 });
                        seedRelations.Add(new DailyMenuDish { DailyMenuId = menuId, DishId = 3 });
                        break;
                }

                menuId++; // prijedji na veceru

                
                switch (patternDay % 5)
                {
                    case 0: 
                        seedRelations.Add(new DailyMenuDish { DailyMenuId = menuId, DishId = 2 });
                        seedRelations.Add(new DailyMenuDish { DailyMenuId = menuId, DishId = 3 });
                        break;
                    case 1: 
                        seedRelations.Add(new DailyMenuDish { DailyMenuId = menuId, DishId = 1 });
                        seedRelations.Add(new DailyMenuDish { DailyMenuId = menuId, DishId = 2 });
                        seedRelations.Add(new DailyMenuDish { DailyMenuId = menuId, DishId = 3 });
                        break;
                    case 2: 
                        seedRelations.Add(new DailyMenuDish { DailyMenuId = menuId, DishId = 1 });
                        seedRelations.Add(new DailyMenuDish { DailyMenuId = menuId, DishId = 3 });
                        break;
                    case 3: 
                        seedRelations.Add(new DailyMenuDish { DailyMenuId = menuId, DishId = 1 });
                        seedRelations.Add(new DailyMenuDish { DailyMenuId = menuId, DishId = 2 });
                        seedRelations.Add(new DailyMenuDish { DailyMenuId = menuId, DishId = 3 });
                        break;
                    case 4: 
                        seedRelations.Add(new DailyMenuDish { DailyMenuId = menuId, DishId = 1 });
                        seedRelations.Add(new DailyMenuDish { DailyMenuId = menuId, DishId = 2 });
                        break;
                }

                menuId++; // prijedji na rucak
                patternDay++;
            }

            modelBuilder.Entity<DailyMenuDish>().HasData(seedRelations.ToArray());
        }
    }
}