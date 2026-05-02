using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TMS.MVC.Migrations
{
    /// <inheritdoc />
    public partial class mig1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NationalId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LegalEntities",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CompanyName = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    CompanyType = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    City = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LegalEntities", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PermissionDefinitions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Key = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Category = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PermissionDefinitions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Places",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CountryName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    ProvinceName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    CityName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    Address = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Latitude = table.Column<decimal>(type: "decimal(10,7)", nullable: true),
                    Longitude = table.Column<decimal>(type: "decimal(10,7)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Places", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    Type = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TicketCategories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    SortOrder = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TicketCategories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TransportAgreements",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    CargoOwnerName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Origin = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Destination = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    ProductName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Unit = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Status = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransportAgreements", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DriverProfiles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ApplicationUserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    SmartCardNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    DrivingLicenseNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    IsBlocked = table.Column<bool>(type: "bit", nullable: false),
                    BlockDescription = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    WalletBalance = table.Column<decimal>(type: "decimal(18,2)", nullable: true)
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
                name: "Tractors",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PolicePlateNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    NationalId = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    TractorSmartCardNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    FileNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Status = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    TractorIdentifier = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    MaxLoadCapacity = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    CapacityUnit = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    TechnicalInspectionExpireDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ThirdPartyInsuranceExpireDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    WalletBalance = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    ProductionYear = table.Column<int>(type: "int", nullable: true),
                    AxleCount = table.Column<int>(type: "int", nullable: true),
                    SystemName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    TractorType = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    TransitPlateNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    OwnerApplicationUserId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tractors", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tractors_AspNetUsers_OwnerApplicationUserId",
                        column: x => x.OwnerApplicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "LegalEntityAddresses",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LegalEntityId = table.Column<long>(type: "bigint", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    AddressText = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    PostalCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LegalEntityAddresses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LegalEntityAddresses_LegalEntities_LegalEntityId",
                        column: x => x.LegalEntityId,
                        principalTable: "LegalEntities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LegalEntityBankAccounts",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LegalEntityId = table.Column<long>(type: "bigint", nullable: false),
                    Iban = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    AccountHolderName = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    VerifiedName = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LegalEntityBankAccounts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LegalEntityBankAccounts_LegalEntities_LegalEntityId",
                        column: x => x.LegalEntityId,
                        principalTable: "LegalEntities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LegalEntityContacts",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LegalEntityId = table.Column<long>(type: "bigint", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    ContactValue = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    HasSms = table.Column<bool>(type: "bit", nullable: false),
                    HasWhatsApp = table.Column<bool>(type: "bit", nullable: false),
                    IsFax = table.Column<bool>(type: "bit", nullable: false),
                    IsPhone = table.Column<bool>(type: "bit", nullable: false),
                    IsEmail = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LegalEntityContacts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LegalEntityContacts_LegalEntities_LegalEntityId",
                        column: x => x.LegalEntityId,
                        principalTable: "LegalEntities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RolePermissions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleName = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    PermissionDefinitionId = table.Column<int>(type: "int", nullable: false),
                    IsGranted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RolePermissions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RolePermissions_PermissionDefinitions_PermissionDefinitionId",
                        column: x => x.PermissionDefinitionId,
                        principalTable: "PermissionDefinitions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserPermissions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    PermissionDefinitionId = table.Column<int>(type: "int", nullable: false),
                    IsGranted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserPermissions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserPermissions_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserPermissions_PermissionDefinitions_PermissionDefinitionId",
                        column: x => x.PermissionDefinitionId,
                        principalTable: "PermissionDefinitions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Havalehs",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HavalehNumber = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ContractNumber = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    HavalehType = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    RequiresFleetEntryPermit = table.Column<bool>(type: "bit", nullable: false),
                    TransportContractorLegalEntityId = table.Column<long>(type: "bigint", nullable: true),
                    GoodsOwnerLegalEntityId = table.Column<long>(type: "bigint", nullable: true),
                    OriginPlaceId = table.Column<long>(type: "bigint", nullable: true),
                    ProductId = table.Column<long>(type: "bigint", nullable: true),
                    ProductAmount = table.Column<decimal>(type: "decimal(18,3)", nullable: true),
                    Unit = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    PurchaseDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    AllowedLoadingDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ShortagePenaltyPerUnit = table.Column<decimal>(type: "decimal(18,3)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Havalehs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Havalehs_LegalEntities_GoodsOwnerLegalEntityId",
                        column: x => x.GoodsOwnerLegalEntityId,
                        principalTable: "LegalEntities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Havalehs_LegalEntities_TransportContractorLegalEntityId",
                        column: x => x.TransportContractorLegalEntityId,
                        principalTable: "LegalEntities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Havalehs_Places_OriginPlaceId",
                        column: x => x.OriginPlaceId,
                        principalTable: "Places",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Havalehs_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Tickets",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Subject = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    CategoryId = table.Column<int>(type: "int", nullable: false),
                    Priority = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    CreatedByUserId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: false),
                    AssignedToUserId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastReplyAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ClosedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RelatedEntityType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    RelatedEntityId = table.Column<long>(type: "bigint", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tickets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tickets_TicketCategories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "TicketCategories",
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
                    Title = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    AddressText = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true)
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
                    AccountOwnerName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    BankName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    AccountNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    CardNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ShebaNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    IsDefault = table.Column<bool>(type: "bit", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true)
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
                    Title = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ContactValue = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    HasSms = table.Column<bool>(type: "bit", nullable: false),
                    HasWhatsApp = table.Column<bool>(type: "bit", nullable: false),
                    IsFax = table.Column<bool>(type: "bit", nullable: false),
                    IsPhone = table.Column<bool>(type: "bit", nullable: false),
                    IsEmail = table.Column<bool>(type: "bit", nullable: false)
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
                name: "TractorAddresses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TractorId = table.Column<int>(type: "int", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    AddressText = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TractorAddresses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TractorAddresses_Tractors_TractorId",
                        column: x => x.TractorId,
                        principalTable: "Tractors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TractorBankAccounts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TractorId = table.Column<int>(type: "int", nullable: false),
                    AccountOwnerName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    BankName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    AccountNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    CardNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ShebaNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    IsDefault = table.Column<bool>(type: "bit", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TractorBankAccounts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TractorBankAccounts_Tractors_TractorId",
                        column: x => x.TractorId,
                        principalTable: "Tractors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TractorContacts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TractorId = table.Column<int>(type: "int", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ContactValue = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    HasSms = table.Column<bool>(type: "bit", nullable: false),
                    HasWhatsApp = table.Column<bool>(type: "bit", nullable: false),
                    IsFax = table.Column<bool>(type: "bit", nullable: false),
                    IsPhone = table.Column<bool>(type: "bit", nullable: false),
                    IsEmail = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TractorContacts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TractorContacts_Tractors_TractorId",
                        column: x => x.TractorId,
                        principalTable: "Tractors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SubHavalehs",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HavalehId = table.Column<long>(type: "bigint", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    ContractType = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    SettlementBase = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    TransportType = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    DestinationPlaceId = table.Column<long>(type: "bigint", nullable: true),
                    DriverCurrencyType = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    DriverCurrencyRate = table.Column<decimal>(type: "decimal(18,4)", nullable: true),
                    GoodsOwnerCurrencyType = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    GoodsOwnerCurrencyRate = table.Column<decimal>(type: "decimal(18,4)", nullable: true),
                    GoodsOwnerPricePerTon = table.Column<decimal>(type: "decimal(18,4)", nullable: true),
                    GoodsOwnerTip = table.Column<decimal>(type: "decimal(18,4)", nullable: true),
                    DriverPricePerTon = table.Column<decimal>(type: "decimal(18,4)", nullable: true),
                    DriverTip = table.Column<decimal>(type: "decimal(18,4)", nullable: true),
                    GoodsOwnerStopFee = table.Column<decimal>(type: "decimal(18,4)", nullable: true),
                    DriverStopFee = table.Column<decimal>(type: "decimal(18,4)", nullable: true),
                    AllowedLoadingTime = table.Column<int>(type: "int", nullable: true),
                    AllowedDeliveryTime = table.Column<int>(type: "int", nullable: true),
                    LateDeliveryPenalty = table.Column<decimal>(type: "decimal(18,4)", nullable: true),
                    LateDeliveryPenaltyType = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    ShortagePenaltyType = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    ShortageType = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    FixedShortageAmount = table.Column<decimal>(type: "decimal(18,4)", nullable: true),
                    AcceptableWeightLoss = table.Column<decimal>(type: "decimal(18,4)", nullable: true),
                    IsUnderSupervisor = table.Column<bool>(type: "bit", nullable: false),
                    RequestedCargoAmountType = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    RequestedCargoAmount = table.Column<decimal>(type: "decimal(18,3)", nullable: true),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubHavalehs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SubHavalehs_Havalehs_HavalehId",
                        column: x => x.HavalehId,
                        principalTable: "Havalehs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SubHavalehs_Places_DestinationPlaceId",
                        column: x => x.DestinationPlaceId,
                        principalTable: "Places",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TicketMessages",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TicketId = table.Column<long>(type: "bigint", nullable: false),
                    Body = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsInternal = table.Column<bool>(type: "bit", nullable: false),
                    CreatedByUserId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TicketMessages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TicketMessages_Tickets_TicketId",
                        column: x => x.TicketId,
                        principalTable: "Tickets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TicketStatusHistories",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TicketId = table.Column<long>(type: "bigint", nullable: false),
                    FromStatus = table.Column<int>(type: "int", nullable: false),
                    ToStatus = table.Column<int>(type: "int", nullable: false),
                    ByUserId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true),
                    At = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Note = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TicketStatusHistories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TicketStatusHistories_Tickets_TicketId",
                        column: x => x.TicketId,
                        principalTable: "Tickets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SubHavalehIntermediatePlaces",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SubHavalehId = table.Column<long>(type: "bigint", nullable: false),
                    PlaceId = table.Column<long>(type: "bigint", nullable: false),
                    SortOrder = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubHavalehIntermediatePlaces", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SubHavalehIntermediatePlaces_Places_PlaceId",
                        column: x => x.PlaceId,
                        principalTable: "Places",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SubHavalehIntermediatePlaces_SubHavalehs_SubHavalehId",
                        column: x => x.SubHavalehId,
                        principalTable: "SubHavalehs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

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
                    CancellationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CancelledBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CancellationReason = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    IsCancellationRequestedByDriver = table.Column<bool>(type: "bit", nullable: false),
                    CancellationRequestDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    FinalFare = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    ShortagePenalty = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    DelayPenalty = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    PayableAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    IsSettled = table.Column<bool>(type: "bit", nullable: false),
                    SettledTo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    SettledBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SettledDate = table.Column<DateTime>(type: "datetime2", nullable: true)
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
                name: "TicketAttachments",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TicketMessageId = table.Column<long>(type: "bigint", nullable: false),
                    FilePath = table.Column<string>(type: "nvarchar(400)", maxLength: 400, nullable: false),
                    OriginalFileName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    SizeBytes = table.Column<long>(type: "bigint", nullable: false),
                    UploadedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TicketAttachments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TicketAttachments_TicketMessages_TicketMessageId",
                        column: x => x.TicketMessageId,
                        principalTable: "TicketMessages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

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
                    RejectionNote = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
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
                    RejectionNote = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
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
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_ChatMessages_TractorAssignmentId_IsRead",
                table: "ChatMessages",
                columns: new[] { "TractorAssignmentId", "IsRead" });

            migrationBuilder.CreateIndex(
                name: "IX_ChatMessages_TractorAssignmentId_SentDate",
                table: "ChatMessages",
                columns: new[] { "TractorAssignmentId", "SentDate" });

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
                name: "IX_Havalehs_GoodsOwnerLegalEntityId",
                table: "Havalehs",
                column: "GoodsOwnerLegalEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_Havalehs_HavalehNumber",
                table: "Havalehs",
                column: "HavalehNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Havalehs_OriginPlaceId",
                table: "Havalehs",
                column: "OriginPlaceId");

            migrationBuilder.CreateIndex(
                name: "IX_Havalehs_ProductId",
                table: "Havalehs",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Havalehs_TransportContractorLegalEntityId",
                table: "Havalehs",
                column: "TransportContractorLegalEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_LegalEntityAddresses_LegalEntityId",
                table: "LegalEntityAddresses",
                column: "LegalEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_LegalEntityBankAccounts_LegalEntityId",
                table: "LegalEntityBankAccounts",
                column: "LegalEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_LegalEntityContacts_LegalEntityId",
                table: "LegalEntityContacts",
                column: "LegalEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_LoadingDocuments_TractorAssignmentId",
                table: "LoadingDocuments",
                column: "TractorAssignmentId");

            migrationBuilder.CreateIndex(
                name: "IX_LocationTrackings_TractorAssignmentId_Timestamp",
                table: "LocationTrackings",
                columns: new[] { "TractorAssignmentId", "Timestamp" });

            migrationBuilder.CreateIndex(
                name: "IX_PermissionDefinitions_Key",
                table: "PermissionDefinitions",
                column: "Key",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Places_CountryName_ProvinceName_CityName_Name",
                table: "Places",
                columns: new[] { "CountryName", "ProvinceName", "CityName", "Name" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Products_Name",
                table: "Products",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RolePermissions_PermissionDefinitionId",
                table: "RolePermissions",
                column: "PermissionDefinitionId");

            migrationBuilder.CreateIndex(
                name: "IX_RolePermissions_RoleName_PermissionDefinitionId",
                table: "RolePermissions",
                columns: new[] { "RoleName", "PermissionDefinitionId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SubHavalehIntermediatePlaces_PlaceId",
                table: "SubHavalehIntermediatePlaces",
                column: "PlaceId");

            migrationBuilder.CreateIndex(
                name: "IX_SubHavalehIntermediatePlaces_SubHavalehId",
                table: "SubHavalehIntermediatePlaces",
                column: "SubHavalehId");

            migrationBuilder.CreateIndex(
                name: "IX_SubHavalehs_DestinationPlaceId",
                table: "SubHavalehs",
                column: "DestinationPlaceId");

            migrationBuilder.CreateIndex(
                name: "IX_SubHavalehs_HavalehId",
                table: "SubHavalehs",
                column: "HavalehId");

            migrationBuilder.CreateIndex(
                name: "IX_TicketAttachments_TicketMessageId",
                table: "TicketAttachments",
                column: "TicketMessageId");

            migrationBuilder.CreateIndex(
                name: "IX_TicketCategories_IsActive_SortOrder",
                table: "TicketCategories",
                columns: new[] { "IsActive", "SortOrder" });

            migrationBuilder.CreateIndex(
                name: "IX_TicketMessages_TicketId_CreatedAt",
                table: "TicketMessages",
                columns: new[] { "TicketId", "CreatedAt" });

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_CategoryId",
                table: "Tickets",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_Code",
                table: "Tickets",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_CreatedByUserId",
                table: "Tickets",
                column: "CreatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_Status_Priority_CreatedAt",
                table: "Tickets",
                columns: new[] { "Status", "Priority", "CreatedAt" });

            migrationBuilder.CreateIndex(
                name: "IX_TicketStatusHistories_TicketId",
                table: "TicketStatusHistories",
                column: "TicketId");

            migrationBuilder.CreateIndex(
                name: "IX_TractorAddresses_TractorId",
                table: "TractorAddresses",
                column: "TractorId");

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
                name: "IX_TractorBankAccounts_TractorId",
                table: "TractorBankAccounts",
                column: "TractorId");

            migrationBuilder.CreateIndex(
                name: "IX_TractorContacts_TractorId",
                table: "TractorContacts",
                column: "TractorId");

            migrationBuilder.CreateIndex(
                name: "IX_Tractors_OwnerApplicationUserId",
                table: "Tractors",
                column: "OwnerApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Tractors_PolicePlateNumber",
                table: "Tractors",
                column: "PolicePlateNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TransportAgreements_Title",
                table: "TransportAgreements",
                column: "Title");

            migrationBuilder.CreateIndex(
                name: "IX_UnloadingDocuments_TractorAssignmentId",
                table: "UnloadingDocuments",
                column: "TractorAssignmentId");

            migrationBuilder.CreateIndex(
                name: "IX_UserPermissions_PermissionDefinitionId",
                table: "UserPermissions",
                column: "PermissionDefinitionId");

            migrationBuilder.CreateIndex(
                name: "IX_UserPermissions_UserId_PermissionDefinitionId",
                table: "UserPermissions",
                columns: new[] { "UserId", "PermissionDefinitionId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "ChatMessages");

            migrationBuilder.DropTable(
                name: "DriverAddresses");

            migrationBuilder.DropTable(
                name: "DriverBankAccounts");

            migrationBuilder.DropTable(
                name: "DriverContacts");

            migrationBuilder.DropTable(
                name: "LegalEntityAddresses");

            migrationBuilder.DropTable(
                name: "LegalEntityBankAccounts");

            migrationBuilder.DropTable(
                name: "LegalEntityContacts");

            migrationBuilder.DropTable(
                name: "LoadingDocuments");

            migrationBuilder.DropTable(
                name: "LocationTrackings");

            migrationBuilder.DropTable(
                name: "RolePermissions");

            migrationBuilder.DropTable(
                name: "SubHavalehIntermediatePlaces");

            migrationBuilder.DropTable(
                name: "TicketAttachments");

            migrationBuilder.DropTable(
                name: "TicketStatusHistories");

            migrationBuilder.DropTable(
                name: "TractorAddresses");

            migrationBuilder.DropTable(
                name: "TractorBankAccounts");

            migrationBuilder.DropTable(
                name: "TractorContacts");

            migrationBuilder.DropTable(
                name: "TransportAgreements");

            migrationBuilder.DropTable(
                name: "UnloadingDocuments");

            migrationBuilder.DropTable(
                name: "UserPermissions");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "TicketMessages");

            migrationBuilder.DropTable(
                name: "TractorAssignments");

            migrationBuilder.DropTable(
                name: "PermissionDefinitions");

            migrationBuilder.DropTable(
                name: "Tickets");

            migrationBuilder.DropTable(
                name: "DriverProfiles");

            migrationBuilder.DropTable(
                name: "SubHavalehs");

            migrationBuilder.DropTable(
                name: "Tractors");

            migrationBuilder.DropTable(
                name: "TicketCategories");

            migrationBuilder.DropTable(
                name: "Havalehs");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "LegalEntities");

            migrationBuilder.DropTable(
                name: "Places");

            migrationBuilder.DropTable(
                name: "Products");
        }
    }
}
