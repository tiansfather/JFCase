using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Master.Migrations
{
    public partial class MES_ProcessQuoteAdvance : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProcessQuoteDetail");

            migrationBuilder.DropColumn(
                name: "EstimateHours",
                table: "ProcessQuote");

            migrationBuilder.DropColumn(
                name: "FeeFactor",
                table: "ProcessQuote");

            migrationBuilder.DropColumn(
                name: "FeeType",
                table: "ProcessQuote");

            migrationBuilder.DropColumn(
                name: "PartName",
                table: "ProcessQuote");

            migrationBuilder.DropColumn(
                name: "PartNum",
                table: "ProcessQuote");

            migrationBuilder.DropColumn(
                name: "PartSpecification",
                table: "ProcessQuote");

            migrationBuilder.DropColumn(
                name: "ProcessTypeName",
                table: "ProcessQuote");

            migrationBuilder.RenameColumn(
                name: "TaskInfo",
                table: "ProcessQuote",
                newName: "QuoteSN");

            migrationBuilder.RenameColumn(
                name: "RequireDate",
                table: "ProcessQuote",
                newName: "PublishDate");

            migrationBuilder.RenameColumn(
                name: "ProjectName",
                table: "ProcessQuote",
                newName: "QuoteName");

            migrationBuilder.AddColumn<DateTime>(
                name: "ExpireDate",
                table: "ProcessQuote",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "QuotePayType",
                table: "ProcessQuote",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "QuoteScope",
                table: "ProcessQuote",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "ProcessQuoteBid",
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
                    ProcessQuoteId = table.Column<int>(nullable: false),
                    UnitId = table.Column<int>(nullable: true),
                    ToTenantId = table.Column<int>(nullable: true),
                    BidDate = table.Column<DateTime>(nullable: true),
                    BidType = table.Column<int>(nullable: true),
                    QuoteBidStatus = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProcessQuoteBid", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProcessQuoteBid_User_CreatorUserId",
                        column: x => x.CreatorUserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProcessQuoteBid_User_DeleterUserId",
                        column: x => x.DeleterUserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProcessQuoteBid_User_LastModifierUserId",
                        column: x => x.LastModifierUserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProcessQuoteBid_ProcessQuote_ProcessQuoteId",
                        column: x => x.ProcessQuoteId,
                        principalTable: "ProcessQuote",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProcessQuoteBid_Tenant_TenantId",
                        column: x => x.TenantId,
                        principalTable: "Tenant",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProcessQuoteBid_Tenant_ToTenantId",
                        column: x => x.ToTenantId,
                        principalTable: "Tenant",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProcessQuoteBid_Unit_UnitId",
                        column: x => x.UnitId,
                        principalTable: "Unit",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ProcessQuoteTask",
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
                    ProcessQuoteId = table.Column<int>(nullable: false),
                    ProcessTaskId = table.Column<int>(nullable: true),
                    ProjectName = table.Column<string>(nullable: true),
                    PartName = table.Column<string>(nullable: true),
                    PartSpecification = table.Column<string>(nullable: true),
                    PartNum = table.Column<int>(nullable: false),
                    RequireDate = table.Column<DateTime>(nullable: true),
                    EstimateHours = table.Column<int>(nullable: true),
                    FeeType = table.Column<int>(nullable: false),
                    ProcessTypeName = table.Column<string>(nullable: true),
                    FeeFactor = table.Column<decimal>(nullable: true),
                    TaskInfo = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProcessQuoteTask", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProcessQuoteTask_User_CreatorUserId",
                        column: x => x.CreatorUserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProcessQuoteTask_User_DeleterUserId",
                        column: x => x.DeleterUserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProcessQuoteTask_User_LastModifierUserId",
                        column: x => x.LastModifierUserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProcessQuoteTask_ProcessQuote_ProcessQuoteId",
                        column: x => x.ProcessQuoteId,
                        principalTable: "ProcessQuote",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProcessQuoteTask_ProcessTask_ProcessTaskId",
                        column: x => x.ProcessTaskId,
                        principalTable: "ProcessTask",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProcessQuoteTask_Tenant_TenantId",
                        column: x => x.TenantId,
                        principalTable: "Tenant",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProcessQuoteBid_CreatorUserId",
                table: "ProcessQuoteBid",
                column: "CreatorUserId");

            migrationBuilder.CreateIndex(
                name: "IX_ProcessQuoteBid_DeleterUserId",
                table: "ProcessQuoteBid",
                column: "DeleterUserId");

            migrationBuilder.CreateIndex(
                name: "IX_ProcessQuoteBid_LastModifierUserId",
                table: "ProcessQuoteBid",
                column: "LastModifierUserId");

            migrationBuilder.CreateIndex(
                name: "IX_ProcessQuoteBid_ProcessQuoteId",
                table: "ProcessQuoteBid",
                column: "ProcessQuoteId");

            migrationBuilder.CreateIndex(
                name: "IX_ProcessQuoteBid_TenantId",
                table: "ProcessQuoteBid",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_ProcessQuoteBid_ToTenantId",
                table: "ProcessQuoteBid",
                column: "ToTenantId");

            migrationBuilder.CreateIndex(
                name: "IX_ProcessQuoteBid_UnitId",
                table: "ProcessQuoteBid",
                column: "UnitId");

            migrationBuilder.CreateIndex(
                name: "IX_ProcessQuoteTask_CreatorUserId",
                table: "ProcessQuoteTask",
                column: "CreatorUserId");

            migrationBuilder.CreateIndex(
                name: "IX_ProcessQuoteTask_DeleterUserId",
                table: "ProcessQuoteTask",
                column: "DeleterUserId");

            migrationBuilder.CreateIndex(
                name: "IX_ProcessQuoteTask_LastModifierUserId",
                table: "ProcessQuoteTask",
                column: "LastModifierUserId");

            migrationBuilder.CreateIndex(
                name: "IX_ProcessQuoteTask_ProcessQuoteId",
                table: "ProcessQuoteTask",
                column: "ProcessQuoteId");

            migrationBuilder.CreateIndex(
                name: "IX_ProcessQuoteTask_ProcessTaskId",
                table: "ProcessQuoteTask",
                column: "ProcessTaskId");

            migrationBuilder.CreateIndex(
                name: "IX_ProcessQuoteTask_TenantId",
                table: "ProcessQuoteTask",
                column: "TenantId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProcessQuoteBid");

            migrationBuilder.DropTable(
                name: "ProcessQuoteTask");

            migrationBuilder.DropColumn(
                name: "ExpireDate",
                table: "ProcessQuote");

            migrationBuilder.DropColumn(
                name: "QuotePayType",
                table: "ProcessQuote");

            migrationBuilder.DropColumn(
                name: "QuoteScope",
                table: "ProcessQuote");

            migrationBuilder.RenameColumn(
                name: "QuoteSN",
                table: "ProcessQuote",
                newName: "TaskInfo");

            migrationBuilder.RenameColumn(
                name: "QuoteName",
                table: "ProcessQuote",
                newName: "ProjectName");

            migrationBuilder.RenameColumn(
                name: "PublishDate",
                table: "ProcessQuote",
                newName: "RequireDate");

            migrationBuilder.AddColumn<int>(
                name: "EstimateHours",
                table: "ProcessQuote",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "FeeFactor",
                table: "ProcessQuote",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "FeeType",
                table: "ProcessQuote",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "PartName",
                table: "ProcessQuote",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PartNum",
                table: "ProcessQuote",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "PartSpecification",
                table: "ProcessQuote",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProcessTypeName",
                table: "ProcessQuote",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ProcessQuoteDetail",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Cost = table.Column<decimal>(nullable: true),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    DeleterUserId = table.Column<long>(nullable: true),
                    DeletionTime = table.Column<DateTime>(nullable: true),
                    ExtensionData = table.Column<string>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierUserId = table.Column<long>(nullable: true),
                    Message = table.Column<string>(nullable: true),
                    Price = table.Column<decimal>(nullable: true),
                    ProcessQuoteId = table.Column<int>(nullable: false),
                    Property = table.Column<string>(type: "json", nullable: true),
                    QuoteDate = table.Column<DateTime>(nullable: true),
                    QuoteDetailStatus = table.Column<int>(nullable: false),
                    Remarks = table.Column<string>(nullable: true),
                    ReplyDate = table.Column<DateTime>(nullable: true),
                    TenantId = table.Column<int>(nullable: false),
                    UnitId = table.Column<int>(nullable: false)
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
                        onDelete: ReferentialAction.Cascade);
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
    }
}
