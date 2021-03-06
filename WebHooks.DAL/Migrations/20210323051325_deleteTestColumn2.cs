using Microsoft.EntityFrameworkCore.Migrations;

namespace WebHooks.DAL.Migrations
{
    public partial class deleteTestColumn2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Test",
                table: "Accounts");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Test",
                table: "Accounts",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
