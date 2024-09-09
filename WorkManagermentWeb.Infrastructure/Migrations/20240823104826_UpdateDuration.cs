using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WorkManagermentWeb.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDuration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<double>(
                name: "WorkRemain",
                table: "WorkItems",
                type: "double precision",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "WorkRemain",
                table: "WorkItems",
                type: "numeric",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "double precision");
        }
    }
}
