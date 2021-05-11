using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WebHooks.DAL.Migrations
{
    public partial class addIsActiveColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DeactualizeTime",
                table: "Subscriptions",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsActual",
                table: "Subscriptions",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeactualizeTime",
                table: "HookTryes",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsActual",
                table: "HookTryes",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeactualizeTime",
                table: "EventHooks",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsActual",
                table: "EventHooks",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeactualizeTime",
                table: "Accounts",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsActual",
                table: "Accounts",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DeactualizeTime",
                table: "Subscriptions");

            migrationBuilder.DropColumn(
                name: "IsActual",
                table: "Subscriptions");

            migrationBuilder.DropColumn(
                name: "DeactualizeTime",
                table: "HookTryes");

            migrationBuilder.DropColumn(
                name: "IsActual",
                table: "HookTryes");

            migrationBuilder.DropColumn(
                name: "DeactualizeTime",
                table: "EventHooks");

            migrationBuilder.DropColumn(
                name: "IsActual",
                table: "EventHooks");

            migrationBuilder.DropColumn(
                name: "DeactualizeTime",
                table: "Accounts");

            migrationBuilder.DropColumn(
                name: "IsActual",
                table: "Accounts");
        }
    }
}
