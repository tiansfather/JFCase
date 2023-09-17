using Microsoft.EntityFrameworkCore.Migrations;

namespace Master.Migrations
{
    public partial class CaseInitial_TransferNum : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TransferNum",
                table: "CaseInitial",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TransferNum",
                table: "CaseInitial");
        }
    }
}
