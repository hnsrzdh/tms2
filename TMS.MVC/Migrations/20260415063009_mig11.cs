using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TMS.MVC.Migrations
{
    /// <inheritdoc />
    public partial class mig11 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
                    OriginCityId = table.Column<long>(type: "bigint", nullable: true),
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
                        name: "FK_Havalehs_Cities_OriginCityId",
                        column: x => x.OriginCityId,
                        principalTable: "Cities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
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
                        name: "FK_Havalehs_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

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
                name: "IX_Havalehs_OriginCityId",
                table: "Havalehs",
                column: "OriginCityId");

            migrationBuilder.CreateIndex(
                name: "IX_Havalehs_ProductId",
                table: "Havalehs",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Havalehs_TransportContractorLegalEntityId",
                table: "Havalehs",
                column: "TransportContractorLegalEntityId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Havalehs");
        }
    }
}
