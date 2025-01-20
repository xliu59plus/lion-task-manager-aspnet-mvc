using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LionTaskManagementApp.Migrations
{
    /// <inheritdoc />
    public partial class postgresql_migration_938 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ChargeTravelFeesOverLimit",
                table: "ContractorInfos",
                newName: "DoesChargeTravelFeesOverLimit");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DoesChargeTravelFeesOverLimit",
                table: "ContractorInfos",
                newName: "ChargeTravelFeesOverLimit");
        }
    }
}
