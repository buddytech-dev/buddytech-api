using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BuddyTech.API.Migrations
{
    /// <inheritdoc />
    public partial class AdicionarRelacionamentoLeadScore : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "CurrentScoreId",
                table: "Leads",
                type: "uuid",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CurrentScoreId",
                table: "Leads");
        }
    }
}
