using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace SmartMenza.Data.Migrations
{
    /// <inheritdoc />
    public partial class RemoveDishRatingsSeed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "DishRatings",
                keyColumn: "DishRatingId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "DishRatings",
                keyColumn: "DishRatingId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "DishRatings",
                keyColumn: "DishRatingId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "DishRatings",
                keyColumn: "DishRatingId",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "DishRatings",
                keyColumn: "DishRatingId",
                keyValue: 5);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "DishRatings",
                columns: new[] { "DishRatingId", "Comment", "CreatedAt", "DishId", "Rating", "UpdatedAt", "UserId" },
                values: new object[,]
                {
                    { 1, "Odlično jelo, preporučujem!", new DateTime(2025, 12, 1, 12, 0, 0, 0, DateTimeKind.Utc), 1, 5, null, 2 },
                    { 2, "Jako ukusno, ali malo premalo začinjeno.", new DateTime(2025, 12, 2, 14, 30, 0, 0, DateTimeKind.Utc), 2, 4, null, 2 },
                    { 3, "Solidno, ali ništa posebno.", new DateTime(2025, 12, 3, 18, 15, 0, 0, DateTimeKind.Utc), 3, 3, null, 2 },
                    { 4, "Dobra riba, svježa i ukusna!", new DateTime(2025, 12, 4, 13, 45, 0, 0, DateTimeKind.Utc), 3, 4, null, 1 },
                    { 5, "", new DateTime(2025, 12, 5, 19, 0, 0, 0, DateTimeKind.Utc), 3, 4, null, 1 }
                });
        }
    }
}
