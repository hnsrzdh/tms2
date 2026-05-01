using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TMS.MVC.Migrations
{
    /// <inheritdoc />
    public partial class mig14 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_LocationTrackings_TractorAssignmentId",
                table: "LocationTrackings");

            migrationBuilder.AddColumn<string>(
                name: "RejectionNote",
                table: "UnloadingDocuments",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CancellationDate",
                table: "TractorAssignments",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CancellationReason",
                table: "TractorAssignments",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CancellationRequestDate",
                table: "TractorAssignments",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CancelledBy",
                table: "TractorAssignments",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsCancellationRequestedByDriver",
                table: "TractorAssignments",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "RejectionNote",
                table: "LoadingDocuments",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ChatMessages",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TractorAssignmentId = table.Column<long>(type: "bigint", nullable: false),
                    SenderId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    SenderName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    SenderRole = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Message = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    FilePath = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    FileName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    FileContentType = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    FileSize = table.Column<long>(type: "bigint", nullable: true),
                    SentDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsRead = table.Column<bool>(type: "bit", nullable: false),
                    ReadDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChatMessages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ChatMessages_TractorAssignments_TractorAssignmentId",
                        column: x => x.TractorAssignmentId,
                        principalTable: "TractorAssignments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LocationTrackings_TractorAssignmentId_Timestamp",
                table: "LocationTrackings",
                columns: new[] { "TractorAssignmentId", "Timestamp" });

            migrationBuilder.CreateIndex(
                name: "IX_ChatMessages_TractorAssignmentId_IsRead",
                table: "ChatMessages",
                columns: new[] { "TractorAssignmentId", "IsRead" });

            migrationBuilder.CreateIndex(
                name: "IX_ChatMessages_TractorAssignmentId_SentDate",
                table: "ChatMessages",
                columns: new[] { "TractorAssignmentId", "SentDate" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ChatMessages");

            migrationBuilder.DropIndex(
                name: "IX_LocationTrackings_TractorAssignmentId_Timestamp",
                table: "LocationTrackings");

            migrationBuilder.DropColumn(
                name: "RejectionNote",
                table: "UnloadingDocuments");

            migrationBuilder.DropColumn(
                name: "CancellationDate",
                table: "TractorAssignments");

            migrationBuilder.DropColumn(
                name: "CancellationReason",
                table: "TractorAssignments");

            migrationBuilder.DropColumn(
                name: "CancellationRequestDate",
                table: "TractorAssignments");

            migrationBuilder.DropColumn(
                name: "CancelledBy",
                table: "TractorAssignments");

            migrationBuilder.DropColumn(
                name: "IsCancellationRequestedByDriver",
                table: "TractorAssignments");

            migrationBuilder.DropColumn(
                name: "RejectionNote",
                table: "LoadingDocuments");

            migrationBuilder.CreateIndex(
                name: "IX_LocationTrackings_TractorAssignmentId",
                table: "LocationTrackings",
                column: "TractorAssignmentId");
        }
    }
}
