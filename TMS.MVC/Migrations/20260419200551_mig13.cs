using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TMS.MVC.Migrations
{
    /// <inheritdoc />
    public partial class mig13 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TractorAssignments",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SubHavalehId = table.Column<long>(type: "bigint", nullable: false),
                    TractorId = table.Column<int>(type: "int", nullable: false),
                    DriverProfileId = table.Column<int>(type: "int", nullable: true),
                    AssignmentDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    ArrivalAtOriginDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsArrivalAtOriginConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    ArrivalAtOriginConfirmedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LoadingStartDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LoadingEndDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LoadedAmount = table.Column<decimal>(type: "decimal(18,3)", nullable: true),
                    IsLoadingConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    LoadingConfirmedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ArrivalAtDestinationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsArrivalAtDestinationConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    ArrivalAtDestinationConfirmedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UnloadingStartDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UnloadingEndDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UnloadedAmount = table.Column<decimal>(type: "decimal(18,3)", nullable: true),
                    ShortageAmount = table.Column<decimal>(type: "decimal(18,3)", nullable: true),
                    IsUnloadingConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    UnloadingConfirmedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FinalFare = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    ShortagePenalty = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    DelayPenalty = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    PayableAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    IsSettled = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TractorAssignments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TractorAssignments_DriverProfiles_DriverProfileId",
                        column: x => x.DriverProfileId,
                        principalTable: "DriverProfiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_TractorAssignments_SubHavalehs_SubHavalehId",
                        column: x => x.SubHavalehId,
                        principalTable: "SubHavalehs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TractorAssignments_Tractors_TractorId",
                        column: x => x.TractorId,
                        principalTable: "Tractors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "LoadingDocuments",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TractorAssignmentId = table.Column<long>(type: "bigint", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    FilePath = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    DocumentType = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    UploadDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UploadedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsApproved = table.Column<bool>(type: "bit", nullable: false),
                    ApprovedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ApprovalDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LoadingDocuments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LoadingDocuments_TractorAssignments_TractorAssignmentId",
                        column: x => x.TractorAssignmentId,
                        principalTable: "TractorAssignments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LocationTrackings",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TractorAssignmentId = table.Column<long>(type: "bigint", nullable: false),
                    Latitude = table.Column<decimal>(type: "decimal(10,7)", nullable: false),
                    Longitude = table.Column<decimal>(type: "decimal(10,7)", nullable: false),
                    Timestamp = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Speed = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Heading = table.Column<decimal>(type: "decimal(18,2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LocationTrackings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LocationTrackings_TractorAssignments_TractorAssignmentId",
                        column: x => x.TractorAssignmentId,
                        principalTable: "TractorAssignments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UnloadingDocuments",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TractorAssignmentId = table.Column<long>(type: "bigint", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    FilePath = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    DocumentType = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    UploadDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UploadedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsApproved = table.Column<bool>(type: "bit", nullable: false),
                    ApprovedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ApprovalDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UnloadingDocuments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UnloadingDocuments_TractorAssignments_TractorAssignmentId",
                        column: x => x.TractorAssignmentId,
                        principalTable: "TractorAssignments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LoadingDocuments_TractorAssignmentId",
                table: "LoadingDocuments",
                column: "TractorAssignmentId");

            migrationBuilder.CreateIndex(
                name: "IX_LocationTrackings_TractorAssignmentId",
                table: "LocationTrackings",
                column: "TractorAssignmentId");

            migrationBuilder.CreateIndex(
                name: "IX_TractorAssignments_DriverProfileId",
                table: "TractorAssignments",
                column: "DriverProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_TractorAssignments_SubHavalehId",
                table: "TractorAssignments",
                column: "SubHavalehId");

            migrationBuilder.CreateIndex(
                name: "IX_TractorAssignments_TractorId",
                table: "TractorAssignments",
                column: "TractorId");

            migrationBuilder.CreateIndex(
                name: "IX_UnloadingDocuments_TractorAssignmentId",
                table: "UnloadingDocuments",
                column: "TractorAssignmentId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LoadingDocuments");

            migrationBuilder.DropTable(
                name: "LocationTrackings");

            migrationBuilder.DropTable(
                name: "UnloadingDocuments");

            migrationBuilder.DropTable(
                name: "TractorAssignments");
        }
    }
}
