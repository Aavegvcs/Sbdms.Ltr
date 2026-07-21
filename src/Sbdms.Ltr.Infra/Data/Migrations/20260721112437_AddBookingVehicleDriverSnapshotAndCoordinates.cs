using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sbdms.Ltr.Infra.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddBookingVehicleDriverSnapshotAndCoordinates : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DriverName",
                table: "Bookings",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DriverNumber",
                table: "Bookings",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "DropLatitude",
                table: "Bookings",
                type: "decimal(9,6)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "DropLongitude",
                table: "Bookings",
                type: "decimal(9,6)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "Modal",
                table: "Bookings",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<decimal>(
                name: "PickLatitude",
                table: "Bookings",
                type: "decimal(9,6)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "PickLongitude",
                table: "Bookings",
                type: "decimal(9,6)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "VehicleNumber",
                table: "Bookings",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DriverName",
                table: "Bookings");

            migrationBuilder.DropColumn(
                name: "DriverNumber",
                table: "Bookings");

            migrationBuilder.DropColumn(
                name: "DropLatitude",
                table: "Bookings");

            migrationBuilder.DropColumn(
                name: "DropLongitude",
                table: "Bookings");

            migrationBuilder.DropColumn(
                name: "Modal",
                table: "Bookings");

            migrationBuilder.DropColumn(
                name: "PickLatitude",
                table: "Bookings");

            migrationBuilder.DropColumn(
                name: "PickLongitude",
                table: "Bookings");

            migrationBuilder.DropColumn(
                name: "VehicleNumber",
                table: "Bookings");
        }
    }
}
