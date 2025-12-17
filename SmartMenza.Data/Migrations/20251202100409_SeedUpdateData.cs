using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 

namespace SmartMenza.Data.Migrations
{
    
    public partial class SeedUpdateData : Migration
    {
        
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DailyMenus_Dishes_dishId",
                table: "DailyMenus");

            migrationBuilder.DropIndex(
                name: "IX_DailyMenus_dishId",
                table: "DailyMenus");

            migrationBuilder.DropColumn(
                name: "dishId",
                table: "DailyMenus");

            migrationBuilder.CreateTable(
                name: "DailyMenuDishes",
                columns: table => new
                {
                    dailyMenuId = table.Column<int>(type: "int", nullable: false),
                    dishId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DailyMenuDishes", x => new { x.dailyMenuId, x.dishId });
                    table.ForeignKey(
                        name: "FK_DailyMenuDishes_DailyMenus_dailyMenuId",
                        column: x => x.dailyMenuId,
                        principalTable: "DailyMenus",
                        principalColumn: "dailyMenuId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DailyMenuDishes_Dishes_dishId",
                        column: x => x.dishId,
                        principalTable: "Dishes",
                        principalColumn: "dishId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "DailyMenuDishes",
                columns: new[] { "dailyMenuId", "dishId" },
                values: new object[,]
                {
                    { 1, 1 },
                    { 1, 2 },
                    { 2, 1 },
                    { 2, 2 }
                });

            migrationBuilder.UpdateData(
                table: "DailyMenus",
                keyColumn: "dailyMenuId",
                keyValue: 2,
                column: "date",
                value: new DateOnly(2025, 11, 30));

            migrationBuilder.InsertData(
                table: "DailyMenus",
                columns: new[] { "dailyMenuId", "date" },
                values: new object[] { 3, new DateOnly(2025, 12, 1) });

            migrationBuilder.InsertData(
                table: "Dishes",
                columns: new[] { "dishId", "calories", "carbohydrates", "description", "fat", "imgPath", "nutricionalValueId", "price", "protein", "title" },
                values: new object[] { 3, 320, 5m, "Fresh grilled fish with lemon", 10m, null, 1, 4.50m, 40m, "Grilled fish" });

            migrationBuilder.InsertData(
                table: "DailyMenuDishes",
                columns: new[] { "dailyMenuId", "dishId" },
                values: new object[,]
                {
                    { 2, 3 },
                    { 3, 3 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_DailyMenuDishes_dishId",
                table: "DailyMenuDishes",
                column: "dishId");
        }

        
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DailyMenuDishes");

            migrationBuilder.DeleteData(
                table: "DailyMenus",
                keyColumn: "dailyMenuId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Dishes",
                keyColumn: "dishId",
                keyValue: 3);

            migrationBuilder.AddColumn<int>(
                name: "dishId",
                table: "DailyMenus",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "DailyMenus",
                keyColumn: "dailyMenuId",
                keyValue: 1,
                column: "dishId",
                value: 2);

            migrationBuilder.UpdateData(
                table: "DailyMenus",
                keyColumn: "dailyMenuId",
                keyValue: 2,
                columns: new[] { "date", "dishId" },
                values: new object[] { new DateOnly(2025, 11, 29), 2 });

            migrationBuilder.CreateIndex(
                name: "IX_DailyMenus_dishId",
                table: "DailyMenus",
                column: "dishId");

            migrationBuilder.AddForeignKey(
                name: "FK_DailyMenus_Dishes_dishId",
                table: "DailyMenus",
                column: "dishId",
                principalTable: "Dishes",
                principalColumn: "dishId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
