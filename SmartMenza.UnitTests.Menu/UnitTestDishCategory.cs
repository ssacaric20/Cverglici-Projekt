using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartMenza.API.Controllers;
using SmartMenza.Business.Models.DailyMenu;
using SmartMenza.Business.Services;
using SmartMenza.Core.Enums;
using SmartMenza.Data.Data;
using SmartMenza.Data.Models;

namespace SmartMenza.UnitTests.Menu
{
    public class UnitTestMenuCategories
    {
        private readonly AppDBContext _context;
        private readonly DailyMenuController _dailyMenuController;
        private readonly DailyMenuServices _dailyMenuServices;

        public UnitTestMenuCategories()
        {
            var options = new DbContextOptionsBuilder<AppDBContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new AppDBContext(options);
            SeedTestData();

            _dailyMenuServices = new DailyMenuServices(_context);
            _dailyMenuController = new DailyMenuController(_dailyMenuServices);
        }

        private void SeedTestData()
        {
            var dish1 = new DishDto
            {
                dishId = 1,
                title = "Chicken with rice",
                description = "Boiled chicken with white rice",
                price = 3.20m,
                calories = 540,
                protein = 35m,
                fiber = 30m,
                carbohydrates = 50m,
                fat = 12m,
                imgPath = null
            };

            var dish2 = new DishDto
            {
                dishId = 2,
                title = "Vegetarian pasta",
                description = "Pasta with vegetables",
                price = 2.80m,
                calories = 450,
                protein = 15m,
                fiber = 40m,
                carbohydrates = 70m,
                fat = 8m,
                imgPath = null
            };

            var dish3 = new DishDto
            {
                dishId = 3,
                title = "Grilled fish",
                description = "Fresh grilled fish with lemon",
                price = 4.50m,
                calories = 320,
                protein = 40m,
                fiber = 0m,
                carbohydrates = 5m,
                fat = 10m,
                imgPath = null
            };

            _context.Dishes.AddRange(dish1, dish2, dish3);

            
            var todayLunchMenu = new DailyMenuDto
            {
                dailyMenuId = 1,
                date = DateOnly.FromDateTime(DateTime.Today),
                category = (int)MenuCategory.Lunch
            };

            var todayDinnerMenu = new DailyMenuDto
            {
                dailyMenuId = 2,
                date = DateOnly.FromDateTime(DateTime.Today),
                category = (int)MenuCategory.Dinner
            };

            
            var fixedLunchMenu = new DailyMenuDto
            {
                dailyMenuId = 3,
                date = new DateOnly(2025, 1, 22),
                category = (int)MenuCategory.Lunch
            };

            var fixedDinnerMenu = new DailyMenuDto
            {
                dailyMenuId = 4,
                date = new DateOnly(2025, 1, 22),
                category = (int)MenuCategory.Dinner
            };

            _context.DailyMenus.AddRange(todayLunchMenu, todayDinnerMenu, fixedLunchMenu, fixedDinnerMenu);
            _context.SaveChanges();

            
            _context.DailyMenuDishes.AddRange(
                new DailyMenuDishDto { dailyMenuId = 1, dishId = 1 },
                new DailyMenuDishDto { dailyMenuId = 1, dishId = 2 },

                new DailyMenuDishDto { dailyMenuId = 2, dishId = 2 },
                new DailyMenuDishDto { dailyMenuId = 2, dishId = 3 },

                new DailyMenuDishDto { dailyMenuId = 3, dishId = 1 },
                new DailyMenuDishDto { dailyMenuId = 3, dishId = 2 },
                new DailyMenuDishDto { dailyMenuId = 3, dishId = 3 },

                new DailyMenuDishDto { dailyMenuId = 4, dishId = 3 }
            );

            _context.SaveChanges();
        }

        

        [Fact]
        public async Task GetTodaysMenuAsync_NoCategory_ReturnsBothLunchAndDinner()
        {
           
            var result = await _dailyMenuController.GetTodaysMenuAsync(null);

           
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var menus = Assert.IsAssignableFrom<IEnumerable<DailyMenuListItemResponse>>(okResult.Value);
            var menuList = menus.ToList();

            Assert.NotNull(menuList);
            Assert.Equal(4, menuList.Count);

            
            Assert.Contains(menuList, m => m.Category == "Lunch");
            Assert.Contains(menuList, m => m.Category == "Dinner");
        }

        

        [Fact]
        public async Task GetTodaysMenuAsync_LunchCategory_ReturnsOnlyLunchDishes()
        {
            var result = await _dailyMenuController.GetTodaysMenuAsync("lunch");

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var menus = Assert.IsAssignableFrom<IEnumerable<DailyMenuListItemResponse>>(okResult.Value);
            var menuList = menus.ToList();

            Assert.NotNull(menuList);
            Assert.Equal(2, menuList.Count); 
            Assert.All(menuList, m => Assert.Equal("Lunch", m.Category));

           
            Assert.Contains(menuList, m => m.Jelo.Title == "Chicken with rice");
            Assert.Contains(menuList, m => m.Jelo.Title == "Vegetarian pasta");
        }

        

        [Fact]
        public async Task GetTodaysMenuAsync_DinnerCategory_ReturnsOnlyDinnerDishes()
        {
            var result = await _dailyMenuController.GetTodaysMenuAsync("dinner");

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var menus = Assert.IsAssignableFrom<IEnumerable<DailyMenuListItemResponse>>(okResult.Value);
            var menuList = menus.ToList();

            Assert.NotNull(menuList);
            Assert.Equal(2, menuList.Count);
            Assert.All(menuList, m => Assert.Equal("Dinner", m.Category));

            Assert.Contains(menuList, m => m.Jelo.Title == "Vegetarian pasta");
            Assert.Contains(menuList, m => m.Jelo.Title == "Grilled fish");
        }

        

