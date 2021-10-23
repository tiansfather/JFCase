using Microsoft.EntityFrameworkCore.Migrations;

namespace Master.Migrations
{
    public partial class CaseSource_BackCount : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BackCount",
                table: "CaseSource",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BackCount",
                table: "CaseSource");
        }
    }
}
