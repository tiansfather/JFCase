using Microsoft.EntityFrameworkCore.Migrations;

namespace Master.Migrations
{
    public partial class MESBom : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
           

            migrationBuilder.AddColumn<bool>(
                name: "EnableBuy",
                table: "Part",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "EnableProcess",
                table: "Part",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "EnableStorage",
                table: "Part",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Material",
                table: "Part",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MeasureMentUnit",
                table: "Part",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ParentId",
                table: "Part",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PartSource",
                table: "Part",
                nullable: false,
                defaultValue: 0);


            migrationBuilder.CreateIndex(
                name: "IX_Part_ParentId",
                table: "Part",
                column: "ParentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Part_Part_ParentId",
                table: "Part",
                column: "ParentId",
                principalTable: "Part",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.Sql("update part set PartSource=4,enableprocess=1 where partsource=0");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Part_Part_ParentId",
                table: "Part");

            migrationBuilder.DropIndex(
                name: "IX_Part_ParentId",
                table: "Part");

            migrationBuilder.DropColumn(
                name: "EnableBuy",
                table: "Part");

            migrationBuilder.DropColumn(
                name: "EnableProcess",
                table: "Part");

            migrationBuilder.DropColumn(
                name: "EnableStorage",
                table: "Part");

            migrationBuilder.DropColumn(
                name: "Material",
                table: "Part");

            migrationBuilder.DropColumn(
                name: "MeasureMentUnit",
                table: "Part");

            migrationBuilder.DropColumn(
                name: "ParentId",
                table: "Part");

            migrationBuilder.DropColumn(
                name: "PartSource",
                table: "Part");

          
        }
    }
}
