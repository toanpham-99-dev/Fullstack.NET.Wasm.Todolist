using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WorkManagermentWeb.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class NotiLangRelate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_NotificationLanguages_NotificationId",
                table: "NotificationLanguages");

            migrationBuilder.CreateIndex(
                name: "IX_NotificationLanguages_NotificationId",
                table: "NotificationLanguages",
                column: "NotificationId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_NotificationLanguages_NotificationId",
                table: "NotificationLanguages");

            migrationBuilder.CreateIndex(
                name: "IX_NotificationLanguages_NotificationId",
                table: "NotificationLanguages",
                column: "NotificationId",
                unique: true);
        }
    }
}
