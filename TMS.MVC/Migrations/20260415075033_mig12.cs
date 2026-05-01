using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TMS.MVC.Migrations
{
    /// <inheritdoc />
    public partial class mig12 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
                    DestinationCityId = table.Column<long>(type: "bigint", nullable: true),
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
                        name: "FK_SubHavalehs_Cities_DestinationCityId",
                        column: x => x.DestinationCityId,
                        principalTable: "Cities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SubHavalehs_Havalehs_HavalehId",
                        column: x => x.HavalehId,
                        principalTable: "Havalehs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SubHavalehIntermediateCities",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SubHavalehId = table.Column<long>(type: "bigint", nullable: false),
                    CityId = table.Column<long>(type: "bigint", nullable: false),
                    SortOrder = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubHavalehIntermediateCities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SubHavalehIntermediateCities_Cities_CityId",
                        column: x => x.CityId,
                        principalTable: "Cities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SubHavalehIntermediateCities_SubHavalehs_SubHavalehId",
                        column: x => x.SubHavalehId,
                        principalTable: "SubHavalehs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SubHavalehIntermediateCities_CityId",
                table: "SubHavalehIntermediateCities",
                column: "CityId");

            migrationBuilder.CreateIndex(
                name: "IX_SubHavalehIntermediateCities_SubHavalehId",
                table: "SubHavalehIntermediateCities",
                column: "SubHavalehId");

            migrationBuilder.CreateIndex(
                name: "IX_SubHavalehs_DestinationCityId",
                table: "SubHavalehs",
                column: "DestinationCityId");

            migrationBuilder.CreateIndex(
                name: "IX_SubHavalehs_HavalehId",
                table: "SubHavalehs",
                column: "HavalehId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SubHavalehIntermediateCities");

            migrationBuilder.DropTable(
                name: "SubHavalehs");
        }
    }
}
