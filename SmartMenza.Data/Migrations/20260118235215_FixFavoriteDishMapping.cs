using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartMenza.Data.Migrations
{
    /// <inheritdoc />
    public partial class FixFavoriteDishMapping : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DailyFoodIntakes_Dishes_dishId",
                table: "DailyFoodIntakes");

            migrationBuilder.DropForeignKey(
                name: "FK_DailyFoodIntakes_Users_userId",
                table: "DailyFoodIntakes");

            migrationBuilder.DropForeignKey(
                name: "FK_DailyMenuDishes_DailyMenus_dailyMenuId",
                table: "DailyMenuDishes");

            migrationBuilder.DropForeignKey(
                name: "FK_DailyMenuDishes_Dishes_dishId",
                table: "DailyMenuDishes");

            migrationBuilder.DropForeignKey(
                name: "FK_DishIngredients_Dishes_dishId",
                table: "DishIngredients");

            migrationBuilder.DropForeignKey(
                name: "FK_DishIngredients_Ingredients_ingredientId",
                table: "DishIngredients");

            migrationBuilder.DropForeignKey(
                name: "FK_DishRatings_Dishes_dishId",
                table: "DishRatings");

            migrationBuilder.DropForeignKey(
                name: "FK_FavoriteDishes_Dishes_dishId",
                table: "FavoriteDishes");

            migrationBuilder.DropForeignKey(
                name: "FK_FavoriteDishes_Users_userId",
                table: "FavoriteDishes");

            migrationBuilder.DropForeignKey(
                name: "FK_NutricionGoals_Users_userId",
                table: "NutricionGoals");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_Roles_roleId",
                table: "Users");

            migrationBuilder.RenameColumn(
                name: "roleId",
                table: "Users",
                newName: "RoleId");

            migrationBuilder.RenameColumn(
                name: "passwordHash",
                table: "Users",
                newName: "PasswordHash");

            migrationBuilder.RenameColumn(
                name: "lastName",
                table: "Users",
                newName: "LastName");

            migrationBuilder.RenameColumn(
                name: "firstName",
                table: "Users",
                newName: "FirstName");

            migrationBuilder.RenameColumn(
                name: "email",
                table: "Users",
                newName: "Email");

            migrationBuilder.RenameColumn(
                name: "userId",
                table: "Users",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Users_roleId",
                table: "Users",
                newName: "IX_Users_RoleId");

            migrationBuilder.RenameColumn(
                name: "roleTitle",
                table: "Roles",
                newName: "RoleTitle");

            migrationBuilder.RenameColumn(
                name: "roleId",
                table: "Roles",
                newName: "RoleId");

            migrationBuilder.RenameColumn(
                name: "userId",
                table: "NutricionGoals",
                newName: "UserId");

            migrationBuilder.RenameColumn(
                name: "proteinsGoal",
                table: "NutricionGoals",
                newName: "ProteinsGoal");

            migrationBuilder.RenameColumn(
                name: "goalSetDate",
                table: "NutricionGoals",
                newName: "GoalSetDate");

            migrationBuilder.RenameColumn(
                name: "fatsGoal",
                table: "NutricionGoals",
                newName: "FatsGoal");

            migrationBuilder.RenameColumn(
                name: "carbohydratesGoal",
                table: "NutricionGoals",
                newName: "CarbohydratesGoal");

            migrationBuilder.RenameColumn(
                name: "caloriesGoal",
                table: "NutricionGoals",
                newName: "CaloriesGoal");

            migrationBuilder.RenameColumn(
                name: "nutricionalGoalId",
                table: "NutricionGoals",
                newName: "NutricionalGoalId");

            migrationBuilder.RenameIndex(
                name: "IX_NutricionGoals_userId",
                table: "NutricionGoals",
                newName: "IX_NutricionGoals_UserId");

            migrationBuilder.RenameColumn(
                name: "name",
                table: "Ingredients",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "ingredientId",
                table: "Ingredients",
                newName: "IngredientId");

            migrationBuilder.RenameColumn(
                name: "dishId",
                table: "FavoriteDishes",
                newName: "DishId");

            migrationBuilder.RenameColumn(
                name: "userId",
                table: "FavoriteDishes",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_FavoriteDishes_dishId",
                table: "FavoriteDishes",
                newName: "IX_FavoriteDishes_DishId");

            migrationBuilder.RenameColumn(
                name: "rating",
                table: "DishRatings",
                newName: "Rating");

            migrationBuilder.RenameColumn(
                name: "dishId",
                table: "DishRatings",
                newName: "DishId");

            migrationBuilder.RenameColumn(
                name: "dishRatingId",
                table: "DishRatings",
                newName: "DishRatingId");

            migrationBuilder.RenameIndex(
                name: "IX_DishRatings_dishId",
                table: "DishRatings",
                newName: "IX_DishRatings_DishId");

            migrationBuilder.RenameColumn(
                name: "ingredientId",
                table: "DishIngredients",
                newName: "IngredientId");

            migrationBuilder.RenameColumn(
                name: "dishId",
                table: "DishIngredients",
                newName: "DishId");

            migrationBuilder.RenameIndex(
                name: "IX_DishIngredients_ingredientId",
                table: "DishIngredients",
                newName: "IX_DishIngredients_IngredientId");

            migrationBuilder.RenameColumn(
                name: "title",
                table: "Dishes",
                newName: "Title");

            migrationBuilder.RenameColumn(
                name: "protein",
                table: "Dishes",
                newName: "Protein");

            migrationBuilder.RenameColumn(
                name: "price",
                table: "Dishes",
                newName: "Price");

            migrationBuilder.RenameColumn(
                name: "imgPath",
                table: "Dishes",
                newName: "ImgPath");

            migrationBuilder.RenameColumn(
                name: "fiber",
                table: "Dishes",
                newName: "Fiber");

            migrationBuilder.RenameColumn(
                name: "fat",
                table: "Dishes",
                newName: "Fat");

            migrationBuilder.RenameColumn(
                name: "description",
                table: "Dishes",
                newName: "Description");

            migrationBuilder.RenameColumn(
                name: "carbohydrates",
                table: "Dishes",
                newName: "Carbohydrates");

            migrationBuilder.RenameColumn(
                name: "calories",
                table: "Dishes",
                newName: "Calories");

            migrationBuilder.RenameColumn(
                name: "dishId",
                table: "Dishes",
                newName: "DishId");

            migrationBuilder.RenameColumn(
                name: "date",
                table: "DailyMenus",
                newName: "Date");

            migrationBuilder.RenameColumn(
                name: "category",
                table: "DailyMenus",
                newName: "Category");

            migrationBuilder.RenameColumn(
                name: "dailyMenuId",
                table: "DailyMenus",
                newName: "DailyMenuId");

            migrationBuilder.RenameColumn(
                name: "dishId",
                table: "DailyMenuDishes",
                newName: "DishId");

            migrationBuilder.RenameColumn(
                name: "dailyMenuId",
                table: "DailyMenuDishes",
                newName: "DailyMenuId");

            migrationBuilder.RenameIndex(
                name: "IX_DailyMenuDishes_dishId",
                table: "DailyMenuDishes",
                newName: "IX_DailyMenuDishes_DishId");

            migrationBuilder.RenameColumn(
                name: "userId",
                table: "DailyFoodIntakes",
                newName: "UserId");

            migrationBuilder.RenameColumn(
                name: "dishId",
                table: "DailyFoodIntakes",
                newName: "DishId");

            migrationBuilder.RenameColumn(
                name: "date",
                table: "DailyFoodIntakes",
                newName: "Date");

            migrationBuilder.RenameColumn(
                name: "dailyFoodIntakeId",
                table: "DailyFoodIntakes",
                newName: "DailyFoodIntakeId");

            migrationBuilder.RenameIndex(
                name: "IX_DailyFoodIntakes_userId",
                table: "DailyFoodIntakes",
                newName: "IX_DailyFoodIntakes_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_DailyFoodIntakes_dishId",
                table: "DailyFoodIntakes",
                newName: "IX_DailyFoodIntakes_DishId");

            migrationBuilder.AddForeignKey(
                name: "FK_DailyFoodIntakes_Dishes_DishId",
                table: "DailyFoodIntakes",
                column: "DishId",
                principalTable: "Dishes",
                principalColumn: "DishId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DailyFoodIntakes_Users_UserId",
                table: "DailyFoodIntakes",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DailyMenuDishes_DailyMenus_DailyMenuId",
                table: "DailyMenuDishes",
                column: "DailyMenuId",
                principalTable: "DailyMenus",
                principalColumn: "DailyMenuId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DailyMenuDishes_Dishes_DishId",
                table: "DailyMenuDishes",
                column: "DishId",
                principalTable: "Dishes",
                principalColumn: "DishId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DishIngredients_Dishes_DishId",
                table: "DishIngredients",
                column: "DishId",
                principalTable: "Dishes",
                principalColumn: "DishId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DishIngredients_Ingredients_IngredientId",
                table: "DishIngredients",
                column: "IngredientId",
                principalTable: "Ingredients",
                principalColumn: "IngredientId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DishRatings_Dishes_DishId",
                table: "DishRatings",
                column: "DishId",
                principalTable: "Dishes",
                principalColumn: "DishId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FavoriteDishes_Dishes_DishId",
                table: "FavoriteDishes",
                column: "DishId",
                principalTable: "Dishes",
                principalColumn: "DishId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FavoriteDishes_Users_UserId",
                table: "FavoriteDishes",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_NutricionGoals_Users_UserId",
                table: "NutricionGoals",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Roles_RoleId",
                table: "Users",
                column: "RoleId",
                principalTable: "Roles",
                principalColumn: "RoleId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DailyFoodIntakes_Dishes_DishId",
                table: "DailyFoodIntakes");

            migrationBuilder.DropForeignKey(
                name: "FK_DailyFoodIntakes_Users_UserId",
                table: "DailyFoodIntakes");

            migrationBuilder.DropForeignKey(
                name: "FK_DailyMenuDishes_DailyMenus_DailyMenuId",
                table: "DailyMenuDishes");

            migrationBuilder.DropForeignKey(
                name: "FK_DailyMenuDishes_Dishes_DishId",
                table: "DailyMenuDishes");

            migrationBuilder.DropForeignKey(
                name: "FK_DishIngredients_Dishes_DishId",
                table: "DishIngredients");

            migrationBuilder.DropForeignKey(
                name: "FK_DishIngredients_Ingredients_IngredientId",
                table: "DishIngredients");

            migrationBuilder.DropForeignKey(
                name: "FK_DishRatings_Dishes_DishId",
                table: "DishRatings");

            migrationBuilder.DropForeignKey(
                name: "FK_FavoriteDishes_Dishes_DishId",
                table: "FavoriteDishes");

            migrationBuilder.DropForeignKey(
                name: "FK_FavoriteDishes_Users_UserId",
                table: "FavoriteDishes");

            migrationBuilder.DropForeignKey(
                name: "FK_NutricionGoals_Users_UserId",
                table: "NutricionGoals");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_Roles_RoleId",
                table: "Users");

            migrationBuilder.RenameColumn(
                name: "RoleId",
                table: "Users",
                newName: "roleId");

            migrationBuilder.RenameColumn(
                name: "PasswordHash",
                table: "Users",
                newName: "passwordHash");

            migrationBuilder.RenameColumn(
                name: "LastName",
                table: "Users",
                newName: "lastName");

            migrationBuilder.RenameColumn(
                name: "FirstName",
                table: "Users",
                newName: "firstName");

            migrationBuilder.RenameColumn(
                name: "Email",
                table: "Users",
                newName: "email");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Users",
                newName: "userId");

            migrationBuilder.RenameIndex(
                name: "IX_Users_RoleId",
                table: "Users",
                newName: "IX_Users_roleId");

            migrationBuilder.RenameColumn(
                name: "RoleTitle",
                table: "Roles",
                newName: "roleTitle");

            migrationBuilder.RenameColumn(
                name: "RoleId",
                table: "Roles",
                newName: "roleId");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "NutricionGoals",
                newName: "userId");

            migrationBuilder.RenameColumn(
                name: "ProteinsGoal",
                table: "NutricionGoals",
                newName: "proteinsGoal");

            migrationBuilder.RenameColumn(
                name: "GoalSetDate",
                table: "NutricionGoals",
                newName: "goalSetDate");

            migrationBuilder.RenameColumn(
                name: "FatsGoal",
                table: "NutricionGoals",
                newName: "fatsGoal");

            migrationBuilder.RenameColumn(
                name: "CarbohydratesGoal",
                table: "NutricionGoals",
                newName: "carbohydratesGoal");

            migrationBuilder.RenameColumn(
                name: "CaloriesGoal",
                table: "NutricionGoals",
                newName: "caloriesGoal");

            migrationBuilder.RenameColumn(
                name: "NutricionalGoalId",
                table: "NutricionGoals",
                newName: "nutricionalGoalId");

            migrationBuilder.RenameIndex(
                name: "IX_NutricionGoals_UserId",
                table: "NutricionGoals",
                newName: "IX_NutricionGoals_userId");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Ingredients",
                newName: "name");

            migrationBuilder.RenameColumn(
                name: "IngredientId",
                table: "Ingredients",
                newName: "ingredientId");

            migrationBuilder.RenameColumn(
                name: "DishId",
                table: "FavoriteDishes",
                newName: "dishId");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "FavoriteDishes",
                newName: "userId");

            migrationBuilder.RenameIndex(
                name: "IX_FavoriteDishes_DishId",
                table: "FavoriteDishes",
                newName: "IX_FavoriteDishes_dishId");

            migrationBuilder.RenameColumn(
                name: "Rating",
                table: "DishRatings",
                newName: "rating");

            migrationBuilder.RenameColumn(
                name: "DishId",
                table: "DishRatings",
                newName: "dishId");

            migrationBuilder.RenameColumn(
                name: "DishRatingId",
                table: "DishRatings",
                newName: "dishRatingId");

            migrationBuilder.RenameIndex(
                name: "IX_DishRatings_DishId",
                table: "DishRatings",
                newName: "IX_DishRatings_dishId");

            migrationBuilder.RenameColumn(
                name: "IngredientId",
                table: "DishIngredients",
                newName: "ingredientId");

            migrationBuilder.RenameColumn(
                name: "DishId",
                table: "DishIngredients",
                newName: "dishId");

            migrationBuilder.RenameIndex(
                name: "IX_DishIngredients_IngredientId",
                table: "DishIngredients",
                newName: "IX_DishIngredients_ingredientId");

            migrationBuilder.RenameColumn(
                name: "Title",
                table: "Dishes",
                newName: "title");

            migrationBuilder.RenameColumn(
                name: "Protein",
                table: "Dishes",
                newName: "protein");

            migrationBuilder.RenameColumn(
                name: "Price",
                table: "Dishes",
                newName: "price");

            migrationBuilder.RenameColumn(
                name: "ImgPath",
                table: "Dishes",
                newName: "imgPath");

            migrationBuilder.RenameColumn(
                name: "Fiber",
                table: "Dishes",
                newName: "fiber");

            migrationBuilder.RenameColumn(
                name: "Fat",
                table: "Dishes",
                newName: "fat");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "Dishes",
                newName: "description");

            migrationBuilder.RenameColumn(
                name: "Carbohydrates",
                table: "Dishes",
                newName: "carbohydrates");

            migrationBuilder.RenameColumn(
                name: "Calories",
                table: "Dishes",
                newName: "calories");

            migrationBuilder.RenameColumn(
                name: "DishId",
                table: "Dishes",
                newName: "dishId");

            migrationBuilder.RenameColumn(
                name: "Date",
                table: "DailyMenus",
                newName: "date");

            migrationBuilder.RenameColumn(
                name: "Category",
                table: "DailyMenus",
                newName: "category");

            migrationBuilder.RenameColumn(
                name: "DailyMenuId",
                table: "DailyMenus",
                newName: "dailyMenuId");

            migrationBuilder.RenameColumn(
                name: "DishId",
                table: "DailyMenuDishes",
                newName: "dishId");

            migrationBuilder.RenameColumn(
                name: "DailyMenuId",
                table: "DailyMenuDishes",
                newName: "dailyMenuId");

            migrationBuilder.RenameIndex(
                name: "IX_DailyMenuDishes_DishId",
                table: "DailyMenuDishes",
                newName: "IX_DailyMenuDishes_dishId");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "DailyFoodIntakes",
                newName: "userId");

            migrationBuilder.RenameColumn(
                name: "DishId",
                table: "DailyFoodIntakes",
                newName: "dishId");

            migrationBuilder.RenameColumn(
                name: "Date",
                table: "DailyFoodIntakes",
                newName: "date");

            migrationBuilder.RenameColumn(
                name: "DailyFoodIntakeId",
                table: "DailyFoodIntakes",
                newName: "dailyFoodIntakeId");

            migrationBuilder.RenameIndex(
                name: "IX_DailyFoodIntakes_UserId",
                table: "DailyFoodIntakes",
                newName: "IX_DailyFoodIntakes_userId");

            migrationBuilder.RenameIndex(
                name: "IX_DailyFoodIntakes_DishId",
                table: "DailyFoodIntakes",
                newName: "IX_DailyFoodIntakes_dishId");

            migrationBuilder.AddForeignKey(
                name: "FK_DailyFoodIntakes_Dishes_dishId",
                table: "DailyFoodIntakes",
                column: "dishId",
                principalTable: "Dishes",
                principalColumn: "dishId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DailyFoodIntakes_Users_userId",
                table: "DailyFoodIntakes",
                column: "userId",
                principalTable: "Users",
                principalColumn: "userId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DailyMenuDishes_DailyMenus_dailyMenuId",
                table: "DailyMenuDishes",
                column: "dailyMenuId",
                principalTable: "DailyMenus",
                principalColumn: "dailyMenuId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DailyMenuDishes_Dishes_dishId",
                table: "DailyMenuDishes",
                column: "dishId",
                principalTable: "Dishes",
                principalColumn: "dishId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DishIngredients_Dishes_dishId",
                table: "DishIngredients",
                column: "dishId",
                principalTable: "Dishes",
                principalColumn: "dishId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DishIngredients_Ingredients_ingredientId",
                table: "DishIngredients",
                column: "ingredientId",
                principalTable: "Ingredients",
                principalColumn: "ingredientId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DishRatings_Dishes_dishId",
                table: "DishRatings",
                column: "dishId",
                principalTable: "Dishes",
                principalColumn: "dishId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FavoriteDishes_Dishes_dishId",
                table: "FavoriteDishes",
                column: "dishId",
                principalTable: "Dishes",
                principalColumn: "dishId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FavoriteDishes_Users_userId",
                table: "FavoriteDishes",
                column: "userId",
                principalTable: "Users",
                principalColumn: "userId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_NutricionGoals_Users_userId",
                table: "NutricionGoals",
                column: "userId",
                principalTable: "Users",
                principalColumn: "userId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Roles_roleId",
                table: "Users",
                column: "roleId",
                principalTable: "Roles",
                principalColumn: "roleId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