        [Fact]
        public async Task GetTodaysMenuAsync_InvalidCategory_ReturnsBadRequest()
        {
            var result = await _dailyMenuController.GetTodaysMenuAsync("breakfast");

            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.NotNull(badRequestResult.Value);
        }

        

        [Theory]
        [InlineData("lunch")]
        [InlineData("Lunch")]
        [InlineData("LUNCH")]
        [InlineData("LuNcH")]
        public async Task GetTodaysMenuAsync_CategoryIsCaseInsensitive(string category)
        {
            var result = await _dailyMenuController.GetTodaysMenuAsync(category);

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var menus = Assert.IsAssignableFrom<IEnumerable<DailyMenuListItemResponse>>(okResult.Value);
            var menuList = menus.ToList();

            Assert.NotNull(menuList);
            Assert.All(menuList, m => Assert.Equal("Lunch", m.Category));
        }

       

        [Fact]
        public async Task GetMenuForDateAsync_WithLunchCategory_ReturnsOnlyLunch()
        {
            string date = "2025-01-22";

            var result = await _dailyMenuController.GetMenuForDateAsync(date, "lunch");

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var menus = Assert.IsAssignableFrom<IEnumerable<DailyMenuListItemResponse>>(okResult.Value);
            var menuList = menus.ToList();

            Assert.NotNull(menuList);
            Assert.Equal(3, menuList.Count);
            Assert.All(menuList, m => Assert.Equal("Lunch", m.Category));
        }

        [Fact]
        public async Task GetMenuForDateAsync_WithDinnerCategory_ReturnsOnlyDinner()
        {
            string date = "2025-01-22";

            var result = await _dailyMenuController.GetMenuForDateAsync(date, "dinner");

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var menus = Assert.IsAssignableFrom<IEnumerable<DailyMenuListItemResponse>>(okResult.Value);
            var menuList = menus.ToList();

            Assert.NotNull(menuList);
            Assert.Single(menuList);
            Assert.All(menuList, m => Assert.Equal("Dinner", m.Category));
            Assert.Equal("Grilled fish", menuList.First().Jelo.Title);
        }

       

        [Fact]
        public async Task GetTodaysMenusGroupedAsync_ReturnsSeparateLunchAndDinner()
        {
            var result = await _dailyMenuController.GetTodaysMenusGroupedAsync();

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var grouped = Assert.IsType<MenusByCategoryResponse>(okResult.Value);

            Assert.NotNull(grouped);
            Assert.NotNull(grouped.Lunch);
            Assert.NotNull(grouped.Dinner);

           
            Assert.Equal(2, grouped.Lunch.Count);
            Assert.Contains(grouped.Lunch, m => m.Jelo.Title == "Chicken with rice");
            Assert.Contains(grouped.Lunch, m => m.Jelo.Title == "Vegetarian pasta");

            
            Assert.Equal(2, grouped.Dinner.Count);
            Assert.Contains(grouped.Dinner, m => m.Jelo.Title == "Vegetarian pasta");
            Assert.Contains(grouped.Dinner, m => m.Jelo.Title == "Grilled fish");
        }

       

        [Fact]
        public async Task VerifyDishCanAppearInBothLunchAndDinner()
        {
            var result = await _dailyMenuController.GetTodaysMenuAsync(null);

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var menus = Assert.IsAssignableFrom<IEnumerable<DailyMenuListItemResponse>>(okResult.Value);
            var menuList = menus.ToList();

            // jelo se pojavljuje u obje kategorije
            var pastaInLunch = menuList.Where(m =>
                m.Jelo.Title == "Vegetarian pasta" && m.Category == "Lunch");
            var pastaInDinner = menuList.Where(m =>
                m.Jelo.Title == "Vegetarian pasta" && m.Category == "Dinner");

            Assert.Single(pastaInLunch);
            Assert.Single(pastaInDinner);
        }

       

        [Fact]
        public async Task GetMenuForDateAsync_DateWithOnlyLunch_DinnerReturnsEmpty()
        {
            
            var onlyLunchMenu = new DailyMenuDto
            {
                dailyMenuId = 5,
                date = new DateOnly(2025, 2, 1),
                category = (int)MenuCategory.Lunch
            };

            _context.DailyMenus.Add(onlyLunchMenu);
            _context.DailyMenuDishes.Add(new DailyMenuDishDto { dailyMenuId = 5, dishId = 1 });
            _context.SaveChanges();

            
            var result = await _dailyMenuController.GetMenuForDateAsync("2025-02-01", "dinner");

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var menus = Assert.IsAssignableFrom<IEnumerable<DailyMenuListItemResponse>>(okResult.Value);

            Assert.Empty(menus);
        }

        
        [Fact]
        public async Task GetTodaysMenuAsync_ResponseAlwaysIncludesCategory()
        {
            var result = await _dailyMenuController.GetTodaysMenuAsync(null);

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var menus = Assert.IsAssignableFrom<IEnumerable<DailyMenuListItemResponse>>(okResult.Value);

            Assert.All(menus, menu =>
            {
                Assert.NotNull(menu.Category);
                Assert.NotEmpty(menu.Category);
                Assert.True(menu.Category == "Lunch" || menu.Category == "Dinner");
            });
        }
    }
}