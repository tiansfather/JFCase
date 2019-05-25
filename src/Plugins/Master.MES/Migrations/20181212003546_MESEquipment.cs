using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Master.Migrations
{
    public partial class MESEquipment : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            

            migrationBuilder.CreateTable(
                name: "Equipment",
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
                    Property = table.Column<string>(nullable: true),
                    TenantId = table.Column<int>(nullable: false),
                    UnitId = table.Column<int>(nullable: true),
                    Status = table.Column<string>(nullable: true),
                    EquipmentSN = table.Column<string>(nullable: true),
                    Brand = table.Column<string>(nullable: true),
                    Range = table.Column<string>(nullable: true),
                    Price = table.Column<decimal>(nullable: true),
                    BuyYear = table.Column<int>(nullable: true),
                    BuyCost = table.Column<decimal>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Equipment", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Equipment_User_CreatorUserId",
                        column: x => x.CreatorUserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Equipment_User_DeleterUserId",
                        column: x => x.DeleterUserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Equipment_User_LastModifierUserId",
                        column: x => x.LastModifierUserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Equipment_Tenant_TenantId",
                        column: x => x.TenantId,
                        principalTable: "Tenant",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Equipment_Unit_UnitId",
                        column: x => x.UnitId,
                        principalTable: "Unit",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "EquipmentProcessType",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    EquipmentId = table.Column<int>(nullable: false),
                    ProcessTypeId = table.Column<int>(nullable: false),
                    TenantId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EquipmentProcessType", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EquipmentProcessType_Equipment_EquipmentId",
                        column: x => x.EquipmentId,
                        principalTable: "Equipment",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EquipmentProcessType_ProcessType_ProcessTypeId",
                        column: x => x.ProcessTypeId,
                        principalTable: "ProcessType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EquipmentProcessType_Tenant_TenantId",
                        column: x => x.TenantId,
                        principalTable: "Tenant",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Equipment_CreatorUserId",
                table: "Equipment",
                column: "CreatorUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Equipment_DeleterUserId",
                table: "Equipment",
                column: "DeleterUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Equipment_LastModifierUserId",
                table: "Equipment",
                column: "LastModifierUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Equipment_TenantId",
                table: "Equipment",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_Equipment_UnitId",
                table: "Equipment",
                column: "UnitId");

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentProcessType_EquipmentId",
                table: "EquipmentProcessType",
                column: "EquipmentId");

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentProcessType_ProcessTypeId",
                table: "EquipmentProcessType",
                column: "ProcessTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentProcessType_TenantId",
                table: "EquipmentProcessType",
                column: "TenantId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EquipmentProcessType");

            migrationBuilder.DropTable(
                name: "Equipment");

           
        }
    }
}
