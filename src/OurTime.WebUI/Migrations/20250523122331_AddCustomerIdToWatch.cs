using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OurTime.WebUI.Migrations
{
    /// <inheritdoc />
    public partial class AddCustomerIdToWatch : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CustomerId",
                table: "Watches",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CustomerId",
                table: "Watches");
        }
    }
}
