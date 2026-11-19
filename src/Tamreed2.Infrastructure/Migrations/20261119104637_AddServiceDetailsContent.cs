using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Tamreed2.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddServiceDetailsContent : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Details",
                table: "Services",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Details",
                table: "Services");
        }
    }
}
