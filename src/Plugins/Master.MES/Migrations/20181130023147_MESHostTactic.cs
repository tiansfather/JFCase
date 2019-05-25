using Microsoft.EntityFrameworkCore.Migrations;

namespace Master.Migrations
{
    public partial class MESHostTactic : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RemindLog_Tenant_TenantId",
                table: "RemindLog");

            migrationBuilder.DropForeignKey(
                name: "FK_Tactic_Tenant_TenantId",
                table: "Tactic");

            

            migrationBuilder.AlterColumn<int>(
                name: "TenantId",
                table: "Tactic",
                nullable: true,
                oldClrType: typeof(int));
           

            migrationBuilder.AlterColumn<int>(
                name: "TenantId",
                table: "RemindLog",
                nullable: true,
                oldClrType: typeof(int));

            

            migrationBuilder.AddForeignKey(
                name: "FK_RemindLog_Tenant_TenantId",
                table: "RemindLog",
                column: "TenantId",
                principalTable: "Tenant",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Tactic_Tenant_TenantId",
                table: "Tactic",
                column: "TenantId",
                principalTable: "Tenant",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RemindLog_Tenant_TenantId",
                table: "RemindLog");

            migrationBuilder.DropForeignKey(
                name: "FK_Tactic_Tenant_TenantId",
                table: "Tactic");

            

            migrationBuilder.AlterColumn<int>(
                name: "TenantId",
                table: "Tactic",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

           

            migrationBuilder.AlterColumn<int>(
                name: "TenantId",
                table: "RemindLog",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

           


            migrationBuilder.AddForeignKey(
                name: "FK_RemindLog_Tenant_TenantId",
                table: "RemindLog",
                column: "TenantId",
                principalTable: "Tenant",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Tactic_Tenant_TenantId",
                table: "Tactic",
                column: "TenantId",
                principalTable: "Tenant",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
