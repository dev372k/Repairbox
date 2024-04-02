using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RepairBox.DAL.Migrations
{
    public partial class UpdatedCustomerIdentities : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Image1",
                table: "CustomerIdentities");

            migrationBuilder.DropColumn(
                name: "Image2",
                table: "CustomerIdentities");

            migrationBuilder.AddColumn<string>(
                name: "Image",
                table: "CustomerIdentities",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Image",
                table: "CustomerIdentities");

            migrationBuilder.AddColumn<byte[]>(
                name: "Image1",
                table: "CustomerIdentities",
                type: "varbinary(max)",
                nullable: false,
                defaultValue: new byte[0]);

            migrationBuilder.AddColumn<byte[]>(
                name: "Image2",
                table: "CustomerIdentities",
                type: "varbinary(max)",
                nullable: false,
                defaultValue: new byte[0]);
        }
    }
}
