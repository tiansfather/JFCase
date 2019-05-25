using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Master.Migrations
{
    public partial class MES_ProcessTaskToTenant : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "ArrangeEndDate",
                table: "ProcessTask",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "ToTenantEstimateHours",
                table: "ProcessTask",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ToTenantId",
                table: "ProcessTask",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ArrangeEndDate",
                table: "ProcessTask");

            migrationBuilder.DropColumn(
                name: "ToTenantEstimateHours",
                table: "ProcessTask");

            migrationBuilder.DropColumn(
                name: "ToTenantId",
                table: "ProcessTask");
        }
    }
}
