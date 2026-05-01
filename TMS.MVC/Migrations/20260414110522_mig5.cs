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
            migrationBuilder.DropTable(
                name: "DriverAddresses");

            migrationBuilder.DropTable(
                name: "DriverBankAccounts");

            migrationBuilder.DropTable(
                name: "DriverContacts");

            migrationBuilder.DropTable(
                name: "DriverTractors");

            migrationBuilder.DropTable(
                name: "TractorOwnerLinks");

            migrationBuilder.DropTable(
                name: "TrailerInfos");

            migrationBuilder.DropTable(
                name: "DriverProfiles");

            migrationBuilder.DropTable(
                name: "TractorOwnerProfiles");

            migrationBuilder.AddColumn<string>(
                name: "OwnerApplicationUserId",
                table: "Tractors",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NationalId",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Tractors_OwnerApplicationUserId",
                table: "Tractors",
                column: "OwnerApplicationUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tractors_AspNetUsers_OwnerApplicationUserId",
                table: "Tractors",
                column: "OwnerApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tractors_AspNetUsers_OwnerApplicationUserId",
                table: "Tractors");

            migrationBuilder.DropIndex(
                name: "IX_Tractors_OwnerApplicationUserId",
                table: "Tractors");

            migrationBuilder.DropColumn(
                name: "OwnerApplicationUserId",
                table: "Tractors");

            migrationBuilder.DropColumn(
                name: "NationalId",
                table: "AspNetUsers");

            migrationBuilder.CreateTable(
                name: "DriverProfiles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ApplicationUserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    DrivingLicenseNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    NationalCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    SmartCardNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DriverProfiles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DriverProfiles_AspNetUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TractorOwnerProfiles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ApplicationUserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    NationalId = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TractorOwnerProfiles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TractorOwnerProfiles_AspNetUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TrailerInfos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TractorId = table.Column<int>(type: "int", nullable: false),
                    PlateNumber = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Title = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    TrailerCategory = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    TrailerType = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrailerInfos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TrailerInfos_Tractors_TractorId",
                        column: x => x.TractorId,
                        principalTable: "Tractors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DriverAddresses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DriverProfileId = table.Column<int>(type: "int", nullable: false),
                    AddressText = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    CityName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Phone = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Title = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DriverAddresses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DriverAddresses_DriverProfiles_DriverProfileId",
                        column: x => x.DriverProfileId,
                        principalTable: "DriverProfiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DriverBankAccounts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DriverProfileId = table.Column<int>(type: "int", nullable: false),
                    AccountNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    AccountOwnerName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    AccountType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    BankName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CardNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    DocumentIdentifier = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    OwnerDisplayName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    OwnerMismatch = table.Column<bool>(type: "bit", nullable: false),
                    ShebaNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    VerificationCode = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DriverBankAccounts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DriverBankAccounts_DriverProfiles_DriverProfileId",
                        column: x => x.DriverProfileId,
                        principalTable: "DriverProfiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DriverContacts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DriverProfileId = table.Column<int>(type: "int", nullable: false),
                    ConfirmedInSamava = table.Column<bool>(type: "bit", nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    RegisteredPhone = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    SmsNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Title = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    WhatsAppNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DriverContacts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DriverContacts_DriverProfiles_DriverProfileId",
                        column: x => x.DriverProfileId,
                        principalTable: "DriverProfiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DriverTractors",
                columns: table => new
                {
                    DriverProfileId = table.Column<int>(type: "int", nullable: false),
                    TractorId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DriverTractors", x => new { x.DriverProfileId, x.TractorId });
                    table.ForeignKey(
                        name: "FK_DriverTractors_DriverProfiles_DriverProfileId",
                        column: x => x.DriverProfileId,
                        principalTable: "DriverProfiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DriverTractors_Tractors_TractorId",
                        column: x => x.TractorId,
                        principalTable: "Tractors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TractorOwnerLinks",
                columns: table => new
                {
                    TractorOwnerProfileId = table.Column<int>(type: "int", nullable: false),
                    TractorId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TractorOwnerLinks", x => new { x.TractorOwnerProfileId, x.TractorId });
                    table.ForeignKey(
                        name: "FK_TractorOwnerLinks_TractorOwnerProfiles_TractorOwnerProfileId",
                        column: x => x.TractorOwnerProfileId,
                        principalTable: "TractorOwnerProfiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TractorOwnerLinks_Tractors_TractorId",
                        column: x => x.TractorId,
                        principalTable: "Tractors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DriverAddresses_DriverProfileId",
                table: "DriverAddresses",
                column: "DriverProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_DriverBankAccounts_DriverProfileId",
                table: "DriverBankAccounts",
                column: "DriverProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_DriverContacts_DriverProfileId",
                table: "DriverContacts",
                column: "DriverProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_DriverProfiles_ApplicationUserId",
                table: "DriverProfiles",
                column: "ApplicationUserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DriverTractors_TractorId",
                table: "DriverTractors",
                column: "TractorId");

            migrationBuilder.CreateIndex(
                name: "IX_TractorOwnerLinks_TractorId",
                table: "TractorOwnerLinks",
                column: "TractorId");

            migrationBuilder.CreateIndex(
                name: "IX_TractorOwnerProfiles_ApplicationUserId",
                table: "TractorOwnerProfiles",
                column: "ApplicationUserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TrailerInfos_TractorId",
                table: "TrailerInfos",
                column: "TractorId");
        }
    }
}
