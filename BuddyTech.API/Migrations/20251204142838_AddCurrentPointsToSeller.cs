using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BuddyTech.API.Migrations
{
    /// <inheritdoc />
    public partial class AddCurrentPointsToSeller : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CurrentPoints",
                table: "Sellers",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CurrentPoints",
                table: "Sellers");
        }
    }
}
