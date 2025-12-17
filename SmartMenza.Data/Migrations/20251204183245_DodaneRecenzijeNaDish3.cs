using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 

namespace SmartMenza.Data.Migrations
{
    
    public partial class DodaneRecenzijeNaDish3 : Migration
    {
        
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
