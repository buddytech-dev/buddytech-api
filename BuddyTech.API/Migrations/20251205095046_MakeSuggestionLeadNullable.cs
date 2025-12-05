using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BuddyTech.API.Migrations
{
    /// <inheritdoc />
    public partial class MakeSuggestionLeadNullable : Migration
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

            migrationBuilder.AlterColumn<Guid>(
                name: "LeadId",
                table: "Suggestions",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

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
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

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

            migrationBuilder.AlterColumn<Guid>(
                name: "LeadId",
                table: "Suggestions",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

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
    }
}
