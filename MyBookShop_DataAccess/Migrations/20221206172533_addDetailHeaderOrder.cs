using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyBookShop.Migrations
{
    public partial class addDetailHeaderOrder : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "City",
                table: "OrderHeader",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Region",
                table: "OrderHeader",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "StreetAddress",
                table: "OrderHeader",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "City",
                table: "OrderHeader");

            migrationBuilder.DropColumn(
                name: "Region",
                table: "OrderHeader");

            migrationBuilder.DropColumn(
                name: "StreetAddress",
                table: "OrderHeader");
        }
    }
}
