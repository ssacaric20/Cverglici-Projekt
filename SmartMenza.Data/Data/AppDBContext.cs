using Microsoft.EntityFrameworkCore;
using SmartMenza.Data.Models;
using SmartMenza.Core.Enums;

namespace SmartMenza.Data.Data
{
    public class AppDBContext : DbContext
    {
        // konstruktor
        public AppDBContext(DbContextOptions<AppDBContext> options)
            : base(options)
        {
        }

        // za svaku tablicu u bp
        public DbSet<RoleDto> Roles { get; set; } = null!;
        public DbSet<UserDto> Users { get; set; } = null!;
        public DbSet<DishDto> Dishes { get; set; } = null!;
        public DbSet<IngredientDto> Ingredients { get; set; } = null!;
        public DbSet<NutricionGoalDto> NutricionGoals { get; set; } = null!;
        public DbSet<DailyFoodIntakeDto> DailyFoodIntakes { get; set; } = null!;
        public DbSet<DailyMenuDto> DailyMenus { get; set; } = null!;

        // vise-vise veze
        public DbSet<DishIngredientDto> DishIngredients { get; set; } = null!;
        public DbSet<FavoriteDishDto> FavoriteDishes { get; set; } = null!;
        public DbSet<DishRatingDto> DishRatings { get; set; } = null!;
        public DbSet<DailyMenuDishDto> DailyMenuDishes { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Primary keys
            modelBuilder.Entity<RoleDto>().HasKey(r => r.roleId);
            modelBuilder.Entity<UserDto>().HasKey(u => u.userId);
            modelBuilder.Entity<IngredientDto>().HasKey(i => i.ingredientId);
            modelBuilder.Entity<DishDto>().HasKey(d => d.dishId);
            modelBuilder.Entity<DishRatingDto>().HasKey(dr => dr.dishRatingId);
            modelBuilder.Entity<NutricionGoalDto>().HasKey(ng => ng.nutricionalGoalId);
            modelBuilder.Entity<DailyFoodIntakeDto>().HasKey(dfi => dfi.dailyFoodIntakeId);
            modelBuilder.Entity<DailyMenuDto>().HasKey(dm => dm.dailyMenuId);

            // kompozitni
            modelBuilder.Entity<DishIngredientDto>()
                .HasKey(di => new { di.dishId, di.ingredientId });

            modelBuilder.Entity<FavoriteDishDto>()
                .HasKey(fd => new { fd.userId, fd.dishId });

            modelBuilder.Entity<DailyMenuDishDto>()
                .HasKey(dmd => new { dmd.dailyMenuId, dmd.dishId });

            // veze
            modelBuilder.Entity<DailyMenuDishDto>()
                .HasOne(dmd => dmd.dailyMenu)
                .WithMany(dm => dm.dailyMenuDishes)
                .HasForeignKey(dmd => dmd.dailyMenuId);

            modelBuilder.Entity<DailyMenuDishDto>()
                .HasOne(dmd => dmd.dish)
                .WithMany(d => d.dailyMenuDishes)
                .HasForeignKey(dmd => dmd.dishId);

            // seedanje
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

        // seed metode
        private void SeedRoles(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<RoleDto>().HasData(
                new RoleDto { roleId = 1, roleTitle = "Employee" },
                new RoleDto { roleId = 2, roleTitle = "Student" }
            );
        }

        private void SeedUsers(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserDto>().HasData(
                new UserDto
                {
                    userId = 1,
                    firstName = "Test",
                    lastName = "Employee",
                    email = "employee@test.com",
                    passwordHash = "pass123",
                    roleId = (int)UserRole.Employee
                },
                new UserDto
                {
                    userId = 2,
                    firstName = "Test",
                    lastName = "Student",
                    email = "student@test.com",
                    passwordHash = "pass123",
                    roleId = (int)UserRole.Student
                }
            );
        }

        private void SeedIngredients(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<IngredientDto>().HasData(
                new IngredientDto { ingredientId = 1, name = "Chicken breast" },
                new IngredientDto { ingredientId = 2, name = "Rice" },
                new IngredientDto { ingredientId = 3, name = "Pasta" },
                new IngredientDto { ingredientId = 4, name = "Mixed vegetables" }
            );
        }

        private void SeedDishes(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DishDto>().HasData(
                new DishDto
                {
                    dishId = 1,
                    title = "Chicken with rice",
                    description = "Boiled chicken with white rice",
                    price = 3.20m,
                    calories = 540,
                    protein = 35m,
                    carbohydrates = 50m,
                    fat = 12m,
                    imgPath = null
                },
                new DishDto
                {
                    dishId = 2,
                    title = "Vegetarian pasta",
                    description = "Pasta with vegetables",
                    price = 2.80m,
                    calories = 450,
                    protein = 15m,
                    carbohydrates = 70m,
                    fat = 8m,
                    imgPath = null
                },
                new DishDto
                {
                    dishId = 3,
                    title = "Grilled fish",
                    description = "Fresh grilled fish with lemon",
                    price = 4.50m,
                    calories = 320,
                    protein = 40m,
                    carbohydrates = 5m,
                    fat = 10m,
                    imgPath = null
                }
            );
        }

        private void SeedDishIngredients(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DishIngredientDto>().HasData(
                new DishIngredientDto { dishId = 1, ingredientId = 1 },
                new DishIngredientDto { dishId = 1, ingredientId = 2 },
                new DishIngredientDto { dishId = 2, ingredientId = 3 },
                new DishIngredientDto { dishId = 2, ingredientId = 4 }
            );
        }

        private void SeedFavoriteDishes(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<FavoriteDishDto>().HasData(
                new FavoriteDishDto { userId = 2, dishId = 1 },
                new FavoriteDishDto { userId = 2, dishId = 2 }
            );
        }

        private void SeedDishRatings(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DishRatingDto>().HasData(
                new DishRatingDto { dishRatingId = 1, dishId = 1, rating = 5 },
                new DishRatingDto { dishRatingId = 2, dishId = 2, rating = 4 }
            );
        }

        private void SeedNutricionGoals(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<NutricionGoalDto>().HasData(
                new NutricionGoalDto
                {
                    nutricionalGoalId = 1,
                    userId = 2,
                    caloriesGoal = 2000,
                    proteinsGoal = 120m,
                    fatsGoal = 70m,
                    carbohydratesGoal = 250m,
                    goalSetDate = new DateTime(2025, 1, 1)
                }
            );
        }

        private void SeedDailyFoodIntakes(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DailyFoodIntakeDto>().HasData(
                new DailyFoodIntakeDto
                {
                    dailyFoodIntakeId = 1,
                    userId = 2,
                    dishId = 1,
                    date = new DateTime(2025, 1, 1)
                },
                new DailyFoodIntakeDto
                {
                    dailyFoodIntakeId = 2,
                    userId = 2,
                    dishId = 2,
                    date = new DateTime(2025, 1, 1)
                }
            );
        }

        private void SeedDailyMenus(ModelBuilder modelBuilder)
        {
            var seedMenus = new List<DailyMenuDto>();
            int menuId = 1;

            // start i end date u kojem ce se nasumicno generirat menus
            var startDate = new DateOnly(2025, 12, 3);
            var endDate = new DateOnly(2025, 12, 27);

            for (var date = startDate; date <= endDate; date = date.AddDays(1))
            {
                // rucak
                seedMenus.Add(new DailyMenuDto
                {
                    dailyMenuId = menuId++,
                    date = date,
                    category = (int)MenuCategory.Lunch
                });

                // vecera
                seedMenus.Add(new DailyMenuDto
                {
                    dailyMenuId = menuId++,
                    date = date,
                    category = (int)MenuCategory.Dinner
                });
            }

            modelBuilder.Entity<DailyMenuDto>().HasData(seedMenus.ToArray());
        }

        private void SeedDailyMenuDishes(ModelBuilder modelBuilder)
        {
            var seedRelations = new List<DailyMenuDishDto>();

            int menuId = 1;
            var startDate = new DateOnly(2025, 12, 3);
            var endDate = new DateOnly(2025, 12, 27);

            int patternDay = 0;
            for (var date = startDate; date <= endDate; date = date.AddDays(1))
            {
                // rucak
                switch (patternDay % 5)
                {
                    case 0: // sva 3 jela
                        seedRelations.Add(new DailyMenuDishDto { dailyMenuId = menuId, dishId = 1 });
                        seedRelations.Add(new DailyMenuDishDto { dailyMenuId = menuId, dishId = 2 });
                        seedRelations.Add(new DailyMenuDishDto { dailyMenuId = menuId, dishId = 3 });
                        break;
                    case 1: // chicken pasta
                        seedRelations.Add(new DailyMenuDishDto { dailyMenuId = menuId, dishId = 1 });
                        seedRelations.Add(new DailyMenuDishDto { dailyMenuId = menuId, dishId = 2 });
                        break;
                    case 2: // pasta i fish
                        seedRelations.Add(new DailyMenuDishDto { dailyMenuId = menuId, dishId = 2 });
                        seedRelations.Add(new DailyMenuDishDto { dailyMenuId = menuId, dishId = 3 });
                        break;
                    case 3: // chicken i fish
                        seedRelations.Add(new DailyMenuDishDto { dailyMenuId = menuId, dishId = 1 });
                        seedRelations.Add(new DailyMenuDishDto { dailyMenuId = menuId, dishId = 3 });
                        break;
                    case 4: // sva 3 jela
                        seedRelations.Add(new DailyMenuDishDto { dailyMenuId = menuId, dishId = 1 });
                        seedRelations.Add(new DailyMenuDishDto { dailyMenuId = menuId, dishId = 2 });
                        seedRelations.Add(new DailyMenuDishDto { dailyMenuId = menuId, dishId = 3 });
                        break;
                }

                menuId++; // prijedji na veceru

                // rucak menu
                switch (patternDay % 5)
                {
                    case 0: // pasta fish
                        seedRelations.Add(new DailyMenuDishDto { dailyMenuId = menuId, dishId = 2 });
                        seedRelations.Add(new DailyMenuDishDto { dailyMenuId = menuId, dishId = 3 });
                        break;
                    case 1: // sva 3 jela
                        seedRelations.Add(new DailyMenuDishDto { dailyMenuId = menuId, dishId = 1 });
                        seedRelations.Add(new DailyMenuDishDto { dailyMenuId = menuId, dishId = 2 });
                        seedRelations.Add(new DailyMenuDishDto { dailyMenuId = menuId, dishId = 3 });
                        break;
                    case 2: // chicken i fish
                        seedRelations.Add(new DailyMenuDishDto { dailyMenuId = menuId, dishId = 1 });
                        seedRelations.Add(new DailyMenuDishDto { dailyMenuId = menuId, dishId = 3 });
                        break;
                    case 3: // sva 3
                        seedRelations.Add(new DailyMenuDishDto { dailyMenuId = menuId, dishId = 1 });
                        seedRelations.Add(new DailyMenuDishDto { dailyMenuId = menuId, dishId = 2 });
                        seedRelations.Add(new DailyMenuDishDto { dailyMenuId = menuId, dishId = 3 });
                        break;
                    case 4: // chicken i pasta
                        seedRelations.Add(new DailyMenuDishDto { dailyMenuId = menuId, dishId = 1 });
                        seedRelations.Add(new DailyMenuDishDto { dailyMenuId = menuId, dishId = 2 });
                        break;
                }

                menuId++; // prijedji na rucak
                patternDay++;
            }

            modelBuilder.Entity<DailyMenuDishDto>().HasData(seedRelations.ToArray());
        }
    }
}