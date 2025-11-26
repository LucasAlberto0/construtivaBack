using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace construtivaBack.Migrations
{
    /// <inheritdoc />
    public partial class UpdateObraAdministradorToNome : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Obras_AspNetUsers_AdministradorId",
                table: "Obras");

            migrationBuilder.DropIndex(
                name: "IX_Obras_AdministradorId",
                table: "Obras");

            migrationBuilder.RenameColumn(
                name: "AdministradorId",
                table: "Obras",
                newName: "AdministradorNome");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "AdministradorNome",
                table: "Obras",
                newName: "AdministradorId");

            migrationBuilder.CreateIndex(
                name: "IX_Obras_AdministradorId",
                table: "Obras",
                column: "AdministradorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Obras_AspNetUsers_AdministradorId",
                table: "Obras",
                column: "AdministradorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
