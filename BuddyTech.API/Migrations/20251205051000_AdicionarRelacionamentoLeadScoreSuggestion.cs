using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BuddyTech.API.Migrations
{
    /// <inheritdoc />
    public partial class AdicionarRelacionamentoLeadScoreSuggestion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Leads_LeadScores_CurrentScoreId",
                table: "Leads");

            migrationBuilder.DropForeignKey(
                name: "FK_Leads_Suggestions_SuggestionId",
                table: "Leads");

            migrationBuilder.DropIndex(
                name: "IX_LeadScores_LeadId",
                table: "LeadScores");

            migrationBuilder.DropIndex(
                name: "IX_Leads_CurrentScoreId",
                table: "Leads");

            migrationBuilder.DropIndex(
                name: "IX_Leads_SuggestionId",
                table: "Leads");

            migrationBuilder.DropColumn(
                name: "CurrentScoreId",
                table: "Leads");

            migrationBuilder.DropColumn(
                name: "SuggestionId",
                table: "Leads");

            migrationBuilder.AddColumn<Guid>(
                name: "LeadId",
                table: "Suggestions",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AlterColumn<Guid>(
                name: "LeadId",
                table: "LeadScores",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "LeadId1",
                table: "LeadScores",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Logo",
                table: "Companies",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Suggestions_LeadId",
                table: "Suggestions",
                column: "LeadId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_LeadScores_LeadId",
                table: "LeadScores",
                column: "LeadId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_LeadScores_LeadId1",
                table: "LeadScores",
                column: "LeadId1");

            migrationBuilder.AddForeignKey(
                name: "FK_LeadScores_Leads_LeadId1",
                table: "LeadScores",
                column: "LeadId1",
                principalTable: "Leads",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Suggestions_Leads_LeadId",
                table: "Suggestions",
                column: "LeadId",
                principalTable: "Leads",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LeadScores_Leads_LeadId1",
                table: "LeadScores");

            migrationBuilder.DropForeignKey(
                name: "FK_Suggestions_Leads_LeadId",
                table: "Suggestions");

            migrationBuilder.DropIndex(
                name: "IX_Suggestions_LeadId",
                table: "Suggestions");

            migrationBuilder.DropIndex(
                name: "IX_LeadScores_LeadId",
                table: "LeadScores");

            migrationBuilder.DropIndex(
                name: "IX_LeadScores_LeadId1",
                table: "LeadScores");

            migrationBuilder.DropColumn(
                name: "LeadId",
                table: "Suggestions");

            migrationBuilder.DropColumn(
                name: "LeadId1",
                table: "LeadScores");

            migrationBuilder.DropColumn(
                name: "Logo",
                table: "Companies");

            migrationBuilder.AlterColumn<Guid>(
                name: "LeadId",
                table: "LeadScores",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AddColumn<Guid>(
                name: "CurrentScoreId",
                table: "Leads",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "SuggestionId",
                table: "Leads",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_LeadScores_LeadId",
                table: "LeadScores",
                column: "LeadId");

            migrationBuilder.CreateIndex(
                name: "IX_Leads_CurrentScoreId",
                table: "Leads",
                column: "CurrentScoreId");

            migrationBuilder.CreateIndex(
                name: "IX_Leads_SuggestionId",
                table: "Leads",
                column: "SuggestionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Leads_LeadScores_CurrentScoreId",
                table: "Leads",
                column: "CurrentScoreId",
                principalTable: "LeadScores",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Leads_Suggestions_SuggestionId",
                table: "Leads",
                column: "SuggestionId",
                principalTable: "Suggestions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
