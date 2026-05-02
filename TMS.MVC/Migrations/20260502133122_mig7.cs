using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TMS.MVC.Migrations
{
    /// <inheritdoc />
    public partial class mig7 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DriverWalletWithdrawalRequests",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DriverProfileId = table.Column<int>(type: "int", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,0)", nullable: false),
                    RequestNote = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    PaidAmount = table.Column<decimal>(type: "decimal(18,0)", nullable: true),
                    PaidAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PaidBy = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    PaymentReceiptNote = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    RejectedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RejectedBy = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    RejectionNote = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DriverWalletWithdrawalRequests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DriverWalletWithdrawalRequests_DriverProfiles_DriverProfileId",
                        column: x => x.DriverProfileId,
                        principalTable: "DriverProfiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DriverWalletWithdrawalRequests_DriverProfileId",
                table: "DriverWalletWithdrawalRequests",
                column: "DriverProfileId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DriverWalletWithdrawalRequests");
        }
    }
}
