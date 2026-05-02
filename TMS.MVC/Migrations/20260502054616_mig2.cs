using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TMS.MVC.Migrations
{
    /// <inheritdoc />
    public partial class mig2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DriverCurrencyRate",
                table: "SubHavalehs");

            migrationBuilder.DropColumn(
                name: "DriverCurrencyType",
                table: "SubHavalehs");

            migrationBuilder.DropColumn(
                name: "DriverPricePerTon",
                table: "SubHavalehs");

            migrationBuilder.DropColumn(
                name: "GoodsOwnerCurrencyType",
                table: "SubHavalehs");

            migrationBuilder.RenameColumn(
                name: "GoodsOwnerPricePerTon",
                table: "SubHavalehs",
                newName: "GoodsOwnerPricePer1000Unit");

            migrationBuilder.RenameColumn(
                name: "GoodsOwnerCurrencyRate",
                table: "SubHavalehs",
                newName: "DriverPricePer1000Unit");

            migrationBuilder.AddColumn<string>(
                name: "DriverPriceCurrency",
                table: "SubHavalehs",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DriverStopFeeCurrency",
                table: "SubHavalehs",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DriverTipCurrency",
                table: "SubHavalehs",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "GoodsOwnerPriceCurrency",
                table: "SubHavalehs",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "GoodsOwnerStopFeeCurrency",
                table: "SubHavalehs",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "GoodsOwnerTipCurrency",
                table: "SubHavalehs",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LateDeliveryPenaltyCurrency",
                table: "SubHavalehs",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DriverPriceCurrency",
                table: "SubHavalehs");

            migrationBuilder.DropColumn(
                name: "DriverStopFeeCurrency",
                table: "SubHavalehs");

            migrationBuilder.DropColumn(
                name: "DriverTipCurrency",
                table: "SubHavalehs");

            migrationBuilder.DropColumn(
                name: "GoodsOwnerPriceCurrency",
                table: "SubHavalehs");

            migrationBuilder.DropColumn(
                name: "GoodsOwnerStopFeeCurrency",
                table: "SubHavalehs");

            migrationBuilder.DropColumn(
                name: "GoodsOwnerTipCurrency",
                table: "SubHavalehs");

            migrationBuilder.DropColumn(
                name: "LateDeliveryPenaltyCurrency",
                table: "SubHavalehs");

            migrationBuilder.RenameColumn(
                name: "GoodsOwnerPricePer1000Unit",
                table: "SubHavalehs",
                newName: "GoodsOwnerPricePerTon");

            migrationBuilder.RenameColumn(
                name: "DriverPricePer1000Unit",
                table: "SubHavalehs",
                newName: "GoodsOwnerCurrencyRate");

            migrationBuilder.AddColumn<decimal>(
                name: "DriverCurrencyRate",
                table: "SubHavalehs",
                type: "decimal(18,4)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DriverCurrencyType",
                table: "SubHavalehs",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "DriverPricePerTon",
                table: "SubHavalehs",
                type: "decimal(18,4)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "GoodsOwnerCurrencyType",
                table: "SubHavalehs",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);
        }
    }
}
