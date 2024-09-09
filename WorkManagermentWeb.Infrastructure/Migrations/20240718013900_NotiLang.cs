using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WorkManagermentWeb.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class NotiLang : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EventType",
                table: "Notifications");

            migrationBuilder.CreateTable(
                name: "NotificationLanguages",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    NotificationId = table.Column<Guid>(type: "uuid", nullable: false),
                    Lang = table.Column<string>(type: "text", nullable: false),
                    Content = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NotificationLanguages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NotificationLanguages_Notifications_NotificationId",
                        column: x => x.NotificationId,
                        principalTable: "Notifications",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_NotificationLanguages_NotificationId",
                table: "NotificationLanguages",
                column: "NotificationId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "NotificationLanguages");

            migrationBuilder.AddColumn<int>(
                name: "EventType",
                table: "Notifications",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }
    }
}
