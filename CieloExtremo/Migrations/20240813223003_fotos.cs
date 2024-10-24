using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CieloExtremo.Migrations
{
    /// <inheritdoc />
    public partial class fotos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "foto",
                table: "Producto",
                newName: "foto6");

            migrationBuilder.AddColumn<string>(
                name: "foto1",
                table: "Producto",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "foto2",
                table: "Producto",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "foto3",
                table: "Producto",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "foto4",
                table: "Producto",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "foto5",
                table: "Producto",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "foto1",
                table: "Producto");

            migrationBuilder.DropColumn(
                name: "foto2",
                table: "Producto");

            migrationBuilder.DropColumn(
                name: "foto3",
                table: "Producto");

            migrationBuilder.DropColumn(
                name: "foto4",
                table: "Producto");

            migrationBuilder.DropColumn(
                name: "foto5",
                table: "Producto");

            migrationBuilder.RenameColumn(
                name: "foto6",
                table: "Producto",
                newName: "foto");
        }
    }
}
