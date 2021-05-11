using Microsoft.EntityFrameworkCore.Migrations;

namespace WebHooks.DAL.Migrations
{
    public partial class updateSubscriptionsIndex : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Subscriptions_IsActual_ExternalAccountId_EventId",
                table: "Subscriptions");

            migrationBuilder.CreateIndex(
                name: "IX_Subscriptions_IsActual_AccountId_ExternalAccountId_EventId",
                table: "Subscriptions",
                columns: new[] { "IsActual", "AccountId", "ExternalAccountId", "EventId" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Subscriptions_IsActual_AccountId_ExternalAccountId_EventId",
                table: "Subscriptions");

            migrationBuilder.CreateIndex(
                name: "IX_Subscriptions_IsActual_ExternalAccountId_EventId",
                table: "Subscriptions",
                columns: new[] { "IsActual", "ExternalAccountId", "EventId" });
        }
    }
}
