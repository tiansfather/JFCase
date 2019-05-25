using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Master.Migrations
{
    public partial class MES_BOM_AddField : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "MaterialCode",
                table: "Part",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PartRank",
                table: "Part",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "RequireDate",
                table: "Part",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MaterialCode",
                table: "Part");

            migrationBuilder.DropColumn(
                name: "PartRank",
                table: "Part");

            migrationBuilder.DropColumn(
                name: "RequireDate",
                table: "Part");
        }
    }
}
