using Microsoft.EntityFrameworkCore.Migrations;

namespace WebHooks.DAL.Migrations
{
    public partial class addColumnEventHookIdToHookTry : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HookTryes_EventHooks_EventHookId",
                table: "HookTryes");

            migrationBuilder.AlterColumn<string>(
                name: "HttpStatus",
                table: "HookTryes",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<long>(
                name: "EventHookId",
                table: "HookTryes",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_HookTryes_EventHooks_EventHookId",
                table: "HookTryes",
                column: "EventHookId",
                principalTable: "EventHooks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HookTryes_EventHooks_EventHookId",
                table: "HookTryes");

            migrationBuilder.AlterColumn<int>(
                name: "HttpStatus",
                table: "HookTryes",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "EventHookId",
                table: "HookTryes",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AddForeignKey(
                name: "FK_HookTryes_EventHooks_EventHookId",
                table: "HookTryes",
                column: "EventHookId",
                principalTable: "EventHooks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
