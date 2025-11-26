using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace construtivaBack.Migrations
{
    /// <inheritdoc />
    public partial class AddObservacoesToObra : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Observacoes",
                table: "Obras",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Observacoes",
                table: "Obras");
        }
    }
}
