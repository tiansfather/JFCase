using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Master.Migrations
{
    public partial class CaseUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PublisDate",
                table: "CaseInitial",
                newName: "PublishDate");

            migrationBuilder.AddColumn<DateTime>(
                name: "PublishDate",
                table: "CaseFine",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PublishDate",
                table: "CaseFine");

            migrationBuilder.RenameColumn(
                name: "PublishDate",
                table: "CaseInitial",
                newName: "PublisDate");
        }
    }
}
