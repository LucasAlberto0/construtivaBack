using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace construtivaBack.Migrations
{
    /// <inheritdoc />
    public partial class AddChecklistTemplates : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Checklists_Obras_ObraId",
                table: "Checklists");

            migrationBuilder.AlterColumn<int>(
                name: "ObraId",
                table: "Checklists",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddForeignKey(
                name: "FK_Checklists_Obras_ObraId",
                table: "Checklists",
                column: "ObraId",
                principalTable: "Obras",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Checklists_Obras_ObraId",
                table: "Checklists");

            migrationBuilder.AlterColumn<int>(
                name: "ObraId",
                table: "Checklists",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Checklists_Obras_ObraId",
                table: "Checklists",
                column: "ObraId",
                principalTable: "Obras",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
