using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TMS.MVC.Migrations
{
    /// <inheritdoc />
    public partial class mig6 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Phone",
                table: "TractorContacts");

            migrationBuilder.DropColumn(
                name: "RegisteredPhone",
                table: "TractorContacts");

            migrationBuilder.DropColumn(
                name: "SmsNumber",
                table: "TractorContacts");

            migrationBuilder.DropColumn(
                name: "WhatsAppNumber",
                table: "TractorContacts");

            migrationBuilder.DropColumn(
                name: "CityName",
                table: "TractorAddresses");

            migrationBuilder.DropColumn(
                name: "Phone",
                table: "TractorAddresses");

            migrationBuilder.RenameColumn(
                name: "ConfirmedInSamava",
                table: "TractorContacts",
                newName: "IsPhone");

            migrationBuilder.AddColumn<string>(
                name: "ContactValue",
                table: "TractorContacts",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "HasSms",
                table: "TractorContacts",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "HasWhatsApp",
                table: "TractorContacts",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsEmail",
                table: "TractorContacts",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsFax",
                table: "TractorContacts",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ContactValue",
                table: "TractorContacts");

            migrationBuilder.DropColumn(
                name: "HasSms",
                table: "TractorContacts");

            migrationBuilder.DropColumn(
                name: "HasWhatsApp",
                table: "TractorContacts");

            migrationBuilder.DropColumn(
                name: "IsEmail",
                table: "TractorContacts");

            migrationBuilder.DropColumn(
                name: "IsFax",
                table: "TractorContacts");

            migrationBuilder.RenameColumn(
                name: "IsPhone",
                table: "TractorContacts",
                newName: "ConfirmedInSamava");

            migrationBuilder.AddColumn<string>(
                name: "Phone",
                table: "TractorContacts",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RegisteredPhone",
                table: "TractorContacts",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SmsNumber",
                table: "TractorContacts",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "WhatsAppNumber",
                table: "TractorContacts",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CityName",
                table: "TractorAddresses",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Phone",
                table: "TractorAddresses",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);
        }
    }
}
