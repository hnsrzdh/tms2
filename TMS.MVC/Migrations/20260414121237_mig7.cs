using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TMS.MVC.Migrations
{
    /// <inheritdoc />
    public partial class mig7 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AccountType",
                table: "TractorBankAccounts");

            migrationBuilder.DropColumn(
                name: "DocumentIdentifier",
                table: "TractorBankAccounts");

            migrationBuilder.DropColumn(
                name: "OwnerDisplayName",
                table: "TractorBankAccounts");

            migrationBuilder.DropColumn(
                name: "VerificationCode",
                table: "TractorBankAccounts");

            migrationBuilder.RenameColumn(
                name: "OwnerMismatch",
                table: "TractorBankAccounts",
                newName: "IsDefault");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "TractorBankAccounts",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "TractorBankAccounts");

            migrationBuilder.RenameColumn(
                name: "IsDefault",
                table: "TractorBankAccounts",
                newName: "OwnerMismatch");

            migrationBuilder.AddColumn<string>(
                name: "AccountType",
                table: "TractorBankAccounts",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DocumentIdentifier",
                table: "TractorBankAccounts",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OwnerDisplayName",
                table: "TractorBankAccounts",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "VerificationCode",
                table: "TractorBankAccounts",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);
        }
    }
}
