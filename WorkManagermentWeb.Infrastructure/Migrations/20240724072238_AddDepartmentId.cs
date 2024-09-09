using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WorkManagermentWeb.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddDepartmentId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Boards_WorkSpaces_WorkSpaceId",
                table: "Boards");

            migrationBuilder.AlterColumn<string>(
                name: "Lang",
                table: "NotificationLanguages",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Content",
                table: "NotificationLanguages",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<Guid>(
                name: "WorkSpaceId",
                table: "Boards",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Boards_WorkSpaces_WorkSpaceId",
                table: "Boards",
                column: "WorkSpaceId",
                principalTable: "WorkSpaces",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Boards_WorkSpaces_WorkSpaceId",
                table: "Boards");

            migrationBuilder.AlterColumn<string>(
                name: "Lang",
                table: "NotificationLanguages",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Content",
                table: "NotificationLanguages",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "WorkSpaceId",
                table: "Boards",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AddForeignKey(
                name: "FK_Boards_WorkSpaces_WorkSpaceId",
                table: "Boards",
                column: "WorkSpaceId",
                principalTable: "WorkSpaces",
                principalColumn: "Id");
        }
    }
}
