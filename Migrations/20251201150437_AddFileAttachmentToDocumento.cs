using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace construtivaBack.Migrations
{
    /// <inheritdoc />
    public partial class AddFileAttachmentToDocumento : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Pasta",
                table: "Documentos");

            migrationBuilder.RenameColumn(
                name: "Url",
                table: "Documentos",
                newName: "Tipo");

            migrationBuilder.RenameColumn(
                name: "NomeArquivo",
                table: "Documentos",
                newName: "Nome");

            migrationBuilder.AddColumn<string>(
                name: "CaminhoArquivo",
                table: "Documentos",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<byte[]>(
                name: "ConteudoArquivo",
                table: "Documentos",
                type: "bytea",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DataAnexamento",
                table: "Documentos",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "DataUpload",
                table: "Documentos",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Descricao",
                table: "Documentos",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<long>(
                name: "TamanhoArquivo",
                table: "Documentos",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CaminhoArquivo",
                table: "Documentos");

            migrationBuilder.DropColumn(
                name: "ConteudoArquivo",
                table: "Documentos");

            migrationBuilder.DropColumn(
                name: "DataAnexamento",
                table: "Documentos");

            migrationBuilder.DropColumn(
                name: "DataUpload",
                table: "Documentos");

            migrationBuilder.DropColumn(
                name: "Descricao",
                table: "Documentos");

            migrationBuilder.DropColumn(
                name: "TamanhoArquivo",
                table: "Documentos");

            migrationBuilder.RenameColumn(
                name: "Tipo",
                table: "Documentos",
                newName: "Url");

            migrationBuilder.RenameColumn(
                name: "Nome",
                table: "Documentos",
                newName: "NomeArquivo");

            migrationBuilder.AddColumn<int>(
                name: "Pasta",
                table: "Documentos",
                type: "integer",
                nullable: true);
        }
    }
}
