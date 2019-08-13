using Microsoft.EntityFrameworkCore.Migrations;

namespace Master.Migrations
{
    public partial class NewMiner_AddField : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Introduction",
                table: "NewMiner",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "WorkYear",
                table: "NewMiner",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Introduction",
                table: "NewMiner");

            migrationBuilder.DropColumn(
                name: "WorkYear",
                table: "NewMiner");
        }
    }
}
