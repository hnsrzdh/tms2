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
            migrationBuilder.DropColumn(
                name: "OwnerName",
                table: "Tractors");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "OwnerName",
                table: "Tractors",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);
        }
    }
}
