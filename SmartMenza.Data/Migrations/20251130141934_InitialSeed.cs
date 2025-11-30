using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartMenza.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialSeed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "DailyMenus",
                keyColumn: "dailyMenuId",
                keyValue: 1,
                columns: new[] { "date", "dishId" },
                values: new object[] { new DateOnly(2025, 11, 30), 2 });

            migrationBuilder.UpdateData(
                table: "DailyMenus",
                keyColumn: "dailyMenuId",
                keyValue: 2,
                column: "date",
                value: new DateOnly(2025, 11, 30));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "DailyMenus",
                keyColumn: "dailyMenuId",
                keyValue: 1,
                columns: new[] { "date", "dishId" },
                values: new object[] { new DateOnly(2025, 1, 1), 1 });

            migrationBuilder.UpdateData(
                table: "DailyMenus",
                keyColumn: "dailyMenuId",
                keyValue: 2,
                column: "date",
                value: new DateOnly(2025, 11, 22));
        }
    }
}
