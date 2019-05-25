using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Master.Migrations
{
    public partial class MESInit : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            

            migrationBuilder.CreateTable(
                name: "RemindLog",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    TenantId = table.Column<int>(nullable: false),
                    Property = table.Column<string>(nullable: true),
                    RemindType = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Message = table.Column<string>(nullable: true),
                    Success = table.Column<bool>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RemindLog", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RemindLog_Tenant_TenantId",
                        column: x => x.TenantId,
                        principalTable: "Tenant",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            
            migrationBuilder.CreateTable(
                name: "Part",
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
                    Status = table.Column<string>(nullable: true),
                    ProjectId = table.Column<int>(nullable: false),
                    PartSN = table.Column<string>(nullable: true),
                    PartName = table.Column<string>(nullable: true),
                    PartSpecification = table.Column<string>(nullable: true),
                    PartNum = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Part", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Part_User_CreatorUserId",
                        column: x => x.CreatorUserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Part_User_DeleterUserId",
                        column: x => x.DeleterUserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Part_User_LastModifierUserId",
                        column: x => x.LastModifierUserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Part_Project_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Project",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Part_Tenant_TenantId",
                        column: x => x.TenantId,
                        principalTable: "Tenant",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            

            migrationBuilder.CreateTable(
                name: "Person",
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
                    Status = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    PersonSource = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Person", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Person_User_CreatorUserId",
                        column: x => x.CreatorUserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Person_User_DeleterUserId",
                        column: x => x.DeleterUserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Person_User_LastModifierUserId",
                        column: x => x.LastModifierUserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ProcessType",
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
                    ProcessTypeName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProcessType", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProcessType_User_CreatorUserId",
                        column: x => x.CreatorUserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProcessType_User_DeleterUserId",
                        column: x => x.DeleterUserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProcessType_User_LastModifierUserId",
                        column: x => x.LastModifierUserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProcessType_Tenant_TenantId",
                        column: x => x.TenantId,
                        principalTable: "Tenant",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Tactic",
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
                    TacticName = table.Column<string>(nullable: true),
                    TacticType = table.Column<int>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tactic", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tactic_User_CreatorUserId",
                        column: x => x.CreatorUserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Tactic_User_DeleterUserId",
                        column: x => x.DeleterUserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Tactic_User_LastModifierUserId",
                        column: x => x.LastModifierUserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Tactic_Tenant_TenantId",
                        column: x => x.TenantId,
                        principalTable: "Tenant",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

           

            migrationBuilder.CreateTable(
                name: "ProcessTask",
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
                    Status = table.Column<string>(nullable: true),
                    SupplierId = table.Column<int>(nullable: true),
                    ProcessSN = table.Column<string>(nullable: true),
                    PartId = table.Column<int>(nullable: false),
                    ProcessTypeId = table.Column<int>(nullable: false),
                    EstimateHours = table.Column<decimal>(nullable: true),
                    FeeType = table.Column<int>(nullable: false),
                    FeeFactor = table.Column<decimal>(nullable: true),
                    JobFee = table.Column<decimal>(nullable: true),
                    Fee = table.Column<decimal>(nullable: true),
                    CheckFee = table.Column<decimal>(nullable: true),
                    RequireDate = table.Column<DateTime>(nullable: true),
                    AppointDate = table.Column<DateTime>(nullable: true),
                    ArrangeDate = table.Column<DateTime>(nullable: true),
                    ReceiveDate = table.Column<DateTime>(nullable: true),
                    StartDate = table.Column<DateTime>(nullable: true),
                    EndDate = table.Column<DateTime>(nullable: true),
                    TaskInfo = table.Column<string>(nullable: true),
                    Price = table.Column<decimal>(nullable: true),
                    Progress = table.Column<decimal>(nullable: false),
                    ProcessTaskStatus = table.Column<int>(nullable: false),
                    Poster = table.Column<string>(nullable: true),
                    ProjectCharger = table.Column<string>(nullable: true),
                    CraftsMan = table.Column<string>(nullable: true),
                    Verifier = table.Column<string>(nullable: true),
                    Checker = table.Column<string>(nullable: true),
                    Sort = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProcessTask", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProcessTask_User_CreatorUserId",
                        column: x => x.CreatorUserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProcessTask_User_DeleterUserId",
                        column: x => x.DeleterUserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProcessTask_User_LastModifierUserId",
                        column: x => x.LastModifierUserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProcessTask_Part_PartId",
                        column: x => x.PartId,
                        principalTable: "Part",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProcessTask_ProcessType_ProcessTypeId",
                        column: x => x.ProcessTypeId,
                        principalTable: "ProcessType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProcessTask_Unit_SupplierId",
                        column: x => x.SupplierId,
                        principalTable: "Unit",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProcessTask_Tenant_TenantId",
                        column: x => x.TenantId,
                        principalTable: "Tenant",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TacticPerson",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    TacticId = table.Column<int>(nullable: false),
                    PersonId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TacticPerson", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TacticPerson_Person_PersonId",
                        column: x => x.PersonId,
                        principalTable: "Person",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TacticPerson_Tactic_TacticId",
                        column: x => x.TacticId,
                        principalTable: "Tactic",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProcessTaskReport",
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
                    Status = table.Column<string>(nullable: true),
                    ProcessTaskId = table.Column<int>(nullable: false),
                    ReporterId = table.Column<int>(nullable: false),
                    ReportTime = table.Column<DateTime>(nullable: false),
                    ReportType = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProcessTaskReport", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProcessTaskReport_User_CreatorUserId",
                        column: x => x.CreatorUserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProcessTaskReport_User_DeleterUserId",
                        column: x => x.DeleterUserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProcessTaskReport_User_LastModifierUserId",
                        column: x => x.LastModifierUserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProcessTaskReport_ProcessTask_ProcessTaskId",
                        column: x => x.ProcessTaskId,
                        principalTable: "ProcessTask",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProcessTaskReport_Person_ReporterId",
                        column: x => x.ReporterId,
                        principalTable: "Person",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProcessTaskReport_Tenant_TenantId",
                        column: x => x.TenantId,
                        principalTable: "Tenant",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            

            migrationBuilder.CreateIndex(
                name: "IX_Part_CreatorUserId",
                table: "Part",
                column: "CreatorUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Part_DeleterUserId",
                table: "Part",
                column: "DeleterUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Part_LastModifierUserId",
                table: "Part",
                column: "LastModifierUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Part_ProjectId",
                table: "Part",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_Part_TenantId",
                table: "Part",
                column: "TenantId");

            

            migrationBuilder.CreateIndex(
                name: "IX_Person_CreatorUserId",
                table: "Person",
                column: "CreatorUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Person_DeleterUserId",
                table: "Person",
                column: "DeleterUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Person_LastModifierUserId",
                table: "Person",
                column: "LastModifierUserId");

            migrationBuilder.CreateIndex(
                name: "IX_ProcessTask_CreatorUserId",
                table: "ProcessTask",
                column: "CreatorUserId");

            migrationBuilder.CreateIndex(
                name: "IX_ProcessTask_DeleterUserId",
                table: "ProcessTask",
                column: "DeleterUserId");

            migrationBuilder.CreateIndex(
                name: "IX_ProcessTask_LastModifierUserId",
                table: "ProcessTask",
                column: "LastModifierUserId");

            migrationBuilder.CreateIndex(
                name: "IX_ProcessTask_PartId",
                table: "ProcessTask",
                column: "PartId");

            migrationBuilder.CreateIndex(
                name: "IX_ProcessTask_ProcessTypeId",
                table: "ProcessTask",
                column: "ProcessTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_ProcessTask_SupplierId",
                table: "ProcessTask",
                column: "SupplierId");

            migrationBuilder.CreateIndex(
                name: "IX_ProcessTask_TenantId",
                table: "ProcessTask",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_ProcessTaskReport_CreatorUserId",
                table: "ProcessTaskReport",
                column: "CreatorUserId");

            migrationBuilder.CreateIndex(
                name: "IX_ProcessTaskReport_DeleterUserId",
                table: "ProcessTaskReport",
                column: "DeleterUserId");

            migrationBuilder.CreateIndex(
                name: "IX_ProcessTaskReport_LastModifierUserId",
                table: "ProcessTaskReport",
                column: "LastModifierUserId");

            migrationBuilder.CreateIndex(
                name: "IX_ProcessTaskReport_ProcessTaskId",
                table: "ProcessTaskReport",
                column: "ProcessTaskId");

            migrationBuilder.CreateIndex(
                name: "IX_ProcessTaskReport_ReporterId",
                table: "ProcessTaskReport",
                column: "ReporterId");

            migrationBuilder.CreateIndex(
                name: "IX_ProcessTaskReport_TenantId",
                table: "ProcessTaskReport",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_ProcessType_CreatorUserId",
                table: "ProcessType",
                column: "CreatorUserId");

            migrationBuilder.CreateIndex(
                name: "IX_ProcessType_DeleterUserId",
                table: "ProcessType",
                column: "DeleterUserId");

            migrationBuilder.CreateIndex(
                name: "IX_ProcessType_LastModifierUserId",
                table: "ProcessType",
                column: "LastModifierUserId");

            migrationBuilder.CreateIndex(
                name: "IX_ProcessType_TenantId",
                table: "ProcessType",
                column: "TenantId");

            
            migrationBuilder.CreateIndex(
                name: "IX_RemindLog_TenantId",
                table: "RemindLog",
                column: "TenantId");

            
            migrationBuilder.CreateIndex(
                name: "IX_Tactic_CreatorUserId",
                table: "Tactic",
                column: "CreatorUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Tactic_DeleterUserId",
                table: "Tactic",
                column: "DeleterUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Tactic_LastModifierUserId",
                table: "Tactic",
                column: "LastModifierUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Tactic_TenantId",
                table: "Tactic",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_TacticPerson_PersonId",
                table: "TacticPerson",
                column: "PersonId");

            migrationBuilder.CreateIndex(
                name: "IX_TacticPerson_TacticId",
                table: "TacticPerson",
                column: "TacticId");


        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            

            migrationBuilder.DropTable(
                name: "ProcessTaskReport");

            migrationBuilder.DropTable(
                name: "RemindLog");

           
            migrationBuilder.DropTable(
                name: "TacticPerson");

            
            migrationBuilder.DropTable(
                name: "ProcessTask");

            migrationBuilder.DropTable(
                name: "Person");

            migrationBuilder.DropTable(
                name: "Tactic");

            migrationBuilder.DropTable(
                name: "Part");

            migrationBuilder.DropTable(
                name: "ProcessType");
            
        }
    }
}
