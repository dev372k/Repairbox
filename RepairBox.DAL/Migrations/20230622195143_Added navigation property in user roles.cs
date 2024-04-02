using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RepairBox.DAL.Migrations
{
    public partial class Addednavigationpropertyinuserroles : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Roles_UserRoleId",
                table: "Users");

            migrationBuilder.AlterColumn<int>(
                name: "UserRoleId",
                table: "Users",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserRole_Permissions_PermissionId",
                table: "UserRole_Permissions",
                column: "PermissionId");

            migrationBuilder.CreateIndex(
                name: "IX_Resources_PermissionId",
                table: "Resources",
                column: "PermissionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Resources_Permissions_PermissionId",
                table: "Resources",
                column: "PermissionId",
                principalTable: "Permissions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserRole_Permissions_Permissions_PermissionId",
                table: "UserRole_Permissions",
                column: "PermissionId",
                principalTable: "Permissions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserRole_Permissions_Roles_RoleId",
                table: "UserRole_Permissions",
                column: "RoleId",
                principalTable: "Roles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Roles_UserRoleId",
                table: "Users",
                column: "UserRoleId",
                principalTable: "Roles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Resources_Permissions_PermissionId",
                table: "Resources");

            migrationBuilder.DropForeignKey(
                name: "FK_UserRole_Permissions_Permissions_PermissionId",
                table: "UserRole_Permissions");

            migrationBuilder.DropForeignKey(
                name: "FK_UserRole_Permissions_Roles_RoleId",
                table: "UserRole_Permissions");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_Roles_UserRoleId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_UserRole_Permissions_PermissionId",
                table: "UserRole_Permissions");

            migrationBuilder.DropIndex(
                name: "IX_Resources_PermissionId",
                table: "Resources");

            migrationBuilder.AlterColumn<int>(
                name: "UserRoleId",
                table: "Users",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Roles_UserRoleId",
                table: "Users",
                column: "UserRoleId",
                principalTable: "Roles",
                principalColumn: "Id");
        }
    }
}
