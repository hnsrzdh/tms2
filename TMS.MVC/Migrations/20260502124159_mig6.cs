using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TMS.MVC.Migrations
{
    /// <inheritdoc />
    public partial class mig6 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CargoAnnouncements",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedByUserId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true),
                    CreatedByDisplayName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    CustomerCompanyName = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    ContactPersonName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    ContactMobile = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    ContactPhone = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    ContactEmail = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    AnnouncementType = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    RequiresFleetEntryPermit = table.Column<bool>(type: "bit", nullable: false),
                    OriginPlaceName = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    DestinationPlaceName = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    ProductName = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    ProductAmount = table.Column<decimal>(type: "decimal(18,3)", nullable: false),
                    Unit = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    LoadingStartDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LoadingEndDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ShortagePenaltyPerUnit = table.Column<decimal>(type: "decimal(18,3)", nullable: true),
                    CustomerNotes = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    OperatorNote = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    ReviewedByUserId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true),
                    ReviewedByDisplayName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    ReviewedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedHavalehNumber = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CargoAnnouncements", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CargoAnnouncements_ContactMobile",
                table: "CargoAnnouncements",
                column: "ContactMobile");

            migrationBuilder.CreateIndex(
                name: "IX_CargoAnnouncements_CustomerCompanyName",
                table: "CargoAnnouncements",
                column: "CustomerCompanyName");

            migrationBuilder.CreateIndex(
                name: "IX_CargoAnnouncements_Status_CreatedAt",
                table: "CargoAnnouncements",
                columns: new[] { "Status", "CreatedAt" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CargoAnnouncements");
        }
    }
}
