using Microsoft.EntityFrameworkCore.Migrations;

namespace Master.Migrations
{
    public partial class 报工进度 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            

            migrationBuilder.AddColumn<decimal>(
                name: "Progress",
                table: "ProcessTaskReport",
                nullable: false,
                defaultValue: 0m);

            
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Progress",
                table: "ProcessTaskReport");

            
        }
    }
}
