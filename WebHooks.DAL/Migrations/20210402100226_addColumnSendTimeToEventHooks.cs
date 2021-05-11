using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WebHooks.DAL.Migrations
{
    public partial class addColumnSendTimeToEventHooks : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "SendTime",
                table: "EventHooks",
                type: "datetime2",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SendTime",
                table: "EventHooks");
        }
    }
}
