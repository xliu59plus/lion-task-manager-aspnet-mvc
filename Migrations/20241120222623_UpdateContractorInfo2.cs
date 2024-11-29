using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LionTaskManagementApp.Migrations
{
    /// <inheritdoc />
    public partial class UpdateContractorInfo2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AdditionalNotes",
                table: "ContractorInfos",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ArtworkSpecialization",
                table: "ContractorInfos",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "BankingInfo",
                table: "ContractorInfos",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "BusinessDocumentationLink",
                table: "ContractorInfos",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<decimal>(
                name: "CMYKPrice",
                table: "ContractorInfos",
                type: "numeric",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "CMYKWhiteColorPrice",
                table: "ContractorInfos",
                type: "numeric",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "ChargeTravelFeesOverLimit",
                table: "ContractorInfos",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "City",
                table: "ContractorInfos",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CompanyName",
                table: "ContractorInfos",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "DoesPrintWhiteColor",
                table: "ContractorInfos",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "EIN",
                table: "ContractorInfos",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "FacebookLink",
                table: "ContractorInfos",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "FirstLine",
                table: "ContractorInfos",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "InstagramLink",
                table: "ContractorInfos",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "ContractorInfos",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "SecondLine",
                table: "ContractorInfos",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "StateProvince",
                table: "ContractorInfos",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "SupportsCMYK",
                table: "ContractorInfos",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "TikTokLink",
                table: "ContractorInfos",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<decimal>(
                name: "TravelFeeOverLimit",
                table: "ContractorInfos",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "WallpenHubProfileLink",
                table: "ContractorInfos",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "WallpenMachineModel",
                table: "ContractorInfos",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "WallpenSerialNumber",
                table: "ContractorInfos",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<decimal>(
                name: "WhiteColorPrice",
                table: "ContractorInfos",
                type: "numeric",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AdditionalNotes",
                table: "ContractorInfos");

            migrationBuilder.DropColumn(
                name: "ArtworkSpecialization",
                table: "ContractorInfos");

            migrationBuilder.DropColumn(
                name: "BankingInfo",
                table: "ContractorInfos");

            migrationBuilder.DropColumn(
                name: "BusinessDocumentationLink",
                table: "ContractorInfos");

            migrationBuilder.DropColumn(
                name: "CMYKPrice",
                table: "ContractorInfos");

            migrationBuilder.DropColumn(
                name: "CMYKWhiteColorPrice",
                table: "ContractorInfos");

            migrationBuilder.DropColumn(
                name: "ChargeTravelFeesOverLimit",
                table: "ContractorInfos");

            migrationBuilder.DropColumn(
                name: "City",
                table: "ContractorInfos");

            migrationBuilder.DropColumn(
                name: "CompanyName",
                table: "ContractorInfos");

            migrationBuilder.DropColumn(
                name: "DoesPrintWhiteColor",
                table: "ContractorInfos");

            migrationBuilder.DropColumn(
                name: "EIN",
                table: "ContractorInfos");

            migrationBuilder.DropColumn(
                name: "FacebookLink",
                table: "ContractorInfos");

            migrationBuilder.DropColumn(
                name: "FirstLine",
                table: "ContractorInfos");

            migrationBuilder.DropColumn(
                name: "InstagramLink",
                table: "ContractorInfos");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "ContractorInfos");

            migrationBuilder.DropColumn(
                name: "SecondLine",
                table: "ContractorInfos");

            migrationBuilder.DropColumn(
                name: "StateProvince",
                table: "ContractorInfos");

            migrationBuilder.DropColumn(
                name: "SupportsCMYK",
                table: "ContractorInfos");

            migrationBuilder.DropColumn(
                name: "TikTokLink",
                table: "ContractorInfos");

            migrationBuilder.DropColumn(
                name: "TravelFeeOverLimit",
                table: "ContractorInfos");

            migrationBuilder.DropColumn(
                name: "WallpenHubProfileLink",
                table: "ContractorInfos");

            migrationBuilder.DropColumn(
                name: "WallpenMachineModel",
                table: "ContractorInfos");

            migrationBuilder.DropColumn(
                name: "WallpenSerialNumber",
                table: "ContractorInfos");

            migrationBuilder.DropColumn(
                name: "WhiteColorPrice",
                table: "ContractorInfos");
        }
    }
}
