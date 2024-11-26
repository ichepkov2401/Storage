using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Storage.Data.Migrations
{
    /// <inheritdoc />
    public partial class Depth : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Deep",
                table: "Pallets",
                newName: "Depth");

            migrationBuilder.RenameColumn(
                name: "Deep",
                table: "Boxes",
                newName: "Depth");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Depth",
                table: "Pallets",
                newName: "Deep");

            migrationBuilder.RenameColumn(
                name: "Depth",
                table: "Boxes",
                newName: "Deep");
        }
    }
}
