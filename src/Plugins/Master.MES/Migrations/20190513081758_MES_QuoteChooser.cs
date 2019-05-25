using Microsoft.EntityFrameworkCore.Migrations;

namespace Master.Migrations
{
    public partial class MES_QuoteChooser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "ChooseUserId",
                table: "ProcessQuote",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProcessQuote_ChooseUserId",
                table: "ProcessQuote",
                column: "ChooseUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProcessQuote_User_ChooseUserId",
                table: "ProcessQuote",
                column: "ChooseUserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProcessQuote_User_ChooseUserId",
                table: "ProcessQuote");

            migrationBuilder.DropIndex(
                name: "IX_ProcessQuote_ChooseUserId",
                table: "ProcessQuote");

            migrationBuilder.DropColumn(
                name: "ChooseUserId",
                table: "ProcessQuote");
        }
    }
}
