using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LionTaskManagementApp.Migrations
{
    /// <inheritdoc />
    public partial class update_task_model : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ArtworkPath",
                table: "Tasks",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "Deadline",
                table: "Tasks",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AddColumn<bool>(
                name: "DowngradeResolution",
                table: "Tasks",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "IndoorOutdoor",
                table: "Tasks",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "ProjectResolution",
                table: "Tasks",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "WallPicPath",
                table: "Tasks",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "WallType",
                table: "Tasks",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ArtworkPath",
                table: "Tasks");

            migrationBuilder.DropColumn(
                name: "Deadline",
                table: "Tasks");

            migrationBuilder.DropColumn(
                name: "DowngradeResolution",
                table: "Tasks");

            migrationBuilder.DropColumn(
                name: "IndoorOutdoor",
                table: "Tasks");

            migrationBuilder.DropColumn(
                name: "ProjectResolution",
                table: "Tasks");

            migrationBuilder.DropColumn(
                name: "WallPicPath",
                table: "Tasks");

            migrationBuilder.DropColumn(
                name: "WallType",
                table: "Tasks");
        }
    }
}
