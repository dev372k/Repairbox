using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RepairBox.DAL.Migrations
{
    public partial class addOrderDefecttbl : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderDefect_Orders_OrderId",
                table: "OrderDefect");

            migrationBuilder.DropPrimaryKey(
                name: "PK_OrderDefect",
                table: "OrderDefect");

            migrationBuilder.RenameTable(
                name: "OrderDefect",
                newName: "OrderDefects");

            migrationBuilder.RenameIndex(
                name: "IX_OrderDefect_OrderId",
                table: "OrderDefects",
                newName: "IX_OrderDefects_OrderId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_OrderDefects",
                table: "OrderDefects",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderDefects_Orders_OrderId",
                table: "OrderDefects",
                column: "OrderId",
                principalTable: "Orders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderDefects_Orders_OrderId",
                table: "OrderDefects");

            migrationBuilder.DropPrimaryKey(
                name: "PK_OrderDefects",
                table: "OrderDefects");

            migrationBuilder.RenameTable(
                name: "OrderDefects",
                newName: "OrderDefect");

            migrationBuilder.RenameIndex(
                name: "IX_OrderDefects_OrderId",
                table: "OrderDefect",
                newName: "IX_OrderDefect_OrderId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_OrderDefect",
                table: "OrderDefect",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderDefect_Orders_OrderId",
                table: "OrderDefect",
                column: "OrderId",
                principalTable: "Orders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
