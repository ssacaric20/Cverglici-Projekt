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
            // 1. komponentni kljuc za JeloSastojak 
            modelBuilder.Entity<RoleDto>()
        .HasKey(r => r.roleId);

            modelBuilder.Entity<UserDto>()
                .HasKey(u => u.userId);

            modelBuilder.Entity<IngredientDto>()
                .HasKey(i => i.ingredientId);

            modelBuilder.Entity<DishDto>()
                .HasKey(d => d.dishId);

            modelBuilder.Entity<DishRatingDto>()
                .HasKey(dr => dr.dishRatingId);

            modelBuilder.Entity<NutricionGoalDto>()
                .HasKey(ng => ng.nutricionalGoalId);

            modelBuilder.Entity<DailyFoodIntakeDto>()
                .HasKey(dfi => dfi.dailyFoodIntakeId);

            modelBuilder.Entity<DailyMenuDto>()
                .HasKey(dm => dm.dailyMenuId);

            // 2, komponentni kljucevi
            modelBuilder.Entity<DishIngredientDto>()
                .HasKey(di => new { di.dishId, di.ingredientId });

            modelBuilder.Entity<FavoriteDishDto>()
                .HasKey(fd => new { fd.userId, fd.dishId });

            modelBuilder.Entity<DailyMenuDishDto>()
                .HasOne(dmd => dmd.dailyMenu)
                .WithMany(dm => dm.dailyMenuDishes)
                .HasForeignKey(dmd => dmd.dailyMenuId);

            // NOVO: dailymenudish dvokomponentni kljuc
            modelBuilder.Entity<DailyMenuDishDto>()
                .HasKey(dmd => new { dmd.dailyMenuId, dmd.dishId });

            modelBuilder.Entity<DailyMenuDishDto>()
                .HasOne(dmd => dmd.dailyMenu)
                .WithMany(dm => dm.dailyMenuDishes)
                .HasForeignKey(dmd => dmd.dailyMenuId);

            // Seed data
            // Roles
            modelBuilder.Entity<RoleDto>().HasData(
                new RoleDto { roleId = 1, roleTitle = "Employee" },
                new RoleDto { roleId = 2, roleTitle = "Student" }
            );

            // Users
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

            // Ingredients
            modelBuilder.Entity<IngredientDto>().HasData(
                new IngredientDto { ingredientId = 1, name = "Chicken breast" },
                new IngredientDto { ingredientId = 2, name = "Rice" },
                new IngredientDto { ingredientId = 3, name = "Pasta" },
                new IngredientDto { ingredientId = 4, name = "Mixed vegetables" }
            );

            // Dishes - REMOVED nutricionalValueId
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


            // Dish–Ingredient (many-to-many)
            modelBuilder.Entity<DishIngredientDto>().HasData(
                new DishIngredientDto { dishId = 1, ingredientId = 1 },
                new DishIngredientDto { dishId = 1, ingredientId = 2 },
                new DishIngredientDto { dishId = 2, ingredientId = 3 },
                new DishIngredientDto { dishId = 2, ingredientId = 4 }
            );

            // Favorites (user–dish many-to-many)
            modelBuilder.Entity<FavoriteDishDto>().HasData(
                new FavoriteDishDto { userId = 2, dishId = 1 },
                new FavoriteDishDto { userId = 2, dishId = 2 }
            );

            // Dish ratings
            modelBuilder.Entity<DishRatingDto>().HasData(
                new DishRatingDto { dishRatingId = 1, dishId = 1, rating = 5 },
                new DishRatingDto { dishRatingId = 2, dishId = 2, rating = 4 }
            );

            // Nutrition goals
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

            // Daily food intake
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


            // Daily menu
            modelBuilder.Entity<DailyMenuDto>().HasData(
                new DailyMenuDto
                {
                    dailyMenuId = 1,
                    date = new DateOnly(2025, 11, 29)
                },
                new DailyMenuDto
                {
                    dailyMenuId = 2,
                    date = new DateOnly(2025, 12, 2)
                },
                new DailyMenuDto
                {
                    dailyMenuId = 3,
                    date = new DateOnly(2025, 12, 1)
                }
            );

            modelBuilder.Entity<DailyMenuDishDto>().HasData(
                // 29.11.2025 - vege pasta i chicken
                new DailyMenuDishDto { dailyMenuId = 1, dishId = 2 },
                new DailyMenuDishDto { dailyMenuId = 1, dishId = 1 },

                // 30.11.2025 - sva tri jela
                new DailyMenuDishDto { dailyMenuId = 2, dishId = 1 },
                new DailyMenuDishDto { dailyMenuId = 2, dishId = 2 },
                new DailyMenuDishDto { dailyMenuId = 2, dishId = 3 },

                // 01.12.2025 - grilled fish
                new DailyMenuDishDto { dailyMenuId = 3, dishId = 3 }
            );
        }
    }
}