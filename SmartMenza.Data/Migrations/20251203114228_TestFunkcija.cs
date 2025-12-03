using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace SmartMenza.Data.Migrations
{
    /// <inheritdoc />
    public partial class TestFunkcija : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "DailyMenuDishes",
                keyColumns: new[] { "dailyMenuId", "dishId" },
                keyValues: new object[] { 2, 1 });

            migrationBuilder.DeleteData(
                table: "DailyMenuDishes",
                keyColumns: new[] { "dailyMenuId", "dishId" },
                keyValues: new object[] { 3, 3 });

            migrationBuilder.AddColumn<int>(
                name: "category",
                table: "DailyMenus",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.InsertData(
                table: "DailyMenuDishes",
                columns: new[] { "dailyMenuId", "dishId" },
                values: new object[,]
                {
                    { 1, 3 },
                    { 3, 1 },
                    { 3, 2 }
                });

            migrationBuilder.UpdateData(
                table: "DailyMenus",
                keyColumn: "dailyMenuId",
                keyValue: 1,
                columns: new[] { "category", "date" },
                values: new object[] { 1, new DateOnly(2024, 12, 5) });

            migrationBuilder.UpdateData(
                table: "DailyMenus",
                keyColumn: "dailyMenuId",
                keyValue: 2,
                columns: new[] { "category", "date" },
                values: new object[] { 2, new DateOnly(2024, 12, 5) });

            migrationBuilder.UpdateData(
                table: "DailyMenus",
                keyColumn: "dailyMenuId",
                keyValue: 3,
                columns: new[] { "category", "date" },
                values: new object[] { 1, new DateOnly(2024, 12, 6) });

            migrationBuilder.InsertData(
                table: "DailyMenus",
                columns: new[] { "dailyMenuId", "category", "date" },
                values: new object[,]
                {
                    { 4, 2, new DateOnly(2024, 12, 6) },
                    { 5, 1, new DateOnly(2024, 12, 7) },
                    { 6, 2, new DateOnly(2024, 12, 7) },
                    { 7, 1, new DateOnly(2024, 12, 8) },
                    { 8, 2, new DateOnly(2024, 12, 8) },
                    { 9, 1, new DateOnly(2024, 12, 9) },
                    { 10, 2, new DateOnly(2024, 12, 9) },
                    { 11, 1, new DateOnly(2024, 12, 10) },
                    { 12, 2, new DateOnly(2024, 12, 10) },
                    { 13, 1, new DateOnly(2024, 12, 11) },
                    { 14, 2, new DateOnly(2024, 12, 11) },
                    { 15, 1, new DateOnly(2024, 12, 12) },
                    { 16, 2, new DateOnly(2024, 12, 12) },
                    { 17, 1, new DateOnly(2024, 12, 13) },
                    { 18, 2, new DateOnly(2024, 12, 13) },
                    { 19, 1, new DateOnly(2024, 12, 14) },
                    { 20, 2, new DateOnly(2024, 12, 14) },
                    { 21, 1, new DateOnly(2024, 12, 15) },
                    { 22, 2, new DateOnly(2024, 12, 15) },
                    { 23, 1, new DateOnly(2024, 12, 16) },
                    { 24, 2, new DateOnly(2024, 12, 16) },
                    { 25, 1, new DateOnly(2024, 12, 17) },
                    { 26, 2, new DateOnly(2024, 12, 17) },
                    { 27, 1, new DateOnly(2024, 12, 18) },
                    { 28, 2, new DateOnly(2024, 12, 18) },
                    { 29, 1, new DateOnly(2024, 12, 19) },
                    { 30, 2, new DateOnly(2024, 12, 19) },
                    { 31, 1, new DateOnly(2024, 12, 20) },
                    { 32, 2, new DateOnly(2024, 12, 20) },
                    { 33, 1, new DateOnly(2024, 12, 21) },
                    { 34, 2, new DateOnly(2024, 12, 21) },
                    { 35, 1, new DateOnly(2024, 12, 22) },
                    { 36, 2, new DateOnly(2024, 12, 22) },
                    { 37, 1, new DateOnly(2024, 12, 23) },
                    { 38, 2, new DateOnly(2024, 12, 23) },
                    { 39, 1, new DateOnly(2024, 12, 24) },
                    { 40, 2, new DateOnly(2024, 12, 24) },
                    { 41, 1, new DateOnly(2024, 12, 25) },
                    { 42, 2, new DateOnly(2024, 12, 25) },
                    { 43, 1, new DateOnly(2024, 12, 26) },
                    { 44, 2, new DateOnly(2024, 12, 26) },
                    { 45, 1, new DateOnly(2024, 12, 27) },
                    { 46, 2, new DateOnly(2024, 12, 27) }
                });

            migrationBuilder.InsertData(
                table: "DailyMenuDishes",
                columns: new[] { "dailyMenuId", "dishId" },
                values: new object[,]
                {
                    { 4, 1 },
                    { 4, 2 },
                    { 4, 3 },
                    { 5, 2 },
                    { 5, 3 },
                    { 6, 1 },
                    { 6, 3 },
                    { 7, 1 },
                    { 7, 3 },
                    { 8, 1 },
                    { 8, 2 },
                    { 8, 3 },
                    { 9, 1 },
                    { 9, 2 },
                    { 9, 3 },
                    { 10, 1 },
                    { 10, 2 },
                    { 11, 1 },
                    { 11, 2 },
                    { 11, 3 },
                    { 12, 2 },
                    { 12, 3 },
                    { 13, 1 },
                    { 13, 2 },
                    { 14, 1 },
                    { 14, 2 },
                    { 14, 3 },
                    { 15, 2 },
                    { 15, 3 },
                    { 16, 1 },
                    { 16, 3 },
                    { 17, 1 },
                    { 17, 3 },
                    { 18, 1 },
                    { 18, 2 },
                    { 18, 3 },
                    { 19, 1 },
                    { 19, 2 },
                    { 19, 3 },
                    { 20, 1 },
                    { 20, 2 },
                    { 21, 1 },
                    { 21, 2 },
                    { 21, 3 },
                    { 22, 2 },
                    { 22, 3 },
                    { 23, 1 },
                    { 23, 2 },
                    { 24, 1 },
                    { 24, 2 },
                    { 24, 3 },
                    { 25, 2 },
                    { 25, 3 },
                    { 26, 1 },
                    { 26, 3 },
                    { 27, 1 },
                    { 27, 3 },
                    { 28, 1 },
                    { 28, 2 },
                    { 28, 3 },
                    { 29, 1 },
                    { 29, 2 },
                    { 29, 3 },
                    { 30, 1 },
                    { 30, 2 },
                    { 31, 1 },
                    { 31, 2 },
                    { 31, 3 },
                    { 32, 2 },
                    { 32, 3 },
                    { 33, 1 },
                    { 33, 2 },
                    { 34, 1 },
                    { 34, 2 },
                    { 34, 3 },
                    { 35, 2 },
                    { 35, 3 },
                    { 36, 1 },
                    { 36, 3 },
                    { 37, 1 },
                    { 37, 3 },
                    { 38, 1 },
                    { 38, 2 },
                    { 38, 3 },
                    { 39, 1 },
                    { 39, 2 },
                    { 39, 3 },
                    { 40, 1 },
                    { 40, 2 },
                    { 41, 1 },
                    { 41, 2 },
                    { 41, 3 },
                    { 42, 2 },
                    { 42, 3 },
                    { 43, 1 },
                    { 43, 2 },
                    { 44, 1 },
                    { 44, 2 },
                    { 44, 3 },
                    { 45, 2 },
                    { 45, 3 },
                    { 46, 1 },
                    { 46, 3 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "DailyMenuDishes",
                keyColumns: new[] { "dailyMenuId", "dishId" },
                keyValues: new object[] { 1, 3 });

            migrationBuilder.DeleteData(
                table: "DailyMenuDishes",
                keyColumns: new[] { "dailyMenuId", "dishId" },
                keyValues: new object[] { 3, 1 });

            migrationBuilder.DeleteData(
                table: "DailyMenuDishes",
                keyColumns: new[] { "dailyMenuId", "dishId" },
                keyValues: new object[] { 3, 2 });

            migrationBuilder.DeleteData(
                table: "DailyMenuDishes",
                keyColumns: new[] { "dailyMenuId", "dishId" },
                keyValues: new object[] { 4, 1 });

            migrationBuilder.DeleteData(
                table: "DailyMenuDishes",
                keyColumns: new[] { "dailyMenuId", "dishId" },
                keyValues: new object[] { 4, 2 });

            migrationBuilder.DeleteData(
                table: "DailyMenuDishes",
                keyColumns: new[] { "dailyMenuId", "dishId" },
                keyValues: new object[] { 4, 3 });

            migrationBuilder.DeleteData(
                table: "DailyMenuDishes",
                keyColumns: new[] { "dailyMenuId", "dishId" },
                keyValues: new object[] { 5, 2 });

            migrationBuilder.DeleteData(
                table: "DailyMenuDishes",
                keyColumns: new[] { "dailyMenuId", "dishId" },
                keyValues: new object[] { 5, 3 });

            migrationBuilder.DeleteData(
                table: "DailyMenuDishes",
                keyColumns: new[] { "dailyMenuId", "dishId" },
                keyValues: new object[] { 6, 1 });

            migrationBuilder.DeleteData(
                table: "DailyMenuDishes",
                keyColumns: new[] { "dailyMenuId", "dishId" },
                keyValues: new object[] { 6, 3 });

            migrationBuilder.DeleteData(
                table: "DailyMenuDishes",
                keyColumns: new[] { "dailyMenuId", "dishId" },
                keyValues: new object[] { 7, 1 });

            migrationBuilder.DeleteData(
                table: "DailyMenuDishes",
                keyColumns: new[] { "dailyMenuId", "dishId" },
                keyValues: new object[] { 7, 3 });

            migrationBuilder.DeleteData(
                table: "DailyMenuDishes",
                keyColumns: new[] { "dailyMenuId", "dishId" },
                keyValues: new object[] { 8, 1 });

            migrationBuilder.DeleteData(
                table: "DailyMenuDishes",
                keyColumns: new[] { "dailyMenuId", "dishId" },
                keyValues: new object[] { 8, 2 });

            migrationBuilder.DeleteData(
                table: "DailyMenuDishes",
                keyColumns: new[] { "dailyMenuId", "dishId" },
                keyValues: new object[] { 8, 3 });

            migrationBuilder.DeleteData(
                table: "DailyMenuDishes",
                keyColumns: new[] { "dailyMenuId", "dishId" },
                keyValues: new object[] { 9, 1 });

            migrationBuilder.DeleteData(
                table: "DailyMenuDishes",
                keyColumns: new[] { "dailyMenuId", "dishId" },
                keyValues: new object[] { 9, 2 });

            migrationBuilder.DeleteData(
                table: "DailyMenuDishes",
                keyColumns: new[] { "dailyMenuId", "dishId" },
                keyValues: new object[] { 9, 3 });

            migrationBuilder.DeleteData(
                table: "DailyMenuDishes",
                keyColumns: new[] { "dailyMenuId", "dishId" },
                keyValues: new object[] { 10, 1 });

            migrationBuilder.DeleteData(
                table: "DailyMenuDishes",
                keyColumns: new[] { "dailyMenuId", "dishId" },
                keyValues: new object[] { 10, 2 });

            migrationBuilder.DeleteData(
                table: "DailyMenuDishes",
                keyColumns: new[] { "dailyMenuId", "dishId" },
                keyValues: new object[] { 11, 1 });

            migrationBuilder.DeleteData(
                table: "DailyMenuDishes",
                keyColumns: new[] { "dailyMenuId", "dishId" },
                keyValues: new object[] { 11, 2 });

            migrationBuilder.DeleteData(
                table: "DailyMenuDishes",
                keyColumns: new[] { "dailyMenuId", "dishId" },
                keyValues: new object[] { 11, 3 });

            migrationBuilder.DeleteData(
                table: "DailyMenuDishes",
                keyColumns: new[] { "dailyMenuId", "dishId" },
                keyValues: new object[] { 12, 2 });

            migrationBuilder.DeleteData(
                table: "DailyMenuDishes",
                keyColumns: new[] { "dailyMenuId", "dishId" },
                keyValues: new object[] { 12, 3 });

            migrationBuilder.DeleteData(
                table: "DailyMenuDishes",
                keyColumns: new[] { "dailyMenuId", "dishId" },
                keyValues: new object[] { 13, 1 });

            migrationBuilder.DeleteData(
                table: "DailyMenuDishes",
                keyColumns: new[] { "dailyMenuId", "dishId" },
                keyValues: new object[] { 13, 2 });

            migrationBuilder.DeleteData(
                table: "DailyMenuDishes",
                keyColumns: new[] { "dailyMenuId", "dishId" },
                keyValues: new object[] { 14, 1 });

            migrationBuilder.DeleteData(
                table: "DailyMenuDishes",
                keyColumns: new[] { "dailyMenuId", "dishId" },
                keyValues: new object[] { 14, 2 });

            migrationBuilder.DeleteData(
                table: "DailyMenuDishes",
                keyColumns: new[] { "dailyMenuId", "dishId" },
                keyValues: new object[] { 14, 3 });

            migrationBuilder.DeleteData(
                table: "DailyMenuDishes",
                keyColumns: new[] { "dailyMenuId", "dishId" },
                keyValues: new object[] { 15, 2 });

            migrationBuilder.DeleteData(
                table: "DailyMenuDishes",
                keyColumns: new[] { "dailyMenuId", "dishId" },
                keyValues: new object[] { 15, 3 });

            migrationBuilder.DeleteData(
                table: "DailyMenuDishes",
                keyColumns: new[] { "dailyMenuId", "dishId" },
                keyValues: new object[] { 16, 1 });

            migrationBuilder.DeleteData(
                table: "DailyMenuDishes",
                keyColumns: new[] { "dailyMenuId", "dishId" },
                keyValues: new object[] { 16, 3 });

            migrationBuilder.DeleteData(
                table: "DailyMenuDishes",
                keyColumns: new[] { "dailyMenuId", "dishId" },
                keyValues: new object[] { 17, 1 });

            migrationBuilder.DeleteData(
                table: "DailyMenuDishes",
                keyColumns: new[] { "dailyMenuId", "dishId" },
                keyValues: new object[] { 17, 3 });

            migrationBuilder.DeleteData(
                table: "DailyMenuDishes",
                keyColumns: new[] { "dailyMenuId", "dishId" },
                keyValues: new object[] { 18, 1 });

            migrationBuilder.DeleteData(
                table: "DailyMenuDishes",
                keyColumns: new[] { "dailyMenuId", "dishId" },
                keyValues: new object[] { 18, 2 });

            migrationBuilder.DeleteData(
                table: "DailyMenuDishes",
                keyColumns: new[] { "dailyMenuId", "dishId" },
                keyValues: new object[] { 18, 3 });

            migrationBuilder.DeleteData(
                table: "DailyMenuDishes",
                keyColumns: new[] { "dailyMenuId", "dishId" },
                keyValues: new object[] { 19, 1 });

            migrationBuilder.DeleteData(
                table: "DailyMenuDishes",
                keyColumns: new[] { "dailyMenuId", "dishId" },
                keyValues: new object[] { 19, 2 });

            migrationBuilder.DeleteData(
                table: "DailyMenuDishes",
                keyColumns: new[] { "dailyMenuId", "dishId" },
                keyValues: new object[] { 19, 3 });

            migrationBuilder.DeleteData(
                table: "DailyMenuDishes",
                keyColumns: new[] { "dailyMenuId", "dishId" },
                keyValues: new object[] { 20, 1 });

            migrationBuilder.DeleteData(
                table: "DailyMenuDishes",
                keyColumns: new[] { "dailyMenuId", "dishId" },
                keyValues: new object[] { 20, 2 });

            migrationBuilder.DeleteData(
                table: "DailyMenuDishes",
                keyColumns: new[] { "dailyMenuId", "dishId" },
                keyValues: new object[] { 21, 1 });

            migrationBuilder.DeleteData(
                table: "DailyMenuDishes",
                keyColumns: new[] { "dailyMenuId", "dishId" },
                keyValues: new object[] { 21, 2 });

            migrationBuilder.DeleteData(
                table: "DailyMenuDishes",
                keyColumns: new[] { "dailyMenuId", "dishId" },
                keyValues: new object[] { 21, 3 });

            migrationBuilder.DeleteData(
                table: "DailyMenuDishes",
                keyColumns: new[] { "dailyMenuId", "dishId" },
                keyValues: new object[] { 22, 2 });

            migrationBuilder.DeleteData(
                table: "DailyMenuDishes",
                keyColumns: new[] { "dailyMenuId", "dishId" },
                keyValues: new object[] { 22, 3 });

            migrationBuilder.DeleteData(
                table: "DailyMenuDishes",
                keyColumns: new[] { "dailyMenuId", "dishId" },
                keyValues: new object[] { 23, 1 });

            migrationBuilder.DeleteData(
                table: "DailyMenuDishes",
                keyColumns: new[] { "dailyMenuId", "dishId" },
                keyValues: new object[] { 23, 2 });

            migrationBuilder.DeleteData(
                table: "DailyMenuDishes",
                keyColumns: new[] { "dailyMenuId", "dishId" },
                keyValues: new object[] { 24, 1 });

            migrationBuilder.DeleteData(
                table: "DailyMenuDishes",
                keyColumns: new[] { "dailyMenuId", "dishId" },
                keyValues: new object[] { 24, 2 });

            migrationBuilder.DeleteData(
                table: "DailyMenuDishes",
                keyColumns: new[] { "dailyMenuId", "dishId" },
                keyValues: new object[] { 24, 3 });

            migrationBuilder.DeleteData(
                table: "DailyMenuDishes",
                keyColumns: new[] { "dailyMenuId", "dishId" },
                keyValues: new object[] { 25, 2 });

            migrationBuilder.DeleteData(
                table: "DailyMenuDishes",
                keyColumns: new[] { "dailyMenuId", "dishId" },
                keyValues: new object[] { 25, 3 });

            migrationBuilder.DeleteData(
                table: "DailyMenuDishes",
                keyColumns: new[] { "dailyMenuId", "dishId" },
                keyValues: new object[] { 26, 1 });

            migrationBuilder.DeleteData(
                table: "DailyMenuDishes",
                keyColumns: new[] { "dailyMenuId", "dishId" },
                keyValues: new object[] { 26, 3 });

            migrationBuilder.DeleteData(
                table: "DailyMenuDishes",
                keyColumns: new[] { "dailyMenuId", "dishId" },
                keyValues: new object[] { 27, 1 });

            migrationBuilder.DeleteData(
                table: "DailyMenuDishes",
                keyColumns: new[] { "dailyMenuId", "dishId" },
                keyValues: new object[] { 27, 3 });

            migrationBuilder.DeleteData(
                table: "DailyMenuDishes",
                keyColumns: new[] { "dailyMenuId", "dishId" },
                keyValues: new object[] { 28, 1 });

            migrationBuilder.DeleteData(
                table: "DailyMenuDishes",
                keyColumns: new[] { "dailyMenuId", "dishId" },
                keyValues: new object[] { 28, 2 });

            migrationBuilder.DeleteData(
                table: "DailyMenuDishes",
                keyColumns: new[] { "dailyMenuId", "dishId" },
                keyValues: new object[] { 28, 3 });

            migrationBuilder.DeleteData(
                table: "DailyMenuDishes",
                keyColumns: new[] { "dailyMenuId", "dishId" },
                keyValues: new object[] { 29, 1 });

            migrationBuilder.DeleteData(
                table: "DailyMenuDishes",
                keyColumns: new[] { "dailyMenuId", "dishId" },
                keyValues: new object[] { 29, 2 });

            migrationBuilder.DeleteData(
                table: "DailyMenuDishes",
                keyColumns: new[] { "dailyMenuId", "dishId" },
                keyValues: new object[] { 29, 3 });

            migrationBuilder.DeleteData(
                table: "DailyMenuDishes",
                keyColumns: new[] { "dailyMenuId", "dishId" },
                keyValues: new object[] { 30, 1 });

            migrationBuilder.DeleteData(
                table: "DailyMenuDishes",
                keyColumns: new[] { "dailyMenuId", "dishId" },
                keyValues: new object[] { 30, 2 });

            migrationBuilder.DeleteData(
                table: "DailyMenuDishes",
                keyColumns: new[] { "dailyMenuId", "dishId" },
                keyValues: new object[] { 31, 1 });

            migrationBuilder.DeleteData(
                table: "DailyMenuDishes",
                keyColumns: new[] { "dailyMenuId", "dishId" },
                keyValues: new object[] { 31, 2 });

            migrationBuilder.DeleteData(
                table: "DailyMenuDishes",
                keyColumns: new[] { "dailyMenuId", "dishId" },
                keyValues: new object[] { 31, 3 });

            migrationBuilder.DeleteData(
                table: "DailyMenuDishes",
                keyColumns: new[] { "dailyMenuId", "dishId" },
                keyValues: new object[] { 32, 2 });

            migrationBuilder.DeleteData(
                table: "DailyMenuDishes",
                keyColumns: new[] { "dailyMenuId", "dishId" },
                keyValues: new object[] { 32, 3 });

            migrationBuilder.DeleteData(
                table: "DailyMenuDishes",
                keyColumns: new[] { "dailyMenuId", "dishId" },
                keyValues: new object[] { 33, 1 });

            migrationBuilder.DeleteData(
                table: "DailyMenuDishes",
                keyColumns: new[] { "dailyMenuId", "dishId" },
                keyValues: new object[] { 33, 2 });

            migrationBuilder.DeleteData(
                table: "DailyMenuDishes",
                keyColumns: new[] { "dailyMenuId", "dishId" },
                keyValues: new object[] { 34, 1 });

            migrationBuilder.DeleteData(
                table: "DailyMenuDishes",
                keyColumns: new[] { "dailyMenuId", "dishId" },
                keyValues: new object[] { 34, 2 });

            migrationBuilder.DeleteData(
                table: "DailyMenuDishes",
                keyColumns: new[] { "dailyMenuId", "dishId" },
                keyValues: new object[] { 34, 3 });

            migrationBuilder.DeleteData(
                table: "DailyMenuDishes",
                keyColumns: new[] { "dailyMenuId", "dishId" },
                keyValues: new object[] { 35, 2 });

            migrationBuilder.DeleteData(
                table: "DailyMenuDishes",
                keyColumns: new[] { "dailyMenuId", "dishId" },
                keyValues: new object[] { 35, 3 });

            migrationBuilder.DeleteData(
                table: "DailyMenuDishes",
                keyColumns: new[] { "dailyMenuId", "dishId" },
                keyValues: new object[] { 36, 1 });

            migrationBuilder.DeleteData(
                table: "DailyMenuDishes",
                keyColumns: new[] { "dailyMenuId", "dishId" },
                keyValues: new object[] { 36, 3 });

            migrationBuilder.DeleteData(
                table: "DailyMenuDishes",
                keyColumns: new[] { "dailyMenuId", "dishId" },
                keyValues: new object[] { 37, 1 });

            migrationBuilder.DeleteData(
                table: "DailyMenuDishes",
                keyColumns: new[] { "dailyMenuId", "dishId" },
                keyValues: new object[] { 37, 3 });

            migrationBuilder.DeleteData(
                table: "DailyMenuDishes",
                keyColumns: new[] { "dailyMenuId", "dishId" },
                keyValues: new object[] { 38, 1 });

            migrationBuilder.DeleteData(
                table: "DailyMenuDishes",
                keyColumns: new[] { "dailyMenuId", "dishId" },
                keyValues: new object[] { 38, 2 });

            migrationBuilder.DeleteData(
                table: "DailyMenuDishes",
                keyColumns: new[] { "dailyMenuId", "dishId" },
                keyValues: new object[] { 38, 3 });

            migrationBuilder.DeleteData(
                table: "DailyMenuDishes",
                keyColumns: new[] { "dailyMenuId", "dishId" },
                keyValues: new object[] { 39, 1 });

            migrationBuilder.DeleteData(
                table: "DailyMenuDishes",
                keyColumns: new[] { "dailyMenuId", "dishId" },
                keyValues: new object[] { 39, 2 });

            migrationBuilder.DeleteData(
                table: "DailyMenuDishes",
                keyColumns: new[] { "dailyMenuId", "dishId" },
                keyValues: new object[] { 39, 3 });

            migrationBuilder.DeleteData(
                table: "DailyMenuDishes",
                keyColumns: new[] { "dailyMenuId", "dishId" },
                keyValues: new object[] { 40, 1 });

            migrationBuilder.DeleteData(
                table: "DailyMenuDishes",
                keyColumns: new[] { "dailyMenuId", "dishId" },
                keyValues: new object[] { 40, 2 });

            migrationBuilder.DeleteData(
                table: "DailyMenuDishes",
                keyColumns: new[] { "dailyMenuId", "dishId" },
                keyValues: new object[] { 41, 1 });

            migrationBuilder.DeleteData(
                table: "DailyMenuDishes",
                keyColumns: new[] { "dailyMenuId", "dishId" },
                keyValues: new object[] { 41, 2 });

            migrationBuilder.DeleteData(
                table: "DailyMenuDishes",
                keyColumns: new[] { "dailyMenuId", "dishId" },
                keyValues: new object[] { 41, 3 });

            migrationBuilder.DeleteData(
                table: "DailyMenuDishes",
                keyColumns: new[] { "dailyMenuId", "dishId" },
                keyValues: new object[] { 42, 2 });

            migrationBuilder.DeleteData(
                table: "DailyMenuDishes",
                keyColumns: new[] { "dailyMenuId", "dishId" },
                keyValues: new object[] { 42, 3 });

            migrationBuilder.DeleteData(
                table: "DailyMenuDishes",
                keyColumns: new[] { "dailyMenuId", "dishId" },
                keyValues: new object[] { 43, 1 });

            migrationBuilder.DeleteData(
                table: "DailyMenuDishes",
                keyColumns: new[] { "dailyMenuId", "dishId" },
                keyValues: new object[] { 43, 2 });

            migrationBuilder.DeleteData(
                table: "DailyMenuDishes",
                keyColumns: new[] { "dailyMenuId", "dishId" },
                keyValues: new object[] { 44, 1 });

            migrationBuilder.DeleteData(
                table: "DailyMenuDishes",
                keyColumns: new[] { "dailyMenuId", "dishId" },
                keyValues: new object[] { 44, 2 });

            migrationBuilder.DeleteData(
                table: "DailyMenuDishes",
                keyColumns: new[] { "dailyMenuId", "dishId" },
                keyValues: new object[] { 44, 3 });

            migrationBuilder.DeleteData(
                table: "DailyMenuDishes",
                keyColumns: new[] { "dailyMenuId", "dishId" },
                keyValues: new object[] { 45, 2 });

            migrationBuilder.DeleteData(
                table: "DailyMenuDishes",
                keyColumns: new[] { "dailyMenuId", "dishId" },
                keyValues: new object[] { 45, 3 });

            migrationBuilder.DeleteData(
                table: "DailyMenuDishes",
                keyColumns: new[] { "dailyMenuId", "dishId" },
                keyValues: new object[] { 46, 1 });

            migrationBuilder.DeleteData(
                table: "DailyMenuDishes",
                keyColumns: new[] { "dailyMenuId", "dishId" },
                keyValues: new object[] { 46, 3 });

            migrationBuilder.DeleteData(
                table: "DailyMenus",
                keyColumn: "dailyMenuId",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "DailyMenus",
                keyColumn: "dailyMenuId",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "DailyMenus",
                keyColumn: "dailyMenuId",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "DailyMenus",
                keyColumn: "dailyMenuId",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "DailyMenus",
                keyColumn: "dailyMenuId",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "DailyMenus",
                keyColumn: "dailyMenuId",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "DailyMenus",
                keyColumn: "dailyMenuId",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "DailyMenus",
                keyColumn: "dailyMenuId",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "DailyMenus",
                keyColumn: "dailyMenuId",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "DailyMenus",
                keyColumn: "dailyMenuId",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "DailyMenus",
                keyColumn: "dailyMenuId",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "DailyMenus",
                keyColumn: "dailyMenuId",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "DailyMenus",
                keyColumn: "dailyMenuId",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "DailyMenus",
                keyColumn: "dailyMenuId",
                keyValue: 17);

            migrationBuilder.DeleteData(
                table: "DailyMenus",
                keyColumn: "dailyMenuId",
                keyValue: 18);

            migrationBuilder.DeleteData(
                table: "DailyMenus",
                keyColumn: "dailyMenuId",
                keyValue: 19);

            migrationBuilder.DeleteData(
                table: "DailyMenus",
                keyColumn: "dailyMenuId",
                keyValue: 20);

            migrationBuilder.DeleteData(
                table: "DailyMenus",
                keyColumn: "dailyMenuId",
                keyValue: 21);

            migrationBuilder.DeleteData(
                table: "DailyMenus",
                keyColumn: "dailyMenuId",
                keyValue: 22);

            migrationBuilder.DeleteData(
                table: "DailyMenus",
                keyColumn: "dailyMenuId",
                keyValue: 23);

            migrationBuilder.DeleteData(
                table: "DailyMenus",
                keyColumn: "dailyMenuId",
                keyValue: 24);

            migrationBuilder.DeleteData(
                table: "DailyMenus",
                keyColumn: "dailyMenuId",
                keyValue: 25);

            migrationBuilder.DeleteData(
                table: "DailyMenus",
                keyColumn: "dailyMenuId",
                keyValue: 26);

            migrationBuilder.DeleteData(
                table: "DailyMenus",
                keyColumn: "dailyMenuId",
                keyValue: 27);

            migrationBuilder.DeleteData(
                table: "DailyMenus",
                keyColumn: "dailyMenuId",
                keyValue: 28);

            migrationBuilder.DeleteData(
                table: "DailyMenus",
                keyColumn: "dailyMenuId",
                keyValue: 29);

            migrationBuilder.DeleteData(
                table: "DailyMenus",
                keyColumn: "dailyMenuId",
                keyValue: 30);

            migrationBuilder.DeleteData(
                table: "DailyMenus",
                keyColumn: "dailyMenuId",
                keyValue: 31);

            migrationBuilder.DeleteData(
                table: "DailyMenus",
                keyColumn: "dailyMenuId",
                keyValue: 32);

            migrationBuilder.DeleteData(
                table: "DailyMenus",
                keyColumn: "dailyMenuId",
                keyValue: 33);

            migrationBuilder.DeleteData(
                table: "DailyMenus",
                keyColumn: "dailyMenuId",
                keyValue: 34);

            migrationBuilder.DeleteData(
                table: "DailyMenus",
                keyColumn: "dailyMenuId",
                keyValue: 35);

            migrationBuilder.DeleteData(
                table: "DailyMenus",
                keyColumn: "dailyMenuId",
                keyValue: 36);

            migrationBuilder.DeleteData(
                table: "DailyMenus",
                keyColumn: "dailyMenuId",
                keyValue: 37);

            migrationBuilder.DeleteData(
                table: "DailyMenus",
                keyColumn: "dailyMenuId",
                keyValue: 38);

            migrationBuilder.DeleteData(
                table: "DailyMenus",
                keyColumn: "dailyMenuId",
                keyValue: 39);

            migrationBuilder.DeleteData(
                table: "DailyMenus",
                keyColumn: "dailyMenuId",
                keyValue: 40);

            migrationBuilder.DeleteData(
                table: "DailyMenus",
                keyColumn: "dailyMenuId",
                keyValue: 41);

            migrationBuilder.DeleteData(
                table: "DailyMenus",
                keyColumn: "dailyMenuId",
                keyValue: 42);

            migrationBuilder.DeleteData(
                table: "DailyMenus",
                keyColumn: "dailyMenuId",
                keyValue: 43);

            migrationBuilder.DeleteData(
                table: "DailyMenus",
                keyColumn: "dailyMenuId",
                keyValue: 44);

            migrationBuilder.DeleteData(
                table: "DailyMenus",
                keyColumn: "dailyMenuId",
                keyValue: 45);

            migrationBuilder.DeleteData(
                table: "DailyMenus",
                keyColumn: "dailyMenuId",
                keyValue: 46);

            migrationBuilder.DropColumn(
                name: "category",
                table: "DailyMenus");

            migrationBuilder.InsertData(
                table: "DailyMenuDishes",
                columns: new[] { "dailyMenuId", "dishId" },
                values: new object[,]
                {
                    { 2, 1 },
                    { 3, 3 }
                });

            migrationBuilder.UpdateData(
                table: "DailyMenus",
                keyColumn: "dailyMenuId",
                keyValue: 1,
                column: "date",
                value: new DateOnly(2025, 11, 29));

            migrationBuilder.UpdateData(
                table: "DailyMenus",
                keyColumn: "dailyMenuId",
                keyValue: 2,
                column: "date",
                value: new DateOnly(2025, 12, 2));

            migrationBuilder.UpdateData(
                table: "DailyMenus",
                keyColumn: "dailyMenuId",
                keyValue: 3,
                column: "date",
                value: new DateOnly(2025, 12, 1));
        }
    }
}
