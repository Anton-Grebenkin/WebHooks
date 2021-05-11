using Microsoft.EntityFrameworkCore.Migrations;

namespace WebHooks.DAL.Migrations
{
    public partial class deleteColumnHookIdFromHookTry : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HookId",
                table: "HookTryes");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "HookId",
                table: "HookTryes",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);
        }
    }
}
