using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Master.Migrations
{
    public partial class MES_EquipmentOperator : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "OperatorId",
                table: "Equipment",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "EquipmentOperatorHistory",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierUserId = table.Column<long>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeleterUserId = table.Column<long>(nullable: true),
                    DeletionTime = table.Column<DateTime>(nullable: true),
                    ExtensionData = table.Column<string>(nullable: true),
                    Remarks = table.Column<string>(nullable: true),
                    Property = table.Column<string>(type: "json", nullable: true),
                    TenantId = table.Column<int>(nullable: false),
                    EquipmentId = table.Column<int>(nullable: false),
                    OperatorId = table.Column<long>(nullable: false),
                    EquipmentTransitionType = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EquipmentOperatorHistory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EquipmentOperatorHistory_User_CreatorUserId",
                        column: x => x.CreatorUserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EquipmentOperatorHistory_User_DeleterUserId",
                        column: x => x.DeleterUserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EquipmentOperatorHistory_Equipment_EquipmentId",
                        column: x => x.EquipmentId,
                        principalTable: "Equipment",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EquipmentOperatorHistory_User_LastModifierUserId",
                        column: x => x.LastModifierUserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EquipmentOperatorHistory_User_OperatorId",
                        column: x => x.OperatorId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EquipmentOperatorHistory_Tenant_TenantId",
                        column: x => x.TenantId,
                        principalTable: "Tenant",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Equipment_OperatorId",
                table: "Equipment",
                column: "OperatorId");

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentOperatorHistory_CreatorUserId",
                table: "EquipmentOperatorHistory",
                column: "CreatorUserId");

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentOperatorHistory_DeleterUserId",
                table: "EquipmentOperatorHistory",
                column: "DeleterUserId");

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentOperatorHistory_EquipmentId",
                table: "EquipmentOperatorHistory",
                column: "EquipmentId");

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentOperatorHistory_LastModifierUserId",
                table: "EquipmentOperatorHistory",
                column: "LastModifierUserId");

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentOperatorHistory_OperatorId",
                table: "EquipmentOperatorHistory",
                column: "OperatorId");

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentOperatorHistory_TenantId",
                table: "EquipmentOperatorHistory",
                column: "TenantId");

            migrationBuilder.AddForeignKey(
                name: "FK_Equipment_User_OperatorId",
                table: "Equipment",
                column: "OperatorId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Equipment_User_OperatorId",
                table: "Equipment");

            migrationBuilder.DropTable(
                name: "EquipmentOperatorHistory");

            migrationBuilder.DropIndex(
                name: "IX_Equipment_OperatorId",
                table: "Equipment");

            migrationBuilder.DropColumn(
                name: "OperatorId",
                table: "Equipment");
        }
    }
}
