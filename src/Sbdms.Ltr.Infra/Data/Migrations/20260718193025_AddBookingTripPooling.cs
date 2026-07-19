using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sbdms.Ltr.Infra.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddBookingTripPooling : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // BookingStatus no longer has Pending/Confirmed — remap pre-existing rows to Started
            // (their real-world meaning: the ride had begun) before the app starts reading them back.
            migrationBuilder.Sql("UPDATE [Bookings] SET [Status] = 'Started' WHERE [Status] IN ('Pending', 'Confirmed')");

            migrationBuilder.DropIndex(
                name: "IX_Bookings_VehicleId",
                table: "Bookings");

            migrationBuilder.AddColumn<DateTime>(
                name: "LastActivityOn",
                table: "Bookings",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "TripId",
                table: "Bookings",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_TripId",
                table: "Bookings",
                column: "TripId");

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_VehicleId_TripId_Status",
                table: "Bookings",
                columns: new[] { "VehicleId", "TripId", "Status" });

            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_Bookings_TripId",
                table: "Bookings",
                column: "TripId",
                principalTable: "Bookings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_Bookings_TripId",
                table: "Bookings");

            migrationBuilder.DropIndex(
                name: "IX_Bookings_TripId",
                table: "Bookings");

            migrationBuilder.DropIndex(
                name: "IX_Bookings_VehicleId_TripId_Status",
                table: "Bookings");

            migrationBuilder.DropColumn(
                name: "LastActivityOn",
                table: "Bookings");

            migrationBuilder.DropColumn(
                name: "TripId",
                table: "Bookings");

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_VehicleId",
                table: "Bookings",
                column: "VehicleId");
        }
    }
}
