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
            migrationBuilder.AddColumn<string>(
                name: "AccountNumber",
                table: "LegalEntityBankAccounts",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AccountOwnerName",
                table: "LegalEntityBankAccounts",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BankName",
                table: "LegalEntityBankAccounts",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CardNumber",
                table: "LegalEntityBankAccounts",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "LegalEntityBankAccounts",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDefault",
                table: "LegalEntityBankAccounts",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "ShebaNumber",
                table: "LegalEntityBankAccounts",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AccountNumber",
                table: "LegalEntityBankAccounts");

            migrationBuilder.DropColumn(
                name: "AccountOwnerName",
                table: "LegalEntityBankAccounts");

            migrationBuilder.DropColumn(
                name: "BankName",
                table: "LegalEntityBankAccounts");

            migrationBuilder.DropColumn(
                name: "CardNumber",
                table: "LegalEntityBankAccounts");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "LegalEntityBankAccounts");

            migrationBuilder.DropColumn(
                name: "IsDefault",
                table: "LegalEntityBankAccounts");

            migrationBuilder.DropColumn(
                name: "ShebaNumber",
                table: "LegalEntityBankAccounts");
        }
    }
}
