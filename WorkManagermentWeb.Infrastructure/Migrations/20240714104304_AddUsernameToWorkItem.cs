using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WorkManagermentWeb.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddUsernameToWorkItem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "LastUpdateBy",
                table: "WorkItems",
                newName: "LastUpdaterName");

            migrationBuilder.RenameColumn(
                name: "CreatedBy",
                table: "WorkItems",
                newName: "LastUpdaterId");

            migrationBuilder.RenameColumn(
                name: "AssignedTo",
                table: "WorkItems",
                newName: "CreatorName");

            migrationBuilder.AddColumn<string>(
                name: "AssigneeId",
                table: "WorkItems",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AssigneeName",
                table: "WorkItems",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatorId",
                table: "WorkItems",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AssigneeId",
                table: "WorkItems");

            migrationBuilder.DropColumn(
                name: "AssigneeName",
                table: "WorkItems");

            migrationBuilder.DropColumn(
                name: "CreatorId",
                table: "WorkItems");

            migrationBuilder.RenameColumn(
                name: "LastUpdaterName",
                table: "WorkItems",
                newName: "LastUpdateBy");

            migrationBuilder.RenameColumn(
                name: "LastUpdaterId",
                table: "WorkItems",
                newName: "CreatedBy");

            migrationBuilder.RenameColumn(
                name: "CreatorName",
                table: "WorkItems",
                newName: "AssignedTo");
        }
    }
}
