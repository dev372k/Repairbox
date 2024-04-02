using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RepairBox.DAL.Migrations
{
    public partial class updatetransactiontbl : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Transactions",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "StripeCustomerId",
                table: "Transactions",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Email",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "StripeCustomerId",
                table: "Transactions");
        }
    }
}
