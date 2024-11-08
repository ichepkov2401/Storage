﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Storage.Data.Migrations
{
    /// <inheritdoc />
    public partial class Weightadd : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "Weight",
                table: "Boxes",
                type: "REAL",
                nullable: false,
                defaultValue: 0.0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Weight",
                table: "Boxes");
        }
    }
}
