using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LionTaskManagementApp.Migrations
{
    /// <inheritdoc />
    public partial class postgresql_migration_533 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "WallPicPath",
                table: "Tasks",
                newName: "WallPicKey");

            migrationBuilder.RenameColumn(
                name: "ArtworkPath",
                table: "Tasks",
                newName: "ArtworkKey");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "WallPicKey",
                table: "Tasks",
                newName: "WallPicPath");

            migrationBuilder.RenameColumn(
                name: "ArtworkKey",
                table: "Tasks",
                newName: "ArtworkPath");
        }
    }
}
