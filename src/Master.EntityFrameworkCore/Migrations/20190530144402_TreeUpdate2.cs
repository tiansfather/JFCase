using Microsoft.EntityFrameworkCore.Migrations;

namespace Master.Migrations
{
    public partial class TreeUpdate2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BaseTree_BaseTree_BaseTreeId",
                table: "BaseTree");

            migrationBuilder.DropForeignKey(
                name: "FK_BaseTree_BaseTree_RelativeNodeId",
                table: "BaseTree");

            migrationBuilder.DropIndex(
                name: "IX_BaseTree_BaseTreeId",
                table: "BaseTree");

            migrationBuilder.DropIndex(
                name: "IX_BaseTree_RelativeNodeId",
                table: "BaseTree");

            migrationBuilder.DropColumn(
                name: "BaseTreeId",
                table: "BaseTree");

            migrationBuilder.DropColumn(
                name: "RelativeNodeId",
                table: "BaseTree");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BaseTreeId",
                table: "BaseTree",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RelativeNodeId",
                table: "BaseTree",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_BaseTree_BaseTreeId",
                table: "BaseTree",
                column: "BaseTreeId");

            migrationBuilder.CreateIndex(
                name: "IX_BaseTree_RelativeNodeId",
                table: "BaseTree",
                column: "RelativeNodeId");

            migrationBuilder.AddForeignKey(
                name: "FK_BaseTree_BaseTree_BaseTreeId",
                table: "BaseTree",
                column: "BaseTreeId",
                principalTable: "BaseTree",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_BaseTree_BaseTree_RelativeNodeId",
                table: "BaseTree",
                column: "RelativeNodeId",
                principalTable: "BaseTree",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
