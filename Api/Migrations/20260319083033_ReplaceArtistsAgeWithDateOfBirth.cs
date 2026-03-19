using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Api.Migrations
{
    /// <inheritdoc />
    public partial class ReplaceArtistsAgeWithDateOfBirth : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Age",
                table: "Artist");

            migrationBuilder.AddColumn<DateOnly>(
                name: "DateOfBirth",
                table: "Artist",
                type: "date",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateOfBirth",
                table: "Artist");

            migrationBuilder.AddColumn<int>(
                name: "Age",
                table: "Artist",
                type: "int",
                nullable: true);
        }
    }
}
