using Microsoft.EntityFrameworkCore.Migrations;

namespace Master.Migrations
{
    public partial class AddRecSort : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Sort",
                table: "User",
                nullable: false,
                defaultValue: 999999);

            migrationBuilder.AddColumn<int>(
                name: "Sort",
                table: "CaseInitial",
                nullable: false,
                defaultValue: 999999);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Sort",
                table: "User");

            migrationBuilder.DropColumn(
                name: "Sort",
                table: "CaseInitial");
        }
    }
}
