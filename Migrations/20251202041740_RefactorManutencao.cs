using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace construtivaBack.Migrations
{
    /// <inheritdoc />
    public partial class RefactorManutencao : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DataInicio",
                table: "Manutencoes");

            migrationBuilder.DropColumn(
                name: "DatasManutencao",
                table: "Manutencoes");

            migrationBuilder.RenameColumn(
                name: "ImagemUrl",
                table: "Manutencoes",
                newName: "FotoMimeType");

            migrationBuilder.RenameColumn(
                name: "DataTermino",
                table: "Manutencoes",
                newName: "DataManutencao");

            migrationBuilder.AddColumn<string>(
                name: "Descricao",
                table: "Manutencoes",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<byte[]>(
                name: "Foto",
                table: "Manutencoes",
                type: "bytea",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Descricao",
                table: "Manutencoes");

            migrationBuilder.DropColumn(
                name: "Foto",
                table: "Manutencoes");

            migrationBuilder.RenameColumn(
                name: "FotoMimeType",
                table: "Manutencoes",
                newName: "ImagemUrl");

            migrationBuilder.RenameColumn(
                name: "DataManutencao",
                table: "Manutencoes",
                newName: "DataTermino");

            migrationBuilder.AddColumn<DateTime>(
                name: "DataInicio",
                table: "Manutencoes",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "DatasManutencao",
                table: "Manutencoes",
                type: "text",
                nullable: true);
        }
    }
}
