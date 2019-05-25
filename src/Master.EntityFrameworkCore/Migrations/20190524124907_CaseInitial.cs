using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Master.Migrations
{
    public partial class CaseInitial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "OwerId",
                table: "CaseSource",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "OwerId1",
                table: "CaseSource",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "CaseInitial",
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
                    CaseSourceId = table.Column<int>(nullable: false),
                    Title = table.Column<string>(nullable: true),
                    JiuFenLeiXingId = table.Column<int>(nullable: false),
                    JiuFenYuanYinId = table.Column<int>(nullable: false),
                    PanJueJieGuoId = table.Column<int>(nullable: false),
                    ZhuanTiId = table.Column<int>(nullable: false),
                    Keywords = table.Column<string>(nullable: true),
                    Labels = table.Column<string>(nullable: true),
                    Introduction = table.Column<string>(nullable: true),
                    Law = table.Column<string>(nullable: true),
                    Experience = table.Column<string>(nullable: true),
                    LawyerOpinion = table.Column<string>(nullable: true),
                    Status = table.Column<string>(nullable: true),
                    PublisDate = table.Column<DateTime>(nullable: true),
                    ReadNumber = table.Column<int>(nullable: false),
                    PraiseNumber = table.Column<int>(nullable: false),
                    BeatNumber = table.Column<int>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false),
                    CaseStatus = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CaseInitial", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CaseInitial_CaseSource_CaseSourceId",
                        column: x => x.CaseSourceId,
                        principalTable: "CaseSource",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CaseInitial_User_CreatorUserId",
                        column: x => x.CreatorUserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CaseInitial_User_DeleterUserId",
                        column: x => x.DeleterUserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CaseInitial_BaseTree_JiuFenLeiXingId",
                        column: x => x.JiuFenLeiXingId,
                        principalTable: "BaseTree",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CaseInitial_BaseTree_JiuFenYuanYinId",
                        column: x => x.JiuFenYuanYinId,
                        principalTable: "BaseTree",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CaseInitial_User_LastModifierUserId",
                        column: x => x.LastModifierUserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CaseInitial_BaseTree_PanJueJieGuoId",
                        column: x => x.PanJueJieGuoId,
                        principalTable: "BaseTree",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CaseInitial_Tenant_TenantId",
                        column: x => x.TenantId,
                        principalTable: "Tenant",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CaseInitial_BaseTree_ZhuanTiId",
                        column: x => x.ZhuanTiId,
                        principalTable: "BaseTree",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CaseSourceHistory",
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
                    CaseSourceId = table.Column<int>(nullable: false),
                    Reason = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CaseSourceHistory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CaseSourceHistory_CaseSource_CaseSourceId",
                        column: x => x.CaseSourceId,
                        principalTable: "CaseSource",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CaseSourceHistory_User_CreatorUserId",
                        column: x => x.CreatorUserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CaseSourceHistory_User_DeleterUserId",
                        column: x => x.DeleterUserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CaseSourceHistory_User_LastModifierUserId",
                        column: x => x.LastModifierUserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CaseSourceHistory_Tenant_TenantId",
                        column: x => x.TenantId,
                        principalTable: "Tenant",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EmailLog",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    ToEmail = table.Column<string>(nullable: true),
                    Title = table.Column<string>(nullable: true),
                    Content = table.Column<string>(nullable: true),
                    Message = table.Column<string>(nullable: true),
                    Success = table.Column<bool>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmailLog", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CaseSource_OwerId1",
                table: "CaseSource",
                column: "OwerId1");

            migrationBuilder.CreateIndex(
                name: "IX_CaseInitial_CaseSourceId",
                table: "CaseInitial",
                column: "CaseSourceId");

            migrationBuilder.CreateIndex(
                name: "IX_CaseInitial_CreatorUserId",
                table: "CaseInitial",
                column: "CreatorUserId");

            migrationBuilder.CreateIndex(
                name: "IX_CaseInitial_DeleterUserId",
                table: "CaseInitial",
                column: "DeleterUserId");

            migrationBuilder.CreateIndex(
                name: "IX_CaseInitial_JiuFenLeiXingId",
                table: "CaseInitial",
                column: "JiuFenLeiXingId");

            migrationBuilder.CreateIndex(
                name: "IX_CaseInitial_JiuFenYuanYinId",
                table: "CaseInitial",
                column: "JiuFenYuanYinId");

            migrationBuilder.CreateIndex(
                name: "IX_CaseInitial_LastModifierUserId",
                table: "CaseInitial",
                column: "LastModifierUserId");

            migrationBuilder.CreateIndex(
                name: "IX_CaseInitial_PanJueJieGuoId",
                table: "CaseInitial",
                column: "PanJueJieGuoId");

            migrationBuilder.CreateIndex(
                name: "IX_CaseInitial_TenantId",
                table: "CaseInitial",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_CaseInitial_ZhuanTiId",
                table: "CaseInitial",
                column: "ZhuanTiId");

            migrationBuilder.CreateIndex(
                name: "IX_CaseSourceHistory_CaseSourceId",
                table: "CaseSourceHistory",
                column: "CaseSourceId");

            migrationBuilder.CreateIndex(
                name: "IX_CaseSourceHistory_CreatorUserId",
                table: "CaseSourceHistory",
                column: "CreatorUserId");

            migrationBuilder.CreateIndex(
                name: "IX_CaseSourceHistory_DeleterUserId",
                table: "CaseSourceHistory",
                column: "DeleterUserId");

            migrationBuilder.CreateIndex(
                name: "IX_CaseSourceHistory_LastModifierUserId",
                table: "CaseSourceHistory",
                column: "LastModifierUserId");

            migrationBuilder.CreateIndex(
                name: "IX_CaseSourceHistory_TenantId",
                table: "CaseSourceHistory",
                column: "TenantId");

            migrationBuilder.AddForeignKey(
                name: "FK_CaseSource_User_OwerId1",
                table: "CaseSource",
                column: "OwerId1",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CaseSource_User_OwerId1",
                table: "CaseSource");

            migrationBuilder.DropTable(
                name: "CaseInitial");

            migrationBuilder.DropTable(
                name: "CaseSourceHistory");

            migrationBuilder.DropTable(
                name: "EmailLog");

            migrationBuilder.DropIndex(
                name: "IX_CaseSource_OwerId1",
                table: "CaseSource");

            migrationBuilder.DropColumn(
                name: "OwerId",
                table: "CaseSource");

            migrationBuilder.DropColumn(
                name: "OwerId1",
                table: "CaseSource");
        }
    }
}
