using Microsoft.EntityFrameworkCore.Migrations;

namespace Master.Migrations
{
    public partial class MES_ProcessQuoteEstimateHour : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "EstimateHours",
                table: "ProcessQuoteTask",
                nullable: true,
                oldClrType: typeof(int),
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "EstimateHours",
                table: "ProcessQuoteTask",
                nullable: true,
                oldClrType: typeof(decimal),
                oldNullable: true);
        }
    }
}
