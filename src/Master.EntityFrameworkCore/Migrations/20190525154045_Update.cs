using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Master.Migrations
{
    public partial class Update : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CaseInitial_BaseTree_JiuFenLeiXingId",
                table: "CaseInitial");

            migrationBuilder.DropForeignKey(
                name: "FK_CaseInitial_BaseTree_JiuFenYuanYinId",
                table: "CaseInitial");

            migrationBuilder.DropForeignKey(
                name: "FK_CaseInitial_BaseTree_PanJueJieGuoId",
                table: "CaseInitial");

            migrationBuilder.DropForeignKey(
                name: "FK_CaseInitial_BaseTree_ZhuanTiId",
                table: "CaseInitial");

            migrationBuilder.DropForeignKey(
                name: "FK_CaseSource_User_OwerId1",
                table: "CaseSource");

            migrationBuilder.DropIndex(
                name: "IX_CaseSource_OwerId1",
                table: "CaseSource");

            migrationBuilder.DropIndex(
                name: "IX_CaseInitial_JiuFenLeiXingId",
                table: "CaseInitial");

            migrationBuilder.DropIndex(
                name: "IX_CaseInitial_JiuFenYuanYinId",
                table: "CaseInitial");

            migrationBuilder.DropIndex(
                name: "IX_CaseInitial_PanJueJieGuoId",
                table: "CaseInitial");

            migrationBuilder.DropIndex(
                name: "IX_CaseInitial_ZhuanTiId",
                table: "CaseInitial");

            migrationBuilder.DropColumn(
                name: "OwerId1",
                table: "CaseSource");

            migrationBuilder.DropColumn(
                name: "JiuFenLeiXingId",
                table: "CaseInitial");

            migrationBuilder.DropColumn(
                name: "JiuFenYuanYinId",
                table: "CaseInitial");

            migrationBuilder.DropColumn(
                name: "Keywords",
                table: "CaseInitial");

            migrationBuilder.DropColumn(
                name: "Labels",
                table: "CaseInitial");

            migrationBuilder.DropColumn(
                name: "PanJueJieGuoId",
                table: "CaseInitial");

            migrationBuilder.DropColumn(
                name: "ZhuanTiId",
                table: "CaseInitial");

            migrationBuilder.AlterColumn<long>(
                name: "OwerId",
                table: "CaseSource",
                nullable: true,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "CaseCard",
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
                    Title = table.Column<string>(nullable: true),
                    Content = table.Column<string>(nullable: true),
                    CaseInitialId = table.Column<int>(nullable: false),
                    Status = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CaseCard", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CaseCard_CaseInitial_CaseInitialId",
                        column: x => x.CaseInitialId,
                        principalTable: "CaseInitial",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CaseCard_User_CreatorUserId",
                        column: x => x.CreatorUserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CaseCard_User_DeleterUserId",
                        column: x => x.DeleterUserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CaseCard_User_LastModifierUserId",
                        column: x => x.LastModifierUserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CaseCard_Tenant_TenantId",
                        column: x => x.TenantId,
                        principalTable: "Tenant",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CaseFile",
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
                    CaseInitialId = table.Column<int>(nullable: false),
                    Status = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    Title = table.Column<string>(nullable: true),
                    Content = table.Column<string>(nullable: true),
                    MediaPath = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CaseFile", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CaseFile_CaseInitial_CaseInitialId",
                        column: x => x.CaseInitialId,
                        principalTable: "CaseInitial",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CaseFile_User_CreatorUserId",
                        column: x => x.CreatorUserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CaseFile_User_DeleterUserId",
                        column: x => x.DeleterUserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CaseFile_User_LastModifierUserId",
                        column: x => x.LastModifierUserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CaseFile_Tenant_TenantId",
                        column: x => x.TenantId,
                        principalTable: "Tenant",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CaseKey",
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
                    KeyName = table.Column<string>(nullable: true),
                    KeyValue = table.Column<string>(nullable: true),
                    KeyNodeId = table.Column<int>(nullable: false),
                    CaseInitialId = table.Column<int>(nullable: true),
                    CaseFineId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CaseKey", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CaseKey_CaseFile_CaseFineId",
                        column: x => x.CaseFineId,
                        principalTable: "CaseFile",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CaseKey_CaseInitial_CaseInitialId",
                        column: x => x.CaseInitialId,
                        principalTable: "CaseInitial",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CaseKey_User_CreatorUserId",
                        column: x => x.CreatorUserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CaseKey_User_DeleterUserId",
                        column: x => x.DeleterUserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CaseKey_BaseTree_KeyNodeId",
                        column: x => x.KeyNodeId,
                        principalTable: "BaseTree",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CaseKey_User_LastModifierUserId",
                        column: x => x.LastModifierUserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CaseKey_Tenant_TenantId",
                        column: x => x.TenantId,
                        principalTable: "Tenant",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CaseSource_OwerId",
                table: "CaseSource",
                column: "OwerId");

            migrationBuilder.CreateIndex(
                name: "IX_CaseCard_CaseInitialId",
                table: "CaseCard",
                column: "CaseInitialId");

            migrationBuilder.CreateIndex(
                name: "IX_CaseCard_CreatorUserId",
                table: "CaseCard",
                column: "CreatorUserId");

            migrationBuilder.CreateIndex(
                name: "IX_CaseCard_DeleterUserId",
                table: "CaseCard",
                column: "DeleterUserId");

            migrationBuilder.CreateIndex(
                name: "IX_CaseCard_LastModifierUserId",
                table: "CaseCard",
                column: "LastModifierUserId");

            migrationBuilder.CreateIndex(
                name: "IX_CaseCard_TenantId",
                table: "CaseCard",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_CaseFile_CaseInitialId",
                table: "CaseFile",
                column: "CaseInitialId");

            migrationBuilder.CreateIndex(
                name: "IX_CaseFile_CreatorUserId",
                table: "CaseFile",
                column: "CreatorUserId");

            migrationBuilder.CreateIndex(
                name: "IX_CaseFile_DeleterUserId",
                table: "CaseFile",
                column: "DeleterUserId");

            migrationBuilder.CreateIndex(
                name: "IX_CaseFile_LastModifierUserId",
                table: "CaseFile",
                column: "LastModifierUserId");

            migrationBuilder.CreateIndex(
                name: "IX_CaseFile_TenantId",
                table: "CaseFile",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_CaseKey_CaseFineId",
                table: "CaseKey",
                column: "CaseFineId");

            migrationBuilder.CreateIndex(
                name: "IX_CaseKey_CaseInitialId",
                table: "CaseKey",
                column: "CaseInitialId");

            migrationBuilder.CreateIndex(
                name: "IX_CaseKey_CreatorUserId",
                table: "CaseKey",
                column: "CreatorUserId");

            migrationBuilder.CreateIndex(
                name: "IX_CaseKey_DeleterUserId",
                table: "CaseKey",
                column: "DeleterUserId");

            migrationBuilder.CreateIndex(
                name: "IX_CaseKey_KeyNodeId",
                table: "CaseKey",
                column: "KeyNodeId");

            migrationBuilder.CreateIndex(
                name: "IX_CaseKey_LastModifierUserId",
                table: "CaseKey",
                column: "LastModifierUserId");

            migrationBuilder.CreateIndex(
                name: "IX_CaseKey_TenantId",
                table: "CaseKey",
                column: "TenantId");

            migrationBuilder.AddForeignKey(
                name: "FK_CaseSource_User_OwerId",
                table: "CaseSource",
                column: "OwerId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CaseSource_User_OwerId",
                table: "CaseSource");

            migrationBuilder.DropTable(
                name: "CaseCard");

            migrationBuilder.DropTable(
                name: "CaseKey");

            migrationBuilder.DropTable(
                name: "CaseFile");

            migrationBuilder.DropIndex(
                name: "IX_CaseSource_OwerId",
                table: "CaseSource");

            migrationBuilder.AlterColumn<int>(
                name: "OwerId",
                table: "CaseSource",
                nullable: true,
                oldClrType: typeof(long),
                oldNullable: true);

            migrationBuilder.AddColumn<long>(
                name: "OwerId1",
                table: "CaseSource",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "JiuFenLeiXingId",
                table: "CaseInitial",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "JiuFenYuanYinId",
                table: "CaseInitial",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Keywords",
                table: "CaseInitial",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Labels",
                table: "CaseInitial",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PanJueJieGuoId",
                table: "CaseInitial",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ZhuanTiId",
                table: "CaseInitial",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_CaseSource_OwerId1",
                table: "CaseSource",
                column: "OwerId1");

            migrationBuilder.CreateIndex(
                name: "IX_CaseInitial_JiuFenLeiXingId",
                table: "CaseInitial",
                column: "JiuFenLeiXingId");

            migrationBuilder.CreateIndex(
                name: "IX_CaseInitial_JiuFenYuanYinId",
                table: "CaseInitial",
                column: "JiuFenYuanYinId");

            migrationBuilder.CreateIndex(
                name: "IX_CaseInitial_PanJueJieGuoId",
                table: "CaseInitial",
                column: "PanJueJieGuoId");

            migrationBuilder.CreateIndex(
                name: "IX_CaseInitial_ZhuanTiId",
                table: "CaseInitial",
                column: "ZhuanTiId");

            migrationBuilder.AddForeignKey(
                name: "FK_CaseInitial_BaseTree_JiuFenLeiXingId",
                table: "CaseInitial",
                column: "JiuFenLeiXingId",
                principalTable: "BaseTree",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CaseInitial_BaseTree_JiuFenYuanYinId",
                table: "CaseInitial",
                column: "JiuFenYuanYinId",
                principalTable: "BaseTree",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CaseInitial_BaseTree_PanJueJieGuoId",
                table: "CaseInitial",
                column: "PanJueJieGuoId",
                principalTable: "BaseTree",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CaseInitial_BaseTree_ZhuanTiId",
                table: "CaseInitial",
                column: "ZhuanTiId",
                principalTable: "BaseTree",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CaseSource_User_OwerId1",
                table: "CaseSource",
                column: "OwerId1",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
