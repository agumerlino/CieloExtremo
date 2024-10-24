using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CieloExtremo.Migrations
{
    /// <inheritdoc />
    public partial class infovendedor : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "mailVendedor",
                table: "Producto",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "telefonoVendedor",
                table: "Producto",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "mailVendedor",
                table: "Producto");

            migrationBuilder.DropColumn(
                name: "telefonoVendedor",
                table: "Producto");
        }
    }
}
