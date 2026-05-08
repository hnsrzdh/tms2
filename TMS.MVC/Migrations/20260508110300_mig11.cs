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
            migrationBuilder.AddColumn<string>(
                name: "EmailAddress",
                table: "TractorContacts",
                type: "nvarchar(300)",
                maxLength: 300,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FaxNumber",
                table: "TractorContacts",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PhoneNumber",
                table: "TractorContacts",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SmsNumber",
                table: "TractorContacts",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "WhatsAppNumber",
                table: "TractorContacts",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EmailAddress",
                table: "LegalEntityContacts",
                type: "nvarchar(300)",
                maxLength: 300,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FaxNumber",
                table: "LegalEntityContacts",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PhoneNumber",
                table: "LegalEntityContacts",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SmsNumber",
                table: "LegalEntityContacts",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "WhatsAppNumber",
                table: "LegalEntityContacts",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EmailAddress",
                table: "DriverContacts",
                type: "nvarchar(300)",
                maxLength: 300,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FaxNumber",
                table: "DriverContacts",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PhoneNumber",
                table: "DriverContacts",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SmsNumber",
                table: "DriverContacts",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "WhatsAppNumber",
                table: "DriverContacts",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EmailAddress",
                table: "TractorContacts");

            migrationBuilder.DropColumn(
                name: "FaxNumber",
                table: "TractorContacts");

            migrationBuilder.DropColumn(
                name: "PhoneNumber",
                table: "TractorContacts");

            migrationBuilder.DropColumn(
                name: "SmsNumber",
                table: "TractorContacts");

            migrationBuilder.DropColumn(
                name: "WhatsAppNumber",
                table: "TractorContacts");

            migrationBuilder.DropColumn(
                name: "EmailAddress",
                table: "LegalEntityContacts");

            migrationBuilder.DropColumn(
                name: "FaxNumber",
                table: "LegalEntityContacts");

            migrationBuilder.DropColumn(
                name: "PhoneNumber",
                table: "LegalEntityContacts");

            migrationBuilder.DropColumn(
                name: "SmsNumber",
                table: "LegalEntityContacts");

            migrationBuilder.DropColumn(
                name: "WhatsAppNumber",
                table: "LegalEntityContacts");

            migrationBuilder.DropColumn(
                name: "EmailAddress",
                table: "DriverContacts");

            migrationBuilder.DropColumn(
                name: "FaxNumber",
                table: "DriverContacts");

            migrationBuilder.DropColumn(
                name: "PhoneNumber",
                table: "DriverContacts");

            migrationBuilder.DropColumn(
                name: "SmsNumber",
                table: "DriverContacts");

            migrationBuilder.DropColumn(
                name: "WhatsAppNumber",
                table: "DriverContacts");
        }
    }
}
