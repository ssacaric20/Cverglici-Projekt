using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartMenza.Data.Migrations
{
    
    public partial class PonovnoKreiranjeBaze : Migration
    {
        
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "nutricionalValueId",
                table: "Dishes");
        }

        
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "nutricionalValueId",
                table: "Dishes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "Dishes",
                keyColumn: "dishId",
                keyValue: 1,
                column: "nutricionalValueId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Dishes",
                keyColumn: "dishId",
                keyValue: 2,
                column: "nutricionalValueId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Dishes",
                keyColumn: "dishId",
                keyValue: 3,
                column: "nutricionalValueId",
                value: 1);
        }
    }
}
