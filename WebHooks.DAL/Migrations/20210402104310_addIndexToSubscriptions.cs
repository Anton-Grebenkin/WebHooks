using Microsoft.EntityFrameworkCore.Migrations;

namespace WebHooks.DAL.Migrations
{
    public partial class addIndexToSubscriptions : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Subscriptions_IsActual_ExternalAccountId",
                table: "Subscriptions",
                columns: new[] { "IsActual", "ExternalAccountId" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Subscriptions_IsActual_ExternalAccountId",
                table: "Subscriptions");
        }
    }
}
