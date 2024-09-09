using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WorkManagermentWeb.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class CalendarEventRel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CalendarEvent_WorkItems_WorkItemId",
                table: "CalendarEvent");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CalendarEvent",
                table: "CalendarEvent");

            migrationBuilder.RenameTable(
                name: "CalendarEvent",
                newName: "CalendarEvents");

            migrationBuilder.RenameIndex(
                name: "IX_CalendarEvent_WorkItemId",
                table: "CalendarEvents",
                newName: "IX_CalendarEvents_WorkItemId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CalendarEvents",
                table: "CalendarEvents",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CalendarEvents_WorkItems_WorkItemId",
                table: "CalendarEvents",
                column: "WorkItemId",
                principalTable: "WorkItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CalendarEvents_WorkItems_WorkItemId",
                table: "CalendarEvents");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CalendarEvents",
                table: "CalendarEvents");

            migrationBuilder.RenameTable(
                name: "CalendarEvents",
                newName: "CalendarEvent");

            migrationBuilder.RenameIndex(
                name: "IX_CalendarEvents_WorkItemId",
                table: "CalendarEvent",
                newName: "IX_CalendarEvent_WorkItemId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CalendarEvent",
                table: "CalendarEvent",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CalendarEvent_WorkItems_WorkItemId",
                table: "CalendarEvent",
                column: "WorkItemId",
                principalTable: "WorkItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
