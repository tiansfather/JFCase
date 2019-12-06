using Microsoft.EntityFrameworkCore.Migrations;

namespace Master.Migrations
{
    public partial class CaseSourceUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "LawyerFirmsField",
                table: "CaseSource",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TrialPeopleField",
                table: "CaseSource",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LawyerFirmsField",
                table: "CaseSource");

            migrationBuilder.DropColumn(
                name: "TrialPeopleField",
                table: "CaseSource");
        }
    }
}
