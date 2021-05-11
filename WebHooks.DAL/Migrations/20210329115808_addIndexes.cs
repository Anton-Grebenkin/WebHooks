using Microsoft.EntityFrameworkCore.Migrations;

namespace WebHooks.DAL.Migrations
{
    public partial class addIndexes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Subscriptions_IsActual_ExternalAccountId_EventId",
                table: "Subscriptions",
                columns: new[] { "IsActual", "ExternalAccountId", "EventId" });

            migrationBuilder.CreateIndex(
                name: "IX_EventHooks_IsActual_Status_TryCount_SubscriptionId",
                table: "EventHooks",
                columns: new[] { "IsActual", "Status", "TryCount", "SubscriptionId" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Subscriptions_IsActual_ExternalAccountId_EventId",
                table: "Subscriptions");

            migrationBuilder.DropIndex(
                name: "IX_EventHooks_IsActual_Status_TryCount_SubscriptionId",
                table: "EventHooks");
        }
    }
}
