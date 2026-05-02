using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TMS.MVC.Migrations
{
    /// <inheritdoc />
    public partial class mig3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "ChargeableShortageAmount",
                table: "TractorAssignments",
                type: "decimal(18,3)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "DelayPenaltyExchangeRateToBase",
                table: "TractorAssignments",
                type: "decimal(18,6)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DeliveryDelayDays",
                table: "TractorAssignments",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "DriverPriceExchangeRateToBase",
                table: "TractorAssignments",
                type: "decimal(18,6)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "DriverStopFeeExchangeRateToBase",
                table: "TractorAssignments",
                type: "decimal(18,6)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "DriverTipExchangeRateToBase",
                table: "TractorAssignments",
                type: "decimal(18,6)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FinancialAdjustmentNote",
                table: "TractorAssignments",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "FinancialApprovedAmount",
                table: "TractorAssignments",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FinancialApprovedBy",
                table: "TractorAssignments",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "FinancialApprovedDate",
                table: "TractorAssignments",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FinancialBaseCurrency",
                table: "TractorAssignments",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "FinancialCalculatedAmount",
                table: "TractorAssignments",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "FinancialManualAmount",
                table: "TractorAssignments",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsFinancialApproved",
                table: "TractorAssignments",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "LoadingDelayDays",
                table: "TractorAssignments",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "ShortagePenaltyExchangeRateToBase",
                table: "TractorAssignments",
                type: "decimal(18,6)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TotalDelayDays",
                table: "TractorAssignments",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ChargeableShortageAmount",
                table: "TractorAssignments");

            migrationBuilder.DropColumn(
                name: "DelayPenaltyExchangeRateToBase",
                table: "TractorAssignments");

            migrationBuilder.DropColumn(
                name: "DeliveryDelayDays",
                table: "TractorAssignments");

            migrationBuilder.DropColumn(
                name: "DriverPriceExchangeRateToBase",
                table: "TractorAssignments");

            migrationBuilder.DropColumn(
                name: "DriverStopFeeExchangeRateToBase",
                table: "TractorAssignments");

            migrationBuilder.DropColumn(
                name: "DriverTipExchangeRateToBase",
                table: "TractorAssignments");

            migrationBuilder.DropColumn(
                name: "FinancialAdjustmentNote",
                table: "TractorAssignments");

            migrationBuilder.DropColumn(
                name: "FinancialApprovedAmount",
                table: "TractorAssignments");

            migrationBuilder.DropColumn(
                name: "FinancialApprovedBy",
                table: "TractorAssignments");

            migrationBuilder.DropColumn(
                name: "FinancialApprovedDate",
                table: "TractorAssignments");

            migrationBuilder.DropColumn(
                name: "FinancialBaseCurrency",
                table: "TractorAssignments");

            migrationBuilder.DropColumn(
                name: "FinancialCalculatedAmount",
                table: "TractorAssignments");

            migrationBuilder.DropColumn(
                name: "FinancialManualAmount",
                table: "TractorAssignments");

            migrationBuilder.DropColumn(
                name: "IsFinancialApproved",
                table: "TractorAssignments");

            migrationBuilder.DropColumn(
                name: "LoadingDelayDays",
                table: "TractorAssignments");

            migrationBuilder.DropColumn(
                name: "ShortagePenaltyExchangeRateToBase",
                table: "TractorAssignments");

            migrationBuilder.DropColumn(
                name: "TotalDelayDays",
                table: "TractorAssignments");
        }
    }
}
