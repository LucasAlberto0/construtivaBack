using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace construtivaBack.Migrations
{
    /// <inheritdoc />
    public partial class StoreProfilePictureInDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ProfilePictureUrl",
                table: "AspNetUsers",
                newName: "ProfilePictureMimeType");

            migrationBuilder.AddColumn<byte[]>(
                name: "ProfilePictureData",
                table: "AspNetUsers",
                type: "bytea",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProfilePictureData",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "ProfilePictureMimeType",
                table: "AspNetUsers",
                newName: "ProfilePictureUrl");
        }
    }
}
