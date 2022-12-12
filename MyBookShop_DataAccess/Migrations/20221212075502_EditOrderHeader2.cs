using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyBookShop.Migrations
{
    public partial class EditOrderHeader2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "InquiryDate",
                table: "OrderHeader",
                newName: "DateStartState");

            migrationBuilder.AddColumn<DateTime>(
                name: "DateEndState",
                table: "OrderHeader",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<double>(
                name: "FinalOrderTotal",
                table: "OrderHeader",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<string>(
                name: "OrderStatus",
                table: "OrderHeader",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "active_state",
                table: "OrderHeader",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateEndState",
                table: "OrderHeader");

            migrationBuilder.DropColumn(
                name: "FinalOrderTotal",
                table: "OrderHeader");

            migrationBuilder.DropColumn(
                name: "OrderStatus",
                table: "OrderHeader");

            migrationBuilder.DropColumn(
                name: "active_state",
                table: "OrderHeader");

            migrationBuilder.RenameColumn(
                name: "DateStartState",
                table: "OrderHeader",
                newName: "InquiryDate");
        }
    }
}
