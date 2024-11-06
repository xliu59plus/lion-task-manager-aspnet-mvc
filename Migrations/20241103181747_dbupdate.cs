using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LionTaskManagementApp.Migrations
{
    /// <inheritdoc />
    public partial class dbupdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "isActive",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "FullLocation",
                table: "ContractorInfo",
                newName: "FullAddress");

            migrationBuilder.AddColumn<decimal>(
                name: "PreferenceDistance",
                table: "ContractorInfo",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "ProfileSubmitTime",
                table: "ContractorInfo",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PreferenceDistance",
                table: "ContractorInfo");

            migrationBuilder.DropColumn(
                name: "ProfileSubmitTime",
                table: "ContractorInfo");

            migrationBuilder.RenameColumn(
                name: "FullAddress",
                table: "ContractorInfo",
                newName: "FullLocation");

            migrationBuilder.AddColumn<bool>(
                name: "isActive",
                table: "AspNetUsers",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }
    }
}
