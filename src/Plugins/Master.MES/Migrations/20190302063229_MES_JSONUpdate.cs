using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Master.Migrations
{
    public partial class MES_JSONUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MESHelps");

            

            migrationBuilder.AlterColumn<string>(
                name: "Property",
                table: "Tactic",
                type: "json",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Property",
                table: "RemindLog",
                type: "json",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            

            migrationBuilder.AlterColumn<string>(
                name: "Property",
                table: "ProcessType",
                type: "json",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Property",
                table: "ProcessTaskReport",
                type: "json",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Property",
                table: "ProcessTask",
                type: "json",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Property",
                table: "ProcessQuoteDetail",
                type: "json",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Property",
                table: "ProcessQuote",
                type: "json",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Property",
                table: "Person",
                type: "json",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Property",
                table: "Part",
                type: "json",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Property",
                table: "Organization",
                type: "json",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            

            migrationBuilder.AlterColumn<string>(
                name: "Property",
                table: "Equipment",
                type: "json",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Property",
                table: "User",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "json",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Property",
                table: "Unit",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "json",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Property",
                table: "Tenant",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "json",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Property",
                table: "Tactic",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "json",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Property",
                table: "RemindLog",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "json",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Property",
                table: "Project",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "json",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Property",
                table: "ProcessType",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "json",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Property",
                table: "ProcessTaskReport",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "json",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Property",
                table: "ProcessTask",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "json",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Property",
                table: "ProcessQuoteDetail",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "json",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Property",
                table: "ProcessQuote",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "json",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Property",
                table: "Person",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "json",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Property",
                table: "Part",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "json",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Property",
                table: "Organization",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "json",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Property",
                table: "Order",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "json",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Property",
                table: "Equipment",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "json",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Property",
                table: "BaseTree",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "json",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "MESHelps",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    DeleterUserId = table.Column<long>(nullable: true),
                    DeletionTime = table.Column<DateTime>(nullable: true),
                    ExtensionData = table.Column<string>(nullable: true),
                    HelpContent = table.Column<string>(nullable: true),
                    HelpTitle = table.Column<string>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierUserId = table.Column<long>(nullable: true),
                    MenuDisplayName = table.Column<string>(nullable: true),
                    MenuName = table.Column<string>(nullable: true),
                    Property = table.Column<string>(nullable: true),
                    Remarks = table.Column<string>(nullable: true),
                    TenantId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MESHelps", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MESHelps_User_CreatorUserId",
                        column: x => x.CreatorUserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MESHelps_User_DeleterUserId",
                        column: x => x.DeleterUserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MESHelps_User_LastModifierUserId",
                        column: x => x.LastModifierUserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MESHelps_Tenant_TenantId",
                        column: x => x.TenantId,
                        principalTable: "Tenant",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MESHelps_CreatorUserId",
                table: "MESHelps",
                column: "CreatorUserId");

            migrationBuilder.CreateIndex(
                name: "IX_MESHelps_DeleterUserId",
                table: "MESHelps",
                column: "DeleterUserId");

            migrationBuilder.CreateIndex(
                name: "IX_MESHelps_LastModifierUserId",
                table: "MESHelps",
                column: "LastModifierUserId");

            migrationBuilder.CreateIndex(
                name: "IX_MESHelps_TenantId",
                table: "MESHelps",
                column: "TenantId");
        }
    }
}
