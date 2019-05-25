using Microsoft.EntityFrameworkCore.Migrations;

namespace Master.Migrations
{
    public partial class MESProcessEquipment : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            

            migrationBuilder.AddColumn<int>(
                name: "EquipmentId",
                table: "ProcessTask",
                nullable: true);
            

            migrationBuilder.CreateIndex(
                name: "IX_ProcessTask_EquipmentId",
                table: "ProcessTask",
                column: "EquipmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProcessTask_Equipment_EquipmentId",
                table: "ProcessTask",
                column: "EquipmentId",
                principalTable: "Equipment",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProcessTask_Equipment_EquipmentId",
                table: "ProcessTask");

            migrationBuilder.DropIndex(
                name: "IX_ProcessTask_EquipmentId",
                table: "ProcessTask");

            migrationBuilder.DropColumn(
                name: "EquipmentId",
                table: "ProcessTask");

            
        }
    }
}
