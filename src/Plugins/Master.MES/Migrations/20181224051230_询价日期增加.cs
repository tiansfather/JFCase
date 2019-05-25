using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Master.Migrations
{
    public partial class 询价日期增加 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProcessQuoteDetail_ProcessQuote_ProcessQuoteId",
                table: "ProcessQuoteDetail");

           

            migrationBuilder.AlterColumn<int>(
                name: "ProcessQuoteId",
                table: "ProcessQuoteDetail",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "QuoteDate",
                table: "ProcessQuoteDetail",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ReplyDate",
                table: "ProcessQuoteDetail",
                nullable: true);

            

            migrationBuilder.AddForeignKey(
                name: "FK_ProcessQuoteDetail_ProcessQuote_ProcessQuoteId",
                table: "ProcessQuoteDetail",
                column: "ProcessQuoteId",
                principalTable: "ProcessQuote",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProcessQuoteDetail_ProcessQuote_ProcessQuoteId",
                table: "ProcessQuoteDetail");

            migrationBuilder.DropColumn(
                name: "QuoteDate",
                table: "ProcessQuoteDetail");

            migrationBuilder.DropColumn(
                name: "ReplyDate",
                table: "ProcessQuoteDetail");

            

            migrationBuilder.AddForeignKey(
                name: "FK_ProcessQuoteDetail_ProcessQuote_ProcessQuoteId",
                table: "ProcessQuoteDetail",
                column: "ProcessQuoteId",
                principalTable: "ProcessQuote",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
