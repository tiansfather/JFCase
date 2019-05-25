using Microsoft.EntityFrameworkCore.Migrations;

namespace Master.Migrations
{
    public partial class 加工询价增加状态 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Quoted",
                table: "ProcessQuoteDetail");

            

            migrationBuilder.AddColumn<int>(
                name: "QuoteDetailStatus",
                table: "ProcessQuoteDetail",
                nullable: false,
                defaultValue: 0);

            
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "QuoteDetailStatus",
                table: "ProcessQuoteDetail");

            

            migrationBuilder.AddColumn<bool>(
                name: "Quoted",
                table: "ProcessQuoteDetail",
                nullable: false,
                defaultValue: false);

            
        }
    }
}
