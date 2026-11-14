using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Tamreed2.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RemoveNurseAssignment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AssignedNurseId",
                table: "Bookings");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "AssignedNurseId",
                table: "Bookings",
                type: "uniqueidentifier",
                nullable: true);
        }
    }
}
