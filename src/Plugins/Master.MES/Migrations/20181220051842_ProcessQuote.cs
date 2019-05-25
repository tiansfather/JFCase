using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Master.Migrations
{
    public partial class ProcessQuote : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            

            migrationBuilder.CreateTable(
                name: "ProcessQuote",
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
                    ProjectName = table.Column<string>(nullable: true),
                    PartName = table.Column<string>(nullable: true),
                    PartSpecification = table.Column<string>(nullable: true),
                    PartNum = table.Column<int>(nullable: false),
                    RequireDate = table.Column<DateTime>(nullable: true),
                    EstimateHours = table.Column<int>(nullable: true),
                    FeeType = table.Column<int>(nullable: false),
                    ProcessTypeName = table.Column<string>(nullable: true),
                    FeeFactor = table.Column<decimal>(nullable: true),
                    TaskInfo = table.Column<string>(nullable: true),
                    QuoteStatus = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProcessQuote", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProcessQuote_User_CreatorUserId",
                        column: x => x.CreatorUserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProcessQuote_User_DeleterUserId",
                        column: x => x.DeleterUserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProcessQuote_User_LastModifierUserId",
                        column: x => x.LastModifierUserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProcessQuote_Tenant_TenantId",
                        column: x => x.TenantId,
                        principalTable: "Tenant",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProcessQuoteDetail",
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
                    UnitId = table.Column<int>(nullable: false),
                    Message = table.Column<string>(nullable: true),
                    Price = table.Column<decimal>(nullable: true),
                    Cost = table.Column<decimal>(nullable: true),
                    Quoted = table.Column<bool>(nullable: false),
                    ProcessQuoteId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProcessQuoteDetail", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProcessQuoteDetail_User_CreatorUserId",
                        column: x => x.CreatorUserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProcessQuoteDetail_User_DeleterUserId",
                        column: x => x.DeleterUserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProcessQuoteDetail_User_LastModifierUserId",
                        column: x => x.LastModifierUserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProcessQuoteDetail_ProcessQuote_ProcessQuoteId",
                        column: x => x.ProcessQuoteId,
                        principalTable: "ProcessQuote",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProcessQuoteDetail_Tenant_TenantId",
                        column: x => x.TenantId,
                        principalTable: "Tenant",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProcessQuoteDetail_Unit_UnitId",
                        column: x => x.UnitId,
                        principalTable: "Unit",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProcessQuote_CreatorUserId",
                table: "ProcessQuote",
                column: "CreatorUserId");

            migrationBuilder.CreateIndex(
                name: "IX_ProcessQuote_DeleterUserId",
                table: "ProcessQuote",
                column: "DeleterUserId");

            migrationBuilder.CreateIndex(
                name: "IX_ProcessQuote_LastModifierUserId",
                table: "ProcessQuote",
                column: "LastModifierUserId");

            migrationBuilder.CreateIndex(
                name: "IX_ProcessQuote_TenantId",
                table: "ProcessQuote",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_ProcessQuoteDetail_CreatorUserId",
                table: "ProcessQuoteDetail",
                column: "CreatorUserId");

            migrationBuilder.CreateIndex(
                name: "IX_ProcessQuoteDetail_DeleterUserId",
                table: "ProcessQuoteDetail",
                column: "DeleterUserId");

            migrationBuilder.CreateIndex(
                name: "IX_ProcessQuoteDetail_LastModifierUserId",
                table: "ProcessQuoteDetail",
                column: "LastModifierUserId");

            migrationBuilder.CreateIndex(
                name: "IX_ProcessQuoteDetail_ProcessQuoteId",
                table: "ProcessQuoteDetail",
                column: "ProcessQuoteId");

            migrationBuilder.CreateIndex(
                name: "IX_ProcessQuoteDetail_TenantId",
                table: "ProcessQuoteDetail",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_ProcessQuoteDetail_UnitId",
                table: "ProcessQuoteDetail",
                column: "UnitId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProcessQuoteDetail");

            migrationBuilder.DropTable(
                name: "ProcessQuote");

           
        }
    }
}
