using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RealEstateProj.Migrations
{
    /// <inheritdoc />
    public partial class wtfaid : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ID",
                table: "PropertyImages",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "Image",
                table: "PropertyImages",
                newName: "Type");

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Users",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<byte[]>(
                name: "ImageData",
                table: "PropertyImages",
                type: "BLOB",
                nullable: false,
                defaultValue: new byte[0]);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Email",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "ImageData",
                table: "PropertyImages");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "PropertyImages",
                newName: "ID");

            migrationBuilder.RenameColumn(
                name: "Type",
                table: "PropertyImages",
                newName: "Image");
        }
    }
}
