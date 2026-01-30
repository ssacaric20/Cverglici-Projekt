using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartMenza.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddReviewFieldsToDishRating : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Comment",
                table: "DishRatings",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "DishRatings",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "DishRatings",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "DishRatings",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "DishRatings",
                keyColumn: "DishRatingId",
                keyValue: 1,
                columns: new[] { "Comment", "CreatedAt", "UpdatedAt", "UserId" },
                values: new object[] { "Odlično jelo, preporučujem!", new DateTime(2025, 12, 1, 12, 0, 0, 0, DateTimeKind.Utc), null, 2 });

            migrationBuilder.UpdateData(
                table: "DishRatings",
                keyColumn: "DishRatingId",
                keyValue: 2,
                columns: new[] { "Comment", "CreatedAt", "UpdatedAt", "UserId" },
                values: new object[] { "Jako ukusno, ali malo premalo začinjeno.", new DateTime(2025, 12, 2, 14, 30, 0, 0, DateTimeKind.Utc), null, 2 });

            migrationBuilder.UpdateData(
                table: "DishRatings",
                keyColumn: "DishRatingId",
                keyValue: 3,
                columns: new[] { "Comment", "CreatedAt", "UpdatedAt", "UserId" },
                values: new object[] { "Solidno, ali ništa posebno.", new DateTime(2025, 12, 3, 18, 15, 0, 0, DateTimeKind.Utc), null, 2 });

            migrationBuilder.UpdateData(
                table: "DishRatings",
                keyColumn: "DishRatingId",
                keyValue: 4,
                columns: new[] { "Comment", "CreatedAt", "UpdatedAt", "UserId" },
                values: new object[] { "Dobra riba, svježa i ukusna!", new DateTime(2025, 12, 4, 13, 45, 0, 0, DateTimeKind.Utc), null, 1 });

            migrationBuilder.UpdateData(
                table: "DishRatings",
                keyColumn: "DishRatingId",
                keyValue: 5,
                columns: new[] { "Comment", "CreatedAt", "UpdatedAt", "UserId" },
                values: new object[] { "", new DateTime(2025, 12, 5, 19, 0, 0, 0, DateTimeKind.Utc), null, 1 });

            migrationBuilder.CreateIndex(
                name: "IX_DishRatings_UserId",
                table: "DishRatings",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_DishRatings_Users_UserId",
                table: "DishRatings",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DishRatings_Users_UserId",
                table: "DishRatings");

            migrationBuilder.DropIndex(
                name: "IX_DishRatings_UserId",
                table: "DishRatings");

            migrationBuilder.DropColumn(
                name: "Comment",
                table: "DishRatings");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "DishRatings");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "DishRatings");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "DishRatings");
        }
    }
}
