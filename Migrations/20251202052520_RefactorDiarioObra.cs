using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace construtivaBack.Migrations
{
    /// <inheritdoc />
    public partial class RefactorDiarioObra : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FotosDiario");

            migrationBuilder.DropColumn(
                name: "Colaboradores",
                table: "DiariosDeObra");

            migrationBuilder.DropColumn(
                name: "Atividades",
                table: "DiariosDeObra");

            migrationBuilder.AddColumn<int>(
                name: "QuantidadeColaboradores",
                table: "DiariosDeObra",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "DescricaoAtividades",
                table: "DiariosDeObra",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Observacoes",
                table: "DiariosDeObra",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "Foto",
                table: "DiariosDeObra",
                type: "bytea",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FotoMimeType",
                table: "DiariosDeObra",
                type: "text",
                nullable: true);

            migrationBuilder.Sql(
                @"ALTER TABLE ""DiariosDeObra""
                  ALTER COLUMN ""Clima"" TYPE integer
                  USING CASE ""Clima""
                      WHEN 'Ensolarado' THEN 0
                      WHEN 'Parcialmente Nublado' THEN 1
                      WHEN 'Nublado' THEN 2
                      WHEN 'Chuvoso' THEN 3
                      WHEN 'Tempestade' THEN 4
                      ELSE 0
                  END;");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DescricaoAtividades",
                table: "DiariosDeObra");

            migrationBuilder.DropColumn(
                name: "Foto",
                table: "DiariosDeObra");

            migrationBuilder.DropColumn(
                name: "QuantidadeColaboradores",
                table: "DiariosDeObra");

            migrationBuilder.RenameColumn(
                name: "Observacoes",
                table: "DiariosDeObra",
                newName: "Colaboradores");

            migrationBuilder.RenameColumn(
                name: "FotoMimeType",
                table: "DiariosDeObra",
                newName: "Atividades");

            migrationBuilder.AlterColumn<string>(
                name: "Clima",
                table: "DiariosDeObra",
                type: "text",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.CreateTable(
                name: "FotosDiario",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    DiarioObraId = table.Column<int>(type: "integer", nullable: false),
                    Url = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FotosDiario", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FotosDiario_DiariosDeObra_DiarioObraId",
                        column: x => x.DiarioObraId,
                        principalTable: "DiariosDeObra",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FotosDiario_DiarioObraId",
                table: "FotosDiario",
                column: "DiarioObraId");
        }
    }
}
