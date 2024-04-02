using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RepairBox.DAL.Migrations
{
    public partial class Minorchanges2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Brands_BrandId",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_RepairableDefects_RepairableDefectId",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Orders_BrandId",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Orders_RepairableDefectId",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "BrandId",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "RepairableDefectId",
                table: "Orders");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BrandId",
                table: "Orders",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RepairableDefectId",
                table: "Orders",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Orders_BrandId",
                table: "Orders",
                column: "BrandId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_RepairableDefectId",
                table: "Orders",
                column: "RepairableDefectId");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Brands_BrandId",
                table: "Orders",
                column: "BrandId",
                principalTable: "Brands",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_RepairableDefects_RepairableDefectId",
                table: "Orders",
                column: "RepairableDefectId",
                principalTable: "RepairableDefects",
                principalColumn: "Id");
        }
    }
}
