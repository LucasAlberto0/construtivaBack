using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace construtivaBack.Migrations
{
    /// <inheritdoc />
    public partial class UpdateObraResponsibles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Obras_AspNetUsers_CoordenadorId",
                table: "Obras");

            migrationBuilder.DropForeignKey(
                name: "FK_Obras_AspNetUsers_ResponsavelTecnicoId",
                table: "Obras");

            migrationBuilder.DropIndex(
                name: "IX_Obras_CoordenadorId",
                table: "Obras");

            migrationBuilder.DropIndex(
                name: "IX_Obras_ResponsavelTecnicoId",
                table: "Obras");

            migrationBuilder.RenameColumn(
                name: "ResponsavelTecnicoId",
                table: "Obras",
                newName: "ResponsavelTecnicoNome");

            migrationBuilder.RenameColumn(
                name: "CoordenadorId",
                table: "Obras",
                newName: "CoordenadorNome");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ResponsavelTecnicoNome",
                table: "Obras",
                newName: "ResponsavelTecnicoId");

            migrationBuilder.RenameColumn(
                name: "CoordenadorNome",
                table: "Obras",
                newName: "CoordenadorId");

            migrationBuilder.CreateIndex(
                name: "IX_Obras_CoordenadorId",
                table: "Obras",
                column: "CoordenadorId");

            migrationBuilder.CreateIndex(
                name: "IX_Obras_ResponsavelTecnicoId",
                table: "Obras",
                column: "ResponsavelTecnicoId");

            migrationBuilder.AddForeignKey(
                name: "FK_Obras_AspNetUsers_CoordenadorId",
                table: "Obras",
                column: "CoordenadorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Obras_AspNetUsers_ResponsavelTecnicoId",
                table: "Obras",
                column: "ResponsavelTecnicoId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
