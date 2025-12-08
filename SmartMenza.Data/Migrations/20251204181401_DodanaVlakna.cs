using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartMenza.Data.Migrations
{
    
    public partial class DodanaVlakna : Migration
    {
        
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "fiber",
                table: "Dishes",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.UpdateData(
                table: "Dishes",
                keyColumn: "dishId",
                keyValue: 1,
                column: "fiber",
                value: 30m);

            migrationBuilder.UpdateData(
                table: "Dishes",
                keyColumn: "dishId",
                keyValue: 2,
                column: "fiber",
                value: 30m);

            migrationBuilder.UpdateData(
                table: "Dishes",
                keyColumn: "dishId",
                keyValue: 3,
                column: "fiber",
                value: 0m);
        }

        
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "fiber",
                table: "Dishes");
        }
    }
}
