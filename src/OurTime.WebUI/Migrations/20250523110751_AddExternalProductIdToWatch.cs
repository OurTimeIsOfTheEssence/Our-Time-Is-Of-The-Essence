using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OurTime.WebUI.Migrations
{
    /// <inheritdoc />
    public partial class AddExternalProductIdToWatch : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "ExternalProductId",
                table: "Watches",
                type: "bigint",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExternalProductId",
                table: "Watches");
        }

    }
}
