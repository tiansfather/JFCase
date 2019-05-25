using Microsoft.EntityFrameworkCore.Migrations;

namespace Master.Migrations
{
    public partial class 加工任务实际工时 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            


            migrationBuilder.AddColumn<decimal>(
                name: "ActualHours",
                table: "ProcessTask",
                nullable: true);

            
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ActualHours",
                table: "ProcessTask");

            
        }
    }
}
