using Microsoft.EntityFrameworkCore.Migrations;

namespace Master.Migrations
{
    public partial class NewMinerUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_NewMiner_Tenant_TenantId",
                table: "NewMiner");

            migrationBuilder.DropIndex(
                name: "IX_NewMiner_TenantId",
                table: "NewMiner");

            migrationBuilder.DropColumn(
                name: "TenantId",
                table: "NewMiner");

            migrationBuilder.AddColumn<string>(
                name: "PhoneNumber",
                table: "NewMiner",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PhoneNumber",
                table: "NewMiner");

            migrationBuilder.AddColumn<int>(
                name: "TenantId",
                table: "NewMiner",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_NewMiner_TenantId",
                table: "NewMiner",
                column: "TenantId");

            migrationBuilder.AddForeignKey(
                name: "FK_NewMiner_Tenant_TenantId",
                table: "NewMiner",
                column: "TenantId",
                principalTable: "Tenant",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
