using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BuddyTech.API.Migrations
{
    /// <inheritdoc />
    public partial class AdicionarRelacionamentoSuggestion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LeadInteractions_Leads_LeadId",
                table: "LeadInteractions");

            migrationBuilder.DropForeignKey(
                name: "FK_Leads_Companies_CompanyId",
                table: "Leads");

            migrationBuilder.DropForeignKey(
                name: "FK_Leads_Sellers_SellerId",
                table: "Leads");

            migrationBuilder.DropForeignKey(
                name: "FK_LeadScores_Leads_LeadId",
                table: "LeadScores");

            migrationBuilder.DropForeignKey(
                name: "FK_LeadScores_Leads_LeadId1",
                table: "LeadScores");

            migrationBuilder.DropForeignKey(
                name: "FK_SellerMissions_Missions_MissionId",
                table: "SellerMissions");

            migrationBuilder.DropForeignKey(
                name: "FK_SellerMissions_Sellers_SellerId",
                table: "SellerMissions");

            migrationBuilder.DropForeignKey(
                name: "FK_Suggestions_LeadInteractions_InteractionSuggestedId",
                table: "Suggestions");

            migrationBuilder.DropForeignKey(
                name: "FK_Suggestions_Leads_LeadId",
                table: "Suggestions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SellerMissions",
                table: "SellerMissions");

            migrationBuilder.DropIndex(
                name: "IX_SellerMissions_SellerId",
                table: "SellerMissions");

            migrationBuilder.DropIndex(
                name: "IX_LeadScores_LeadId",
                table: "LeadScores");

            migrationBuilder.DropIndex(
                name: "IX_LeadScores_LeadId1",
                table: "LeadScores");

            migrationBuilder.DropColumn(
                name: "LeadId1",
                table: "LeadScores");

            migrationBuilder.AddColumn<Guid>(
                name: "SuggestionId",
                table: "Leads",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_SellerMissions",
                table: "SellerMissions",
                columns: new[] { "SellerId", "MissionId" });

            migrationBuilder.CreateIndex(
                name: "IX_LeadScores_LeadId",
                table: "LeadScores",
                column: "LeadId");

            migrationBuilder.CreateIndex(
                name: "IX_Leads_CurrentScoreId",
                table: "Leads",
                column: "CurrentScoreId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_LeadInteractions_Leads_LeadId",
                table: "LeadInteractions",
                column: "LeadId",
                principalTable: "Leads",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Leads_Companies_CompanyId",
                table: "Leads",
                column: "CompanyId",
                principalTable: "Companies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Leads_LeadScores_CurrentScoreId",
                table: "Leads",
                column: "CurrentScoreId",
                principalTable: "LeadScores",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Leads_Sellers_SellerId",
                table: "Leads",
                column: "SellerId",
                principalTable: "Sellers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_LeadScores_Leads_LeadId",
                table: "LeadScores",
                column: "LeadId",
                principalTable: "Leads",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SellerMissions_Missions_MissionId",
                table: "SellerMissions",
                column: "MissionId",
                principalTable: "Missions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SellerMissions_Sellers_SellerId",
                table: "SellerMissions",
                column: "SellerId",
                principalTable: "Sellers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Suggestions_LeadInteractions_InteractionSuggestedId",
                table: "Suggestions",
                column: "InteractionSuggestedId",
                principalTable: "LeadInteractions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Suggestions_Leads_LeadId",
                table: "Suggestions",
                column: "LeadId",
                principalTable: "Leads",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LeadInteractions_Leads_LeadId",
                table: "LeadInteractions");

            migrationBuilder.DropForeignKey(
                name: "FK_Leads_Companies_CompanyId",
                table: "Leads");

            migrationBuilder.DropForeignKey(
                name: "FK_Leads_LeadScores_CurrentScoreId",
                table: "Leads");

            migrationBuilder.DropForeignKey(
                name: "FK_Leads_Sellers_SellerId",
                table: "Leads");

            migrationBuilder.DropForeignKey(
                name: "FK_LeadScores_Leads_LeadId",
                table: "LeadScores");

            migrationBuilder.DropForeignKey(
                name: "FK_SellerMissions_Missions_MissionId",
                table: "SellerMissions");

            migrationBuilder.DropForeignKey(
                name: "FK_SellerMissions_Sellers_SellerId",
                table: "SellerMissions");

            migrationBuilder.DropForeignKey(
                name: "FK_Suggestions_LeadInteractions_InteractionSuggestedId",
                table: "Suggestions");

            migrationBuilder.DropForeignKey(
                name: "FK_Suggestions_Leads_LeadId",
                table: "Suggestions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SellerMissions",
                table: "SellerMissions");

            migrationBuilder.DropIndex(
                name: "IX_LeadScores_LeadId",
                table: "LeadScores");

            migrationBuilder.DropIndex(
                name: "IX_Leads_CurrentScoreId",
                table: "Leads");

            migrationBuilder.DropColumn(
                name: "SuggestionId",
                table: "Leads");

            migrationBuilder.AddColumn<Guid>(
                name: "LeadId1",
                table: "LeadScores",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_SellerMissions",
                table: "SellerMissions",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_SellerMissions_SellerId",
                table: "SellerMissions",
                column: "SellerId");

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
                name: "FK_LeadInteractions_Leads_LeadId",
                table: "LeadInteractions",
                column: "LeadId",
                principalTable: "Leads",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Leads_Companies_CompanyId",
                table: "Leads",
                column: "CompanyId",
                principalTable: "Companies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Leads_Sellers_SellerId",
                table: "Leads",
                column: "SellerId",
                principalTable: "Sellers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_LeadScores_Leads_LeadId",
                table: "LeadScores",
                column: "LeadId",
                principalTable: "Leads",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_LeadScores_Leads_LeadId1",
                table: "LeadScores",
                column: "LeadId1",
                principalTable: "Leads",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SellerMissions_Missions_MissionId",
                table: "SellerMissions",
                column: "MissionId",
                principalTable: "Missions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SellerMissions_Sellers_SellerId",
                table: "SellerMissions",
                column: "SellerId",
                principalTable: "Sellers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Suggestions_LeadInteractions_InteractionSuggestedId",
                table: "Suggestions",
                column: "InteractionSuggestedId",
                principalTable: "LeadInteractions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Suggestions_Leads_LeadId",
                table: "Suggestions",
                column: "LeadId",
                principalTable: "Leads",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
