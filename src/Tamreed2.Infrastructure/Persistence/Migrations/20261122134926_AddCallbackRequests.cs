using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Tamreed2.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddCallbackRequests : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CallbackRequests",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(120)", maxLength: 120, nullable: false),
                    MobileNumber = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    ServicesNeeded = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Message = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    Status = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    CreatedAtUtc = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    UpdatedAtUtc = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CallbackRequests", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CallbackRequests_CreatedAtUtc",
                table: "CallbackRequests",
                column: "CreatedAtUtc");

            migrationBuilder.CreateIndex(
                name: "IX_CallbackRequests_Status",
                table: "CallbackRequests",
                column: "Status");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CallbackRequests");
        }
    }
}
