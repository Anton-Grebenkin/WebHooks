using Microsoft.EntityFrameworkCore.Migrations;

namespace WebHooks.DAL.Migrations
{
    public partial class addIndexToEventHooks : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_EventHooks_IsActual_SendTime_SubscriptionId",
                table: "EventHooks",
                columns: new[] { "IsActual", "SendTime", "SubscriptionId" });

            migrationBuilder.CreateIndex(
                name: "IX_EventHooks_IsActual_SubscriptionId_SendTime",
                table: "EventHooks",
                columns: new[] { "IsActual", "SubscriptionId", "SendTime" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_EventHooks_IsActual_SendTime_SubscriptionId",
                table: "EventHooks");

            migrationBuilder.DropIndex(
                name: "IX_EventHooks_IsActual_SubscriptionId_SendTime",
                table: "EventHooks");
        }
    }
}
