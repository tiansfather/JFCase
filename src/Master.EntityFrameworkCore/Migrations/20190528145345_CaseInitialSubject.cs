using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Master.Migrations
{
    public partial class CaseInitialSubject : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CaseFile_CaseInitial_CaseInitialId",
                table: "CaseFile");

            migrationBuilder.DropForeignKey(
                name: "FK_CaseFile_User_CreatorUserId",
                table: "CaseFile");

            migrationBuilder.DropForeignKey(
                name: "FK_CaseFile_User_DeleterUserId",
                table: "CaseFile");

            migrationBuilder.DropForeignKey(
                name: "FK_CaseFile_User_LastModifierUserId",
                table: "CaseFile");

            migrationBuilder.DropForeignKey(
                name: "FK_CaseFile_Tenant_TenantId",
                table: "CaseFile");

            migrationBuilder.DropForeignKey(
                name: "FK_CaseKey_CaseFile_CaseFineId",
                table: "CaseKey");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CaseFile",
                table: "CaseFile");

            migrationBuilder.RenameTable(
                name: "CaseFile",
                newName: "CaseFine");

            migrationBuilder.RenameIndex(
                name: "IX_CaseFile_TenantId",
                table: "CaseFine",
                newName: "IX_CaseFine_TenantId");

            migrationBuilder.RenameIndex(
                name: "IX_CaseFile_LastModifierUserId",
                table: "CaseFine",
                newName: "IX_CaseFine_LastModifierUserId");

            migrationBuilder.RenameIndex(
                name: "IX_CaseFile_DeleterUserId",
                table: "CaseFine",
                newName: "IX_CaseFine_DeleterUserId");

            migrationBuilder.RenameIndex(
                name: "IX_CaseFile_CreatorUserId",
                table: "CaseFine",
                newName: "IX_CaseFine_CreatorUserId");

            migrationBuilder.RenameIndex(
                name: "IX_CaseFile_CaseInitialId",
                table: "CaseFine",
                newName: "IX_CaseFine_CaseInitialId");

            migrationBuilder.AddColumn<int>(
                name: "SubjectId",
                table: "CaseInitial",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UserModifyTime",
                table: "CaseFine",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddPrimaryKey(
                name: "PK_CaseFine",
                table: "CaseFine",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_CaseInitial_SubjectId",
                table: "CaseInitial",
                column: "SubjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_CaseFine_CaseInitial_CaseInitialId",
                table: "CaseFine",
                column: "CaseInitialId",
                principalTable: "CaseInitial",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CaseFine_User_CreatorUserId",
                table: "CaseFine",
                column: "CreatorUserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CaseFine_User_DeleterUserId",
                table: "CaseFine",
                column: "DeleterUserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CaseFine_User_LastModifierUserId",
                table: "CaseFine",
                column: "LastModifierUserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CaseFine_Tenant_TenantId",
                table: "CaseFine",
                column: "TenantId",
                principalTable: "Tenant",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CaseInitial_BaseTree_SubjectId",
                table: "CaseInitial",
                column: "SubjectId",
                principalTable: "BaseTree",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CaseKey_CaseFine_CaseFineId",
                table: "CaseKey",
                column: "CaseFineId",
                principalTable: "CaseFine",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CaseFine_CaseInitial_CaseInitialId",
                table: "CaseFine");

            migrationBuilder.DropForeignKey(
                name: "FK_CaseFine_User_CreatorUserId",
                table: "CaseFine");

            migrationBuilder.DropForeignKey(
                name: "FK_CaseFine_User_DeleterUserId",
                table: "CaseFine");

            migrationBuilder.DropForeignKey(
                name: "FK_CaseFine_User_LastModifierUserId",
                table: "CaseFine");

            migrationBuilder.DropForeignKey(
                name: "FK_CaseFine_Tenant_TenantId",
                table: "CaseFine");

            migrationBuilder.DropForeignKey(
                name: "FK_CaseInitial_BaseTree_SubjectId",
                table: "CaseInitial");

            migrationBuilder.DropForeignKey(
                name: "FK_CaseKey_CaseFine_CaseFineId",
                table: "CaseKey");

            migrationBuilder.DropIndex(
                name: "IX_CaseInitial_SubjectId",
                table: "CaseInitial");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CaseFine",
                table: "CaseFine");

            migrationBuilder.DropColumn(
                name: "SubjectId",
                table: "CaseInitial");

            migrationBuilder.DropColumn(
                name: "UserModifyTime",
                table: "CaseFine");

            migrationBuilder.RenameTable(
                name: "CaseFine",
                newName: "CaseFile");

            migrationBuilder.RenameIndex(
                name: "IX_CaseFine_TenantId",
                table: "CaseFile",
                newName: "IX_CaseFile_TenantId");

            migrationBuilder.RenameIndex(
                name: "IX_CaseFine_LastModifierUserId",
                table: "CaseFile",
                newName: "IX_CaseFile_LastModifierUserId");

            migrationBuilder.RenameIndex(
                name: "IX_CaseFine_DeleterUserId",
                table: "CaseFile",
                newName: "IX_CaseFile_DeleterUserId");

            migrationBuilder.RenameIndex(
                name: "IX_CaseFine_CreatorUserId",
                table: "CaseFile",
                newName: "IX_CaseFile_CreatorUserId");

            migrationBuilder.RenameIndex(
                name: "IX_CaseFine_CaseInitialId",
                table: "CaseFile",
                newName: "IX_CaseFile_CaseInitialId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CaseFile",
                table: "CaseFile",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CaseFile_CaseInitial_CaseInitialId",
                table: "CaseFile",
                column: "CaseInitialId",
                principalTable: "CaseInitial",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CaseFile_User_CreatorUserId",
                table: "CaseFile",
                column: "CreatorUserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CaseFile_User_DeleterUserId",
                table: "CaseFile",
                column: "DeleterUserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CaseFile_User_LastModifierUserId",
                table: "CaseFile",
                column: "LastModifierUserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CaseFile_Tenant_TenantId",
                table: "CaseFile",
                column: "TenantId",
                principalTable: "Tenant",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CaseKey_CaseFile_CaseFineId",
                table: "CaseKey",
                column: "CaseFineId",
                principalTable: "CaseFile",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
