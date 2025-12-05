using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace SmartMenza.Data.Migrations
{
    /// <inheritdoc />
    public partial class DodaneRecenzijeNaDish3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "DishRatings",
                columns: new[] { "dishRatingId", "dishId", "rating" },
                values: new object[,]
                {
                    { 3, 3, 3 },
                    { 4, 3, 4 },
                    { 5, 3, 4 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "DishRatings",
                keyColumn: "dishRatingId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "DishRatings",
                keyColumn: "dishRatingId",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "DishRatings",
                keyColumn: "dishRatingId",
                keyValue: 5);
        }
    }
}
