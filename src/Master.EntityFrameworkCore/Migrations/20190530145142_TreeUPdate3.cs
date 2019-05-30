using Microsoft.EntityFrameworkCore.Migrations;

namespace Master.Migrations
{
    public partial class TreeUPdate3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RelativeNodeId",
                table: "BaseTree",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RelativeNodeId",
                table: "BaseTree");
        }
    }
}
