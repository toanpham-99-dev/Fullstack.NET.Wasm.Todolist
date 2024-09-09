using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WorkManagermentWeb.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateBoard : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AssigneeId",
                table: "Boards",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AssigneeName",
                table: "Boards",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<DateOnly>(
                name: "EndDate",
                table: "Boards",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1));

            migrationBuilder.AddColumn<DateOnly>(
                name: "StartDate",
                table: "Boards",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AssigneeId",
                table: "Boards");

            migrationBuilder.DropColumn(
                name: "AssigneeName",
                table: "Boards");

            migrationBuilder.DropColumn(
                name: "EndDate",
                table: "Boards");

            migrationBuilder.DropColumn(
                name: "StartDate",
                table: "Boards");
        }
    }
}
