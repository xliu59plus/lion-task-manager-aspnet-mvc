using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LionTaskManagementApp.Migrations
{
    /// <inheritdoc />
    public partial class UpdateToUpperCase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "length",
                table: "Task",
                newName: "Length");

            migrationBuilder.RenameColumn(
                name: "height",
                table: "Task",
                newName: "Height");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Length",
                table: "Task",
                newName: "length");

            migrationBuilder.RenameColumn(
                name: "Height",
                table: "Task",
                newName: "height");
        }
    }
}
