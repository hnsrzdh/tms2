using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TMS.MVC.Migrations
{
    /// <inheritdoc />
    public partial class mig5 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SubHavalehAssignmentRequests",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SubHavalehId = table.Column<long>(type: "bigint", nullable: false),
                    TractorId = table.Column<int>(type: "int", nullable: false),
                    RequesterUserId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: false),
                    DriverProfileId = table.Column<int>(type: "int", nullable: true),
                    RequestedCargoAmount = table.Column<decimal>(type: "decimal(18,3)", nullable: false),
                    IsTruckCapacityFull = table.Column<bool>(type: "bit", nullable: false),
                    RequestedLoadingDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RequesterNote = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ReviewedByUserId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true),
                    ReviewedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    OperatorNote = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    CreatedTractorAssignmentId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubHavalehAssignmentRequests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SubHavalehAssignmentRequests_AspNetUsers_RequesterUserId",
                        column: x => x.RequesterUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SubHavalehAssignmentRequests_AspNetUsers_ReviewedByUserId",
                        column: x => x.ReviewedByUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SubHavalehAssignmentRequests_DriverProfiles_DriverProfileId",
                        column: x => x.DriverProfileId,
                        principalTable: "DriverProfiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_SubHavalehAssignmentRequests_SubHavalehs_SubHavalehId",
                        column: x => x.SubHavalehId,
                        principalTable: "SubHavalehs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SubHavalehAssignmentRequests_TractorAssignments_CreatedTractorAssignmentId",
                        column: x => x.CreatedTractorAssignmentId,
                        principalTable: "TractorAssignments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_SubHavalehAssignmentRequests_Tractors_TractorId",
                        column: x => x.TractorId,
                        principalTable: "Tractors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SubHavalehAssignmentRequests_CreatedTractorAssignmentId",
                table: "SubHavalehAssignmentRequests",
                column: "CreatedTractorAssignmentId");

            migrationBuilder.CreateIndex(
                name: "IX_SubHavalehAssignmentRequests_DriverProfileId",
                table: "SubHavalehAssignmentRequests",
                column: "DriverProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_SubHavalehAssignmentRequests_RequesterUserId",
                table: "SubHavalehAssignmentRequests",
                column: "RequesterUserId");

            migrationBuilder.CreateIndex(
                name: "IX_SubHavalehAssignmentRequests_ReviewedByUserId",
                table: "SubHavalehAssignmentRequests",
                column: "ReviewedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_SubHavalehAssignmentRequests_Status_CreatedAt",
                table: "SubHavalehAssignmentRequests",
                columns: new[] { "Status", "CreatedAt" });

            migrationBuilder.CreateIndex(
                name: "IX_SubHavalehAssignmentRequests_SubHavalehId_Status",
                table: "SubHavalehAssignmentRequests",
                columns: new[] { "SubHavalehId", "Status" });

            migrationBuilder.CreateIndex(
                name: "IX_SubHavalehAssignmentRequests_TractorId_Status",
                table: "SubHavalehAssignmentRequests",
                columns: new[] { "TractorId", "Status" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SubHavalehAssignmentRequests");
        }
    }
}
