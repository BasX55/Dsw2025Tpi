using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dsw2025Tpi.Data.Migrations.Authenticate
{
    /// <inheritdoc />
    public partial class Loginmodel3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UsuarioLogins_Usuario_UserId",
                table: "UsuarioLogins");

            migrationBuilder.DropForeignKey(
                name: "FK_UsuarioReclamos_Usuario_UserId",
                table: "UsuarioReclamos");

            migrationBuilder.DropForeignKey(
                name: "FK_UsuarioRoles_Usuario_UserId",
                table: "UsuarioRoles");

            migrationBuilder.DropForeignKey(
                name: "FK_UsuarioTokens_Usuario_UserId",
                table: "UsuarioTokens");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Usuario",
                table: "Usuario");

            migrationBuilder.RenameTable(
                name: "Usuario",
                newName: "AspNetUsers");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AspNetUsers",
                table: "AspNetUsers",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UsuarioLogins_AspNetUsers_UserId",
                table: "UsuarioLogins",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UsuarioReclamos_AspNetUsers_UserId",
                table: "UsuarioReclamos",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UsuarioRoles_AspNetUsers_UserId",
                table: "UsuarioRoles",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UsuarioTokens_AspNetUsers_UserId",
                table: "UsuarioTokens",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UsuarioLogins_AspNetUsers_UserId",
                table: "UsuarioLogins");

            migrationBuilder.DropForeignKey(
                name: "FK_UsuarioReclamos_AspNetUsers_UserId",
                table: "UsuarioReclamos");

            migrationBuilder.DropForeignKey(
                name: "FK_UsuarioRoles_AspNetUsers_UserId",
                table: "UsuarioRoles");

            migrationBuilder.DropForeignKey(
                name: "FK_UsuarioTokens_AspNetUsers_UserId",
                table: "UsuarioTokens");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AspNetUsers",
                table: "AspNetUsers");

            migrationBuilder.RenameTable(
                name: "AspNetUsers",
                newName: "Usuario");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Usuario",
                table: "Usuario",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UsuarioLogins_Usuario_UserId",
                table: "UsuarioLogins",
                column: "UserId",
                principalTable: "Usuario",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UsuarioReclamos_Usuario_UserId",
                table: "UsuarioReclamos",
                column: "UserId",
                principalTable: "Usuario",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UsuarioRoles_Usuario_UserId",
                table: "UsuarioRoles",
                column: "UserId",
                principalTable: "Usuario",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UsuarioTokens_Usuario_UserId",
                table: "UsuarioTokens",
                column: "UserId",
                principalTable: "Usuario",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
