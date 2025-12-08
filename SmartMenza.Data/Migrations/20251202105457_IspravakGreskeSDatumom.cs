using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartMenza.Data.Migrations
{
    
    public partial class IspravakGreskeSDatumom : Migration
    {
        
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "DailyMenus",
                keyColumn: "dailyMenuId",
                keyValue: 2,
                column: "date",
                value: new DateOnly(2025, 12, 2));
        }

       
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "DailyMenus",
                keyColumn: "dailyMenuId",
                keyValue: 2,
                column: "date",
                value: new DateOnly(2025, 11, 30));
        }
    }
}
