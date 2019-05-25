using Microsoft.EntityFrameworkCore.Migrations;

namespace Master.Migrations
{
    public partial class MES_Equipment_Person : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "ArrangerId",
                table: "Equipment",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "ProgrammerId",
                table: "Equipment",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Equipment_ArrangerId",
                table: "Equipment",
                column: "ArrangerId");

            migrationBuilder.CreateIndex(
                name: "IX_Equipment_ProgrammerId",
                table: "Equipment",
                column: "ProgrammerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Equipment_User_ArrangerId",
                table: "Equipment",
                column: "ArrangerId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Equipment_User_ProgrammerId",
                table: "Equipment",
                column: "ProgrammerId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Equipment_User_ArrangerId",
                table: "Equipment");

            migrationBuilder.DropForeignKey(
                name: "FK_Equipment_User_ProgrammerId",
                table: "Equipment");

            migrationBuilder.DropIndex(
                name: "IX_Equipment_ArrangerId",
                table: "Equipment");

            migrationBuilder.DropIndex(
                name: "IX_Equipment_ProgrammerId",
                table: "Equipment");

            migrationBuilder.DropColumn(
                name: "ArrangerId",
                table: "Equipment");

            migrationBuilder.DropColumn(
                name: "ProgrammerId",
                table: "Equipment");
        }
    }
}
