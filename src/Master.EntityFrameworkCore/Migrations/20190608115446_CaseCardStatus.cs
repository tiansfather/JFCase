using Microsoft.EntityFrameworkCore.Migrations;

namespace Master.Migrations
{
    public partial class CaseCardStatus : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "CaseFine");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "CaseCard");

            migrationBuilder.AddColumn<int>(
                name: "CaseStatus",
                table: "CaseFine",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CaseStatus",
                table: "CaseCard",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CaseStatus",
                table: "CaseFine");

            migrationBuilder.DropColumn(
                name: "CaseStatus",
                table: "CaseCard");

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "CaseFine",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "CaseCard",
                nullable: false,
                defaultValue: false);
        }
    }
}
