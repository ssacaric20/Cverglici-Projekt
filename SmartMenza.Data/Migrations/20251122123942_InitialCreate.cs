using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace SmartMenza.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Dishes",
                columns: table => new
                {
                    dishId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    calories = table.Column<int>(type: "int", nullable: false),
                    protein = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    fat = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    carbohydrates = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    imgPath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    nutricionalValueId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Dishes", x => x.dishId);
                });

            migrationBuilder.CreateTable(
                name: "Ingredients",
                columns: table => new
                {
                    ingredientId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ingredients", x => x.ingredientId);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    roleId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    roleTitle = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.roleId);
                });

            migrationBuilder.CreateTable(
                name: "DailyMenus",
                columns: table => new
                {
                    dailyMenuId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    date = table.Column<DateOnly>(type: "date", nullable: false),
                    dishId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DailyMenus", x => x.dailyMenuId);
                    table.ForeignKey(
                        name: "FK_DailyMenus_Dishes_dishId",
                        column: x => x.dishId,
                        principalTable: "Dishes",
                        principalColumn: "dishId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DishRatings",
                columns: table => new
                {
                    dishRatingId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    rating = table.Column<int>(type: "int", nullable: false),
                    dishId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DishRatings", x => x.dishRatingId);
                    table.ForeignKey(
                        name: "FK_DishRatings_Dishes_dishId",
                        column: x => x.dishId,
                        principalTable: "Dishes",
                        principalColumn: "dishId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DishIngredients",
                columns: table => new
                {
                    dishId = table.Column<int>(type: "int", nullable: false),
                    ingredientId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DishIngredients", x => new { x.dishId, x.ingredientId });
                    table.ForeignKey(
                        name: "FK_DishIngredients_Dishes_dishId",
                        column: x => x.dishId,
                        principalTable: "Dishes",
                        principalColumn: "dishId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DishIngredients_Ingredients_ingredientId",
                        column: x => x.ingredientId,
                        principalTable: "Ingredients",
                        principalColumn: "ingredientId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    userId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    passwordHash = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    firstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    lastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    roleId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.userId);
                    table.ForeignKey(
                        name: "FK_Users_Roles_roleId",
                        column: x => x.roleId,
                        principalTable: "Roles",
                        principalColumn: "roleId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DailyFoodIntakes",
                columns: table => new
                {
                    dailyFoodIntakeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    userId = table.Column<int>(type: "int", nullable: false),
                    dishId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DailyFoodIntakes", x => x.dailyFoodIntakeId);
                    table.ForeignKey(
                        name: "FK_DailyFoodIntakes_Dishes_dishId",
                        column: x => x.dishId,
                        principalTable: "Dishes",
                        principalColumn: "dishId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DailyFoodIntakes_Users_userId",
                        column: x => x.userId,
                        principalTable: "Users",
                        principalColumn: "userId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FavoriteDishes",
                columns: table => new
                {
                    userId = table.Column<int>(type: "int", nullable: false),
                    dishId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FavoriteDishes", x => new { x.userId, x.dishId });
                    table.ForeignKey(
                        name: "FK_FavoriteDishes_Dishes_dishId",
                        column: x => x.dishId,
                        principalTable: "Dishes",
                        principalColumn: "dishId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FavoriteDishes_Users_userId",
                        column: x => x.userId,
                        principalTable: "Users",
                        principalColumn: "userId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "NutricionGoals",
                columns: table => new
                {
                    nutricionalGoalId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    caloriesGoal = table.Column<int>(type: "int", nullable: false),
                    proteinsGoal = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    fatsGoal = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    carbohydratesGoal = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    goalSetDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    userId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NutricionGoals", x => x.nutricionalGoalId);
                    table.ForeignKey(
                        name: "FK_NutricionGoals_Users_userId",
                        column: x => x.userId,
                        principalTable: "Users",
                        principalColumn: "userId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Dishes",
                columns: new[] { "dishId", "calories", "carbohydrates", "description", "fat", "imgPath", "nutricionalValueId", "price", "protein", "title" },
                values: new object[,]
                {
                    { 1, 540, 50m, "Boiled chicken with white rice", 12m, null, 1, 3.20m, 35m, "Chicken with rice" },
                    { 2, 450, 70m, "Pasta with vegetables", 8m, null, 1, 2.80m, 15m, "Vegetarian pasta" }
                });

            migrationBuilder.InsertData(
                table: "Ingredients",
                columns: new[] { "ingredientId", "name" },
                values: new object[,]
                {
                    { 1, "Chicken breast" },
                    { 2, "Rice" },
                    { 3, "Pasta" },
                    { 4, "Mixed vegetables" }
                });

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "roleId", "roleTitle" },
                values: new object[,]
                {
                    { 1, "Employee" },
                    { 2, "Student" }
                });

            migrationBuilder.InsertData(
                table: "DailyMenus",
                columns: new[] { "dailyMenuId", "date", "dishId" },
                values: new object[,]
                {
                    { 1, new DateOnly(2025, 1, 1), 1 },
                    { 2, new DateOnly(2025, 11, 22), 2 }
                });

            migrationBuilder.InsertData(
                table: "DishIngredients",
                columns: new[] { "dishId", "ingredientId" },
                values: new object[,]
                {
                    { 1, 1 },
                    { 1, 2 },
                    { 2, 3 },
                    { 2, 4 }
                });

            migrationBuilder.InsertData(
                table: "DishRatings",
                columns: new[] { "dishRatingId", "dishId", "rating" },
                values: new object[,]
                {
                    { 1, 1, 5 },
                    { 2, 2, 4 }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "userId", "email", "firstName", "lastName", "passwordHash", "roleId" },
                values: new object[,]
                {
                    { 1, "employee@test.com", "Test", "Employee", "pass123", 1 },
                    { 2, "student@test.com", "Test", "Student", "pass123", 2 }
                });

            migrationBuilder.InsertData(
                table: "DailyFoodIntakes",
                columns: new[] { "dailyFoodIntakeId", "date", "dishId", "userId" },
                values: new object[,]
                {
                    { 1, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 2 },
                    { 2, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, 2 }
                });

            migrationBuilder.InsertData(
                table: "FavoriteDishes",
                columns: new[] { "dishId", "userId" },
                values: new object[,]
                {
                    { 1, 2 },
                    { 2, 2 }
                });

            migrationBuilder.InsertData(
                table: "NutricionGoals",
                columns: new[] { "nutricionalGoalId", "caloriesGoal", "carbohydratesGoal", "fatsGoal", "goalSetDate", "proteinsGoal", "userId" },
                values: new object[] { 1, 2000, 250m, 70m, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 120m, 2 });

            migrationBuilder.CreateIndex(
                name: "IX_DailyFoodIntakes_dishId",
                table: "DailyFoodIntakes",
                column: "dishId");

            migrationBuilder.CreateIndex(
                name: "IX_DailyFoodIntakes_userId",
                table: "DailyFoodIntakes",
                column: "userId");

            migrationBuilder.CreateIndex(
                name: "IX_DailyMenus_dishId",
                table: "DailyMenus",
                column: "dishId");

            migrationBuilder.CreateIndex(
                name: "IX_DishIngredients_ingredientId",
                table: "DishIngredients",
                column: "ingredientId");

            migrationBuilder.CreateIndex(
                name: "IX_DishRatings_dishId",
                table: "DishRatings",
                column: "dishId");

            migrationBuilder.CreateIndex(
                name: "IX_FavoriteDishes_dishId",
                table: "FavoriteDishes",
                column: "dishId");

            migrationBuilder.CreateIndex(
                name: "IX_NutricionGoals_userId",
                table: "NutricionGoals",
                column: "userId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_roleId",
                table: "Users",
                column: "roleId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DailyFoodIntakes");

            migrationBuilder.DropTable(
                name: "DailyMenus");

            migrationBuilder.DropTable(
                name: "DishIngredients");

            migrationBuilder.DropTable(
                name: "DishRatings");

            migrationBuilder.DropTable(
                name: "FavoriteDishes");

            migrationBuilder.DropTable(
                name: "NutricionGoals");

            migrationBuilder.DropTable(
                name: "Ingredients");

            migrationBuilder.DropTable(
                name: "Dishes");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Roles");
        }
    }
}
