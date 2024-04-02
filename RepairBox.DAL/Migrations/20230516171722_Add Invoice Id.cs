using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RepairBox.DAL.Migrations
{
    public partial class AddInvoiceId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "InvoiceId",
                table: "Invoices",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "InvoiceId",
                table: "Invoices");
        }
    }
}
