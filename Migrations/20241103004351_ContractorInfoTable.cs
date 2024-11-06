using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LionTaskManagementApp.Migrations
{
    /// <inheritdoc />
    public partial class ContractorInfoTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Location",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "DOB",
                table: "AspNetUsers",
                newName: "RegisterTime");

            migrationBuilder.AddColumn<string>(
                name: "RequestList",
                table: "Task",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "isActive",
                table: "AspNetUsers",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "ContractorInfo",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "text", nullable: false),
                    CostPerSqrFoot = table.Column<decimal>(type: "numeric", nullable: false),
                    FullLocation = table.Column<string>(type: "text", nullable: false),
                    LatAndLongitude = table.Column<string>(type: "text", nullable: false),
                    ZipCode = table.Column<string>(type: "text", nullable: false),
                    ActivatedTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContractorInfo", x => x.UserId);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ContractorInfo");

            migrationBuilder.DropColumn(
                name: "RequestList",
                table: "Task");

            migrationBuilder.DropColumn(
                name: "isActive",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "RegisterTime",
                table: "AspNetUsers",
                newName: "DOB");

            migrationBuilder.AddColumn<string>(
                name: "Location",
                table: "AspNetUsers",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}
