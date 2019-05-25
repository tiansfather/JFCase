using Microsoft.EntityFrameworkCore.Migrations;

namespace Master.Migrations
{
    public partial class FixEquipmentCapacity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "DayCapacity",
                table: "Equipment",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "DayCapacity",
                table: "Equipment",
                nullable: true,
                oldClrType: typeof(decimal),
                oldNullable: true);
        }
    }
}
