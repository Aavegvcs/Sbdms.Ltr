using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sbdms.Ltr.Infra.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddVendorAndVehicleDetails : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Vendors",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ContactNumber = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Address = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vendors", x => x.Id);
                });

            // Seed a default vendor (Id = 1) so existing Vehicle/Driver rows have somewhere valid
            // to point their backfilled VendorId at before the FK constraints below are added.
            migrationBuilder.InsertData(
                table: "Vendors",
                columns: new[] { "Name", "ContactNumber", "Email", "Address", "IsActive", "CreatedOn", "ModifiedOn" },
                values: new object[] { "Default Vendor", "0000000000", null, null, true, new DateTime(2026, 7, 21, 0, 0, 0, DateTimeKind.Utc), null });

            migrationBuilder.AddColumn<string>(
                name: "Modal",
                table: "Vehicles",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "VehicleCompany",
                table: "Vehicles",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "VehicleNumber",
                table: "Vehicles",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "VendorId",
                table: "Vehicles",
                type: "int",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.AddColumn<int>(
                name: "VendorId",
                table: "Drivers",
                type: "int",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.CreateIndex(
                name: "IX_Vehicles_VendorId",
                table: "Vehicles",
                column: "VendorId");

            migrationBuilder.CreateIndex(
                name: "IX_Drivers_VendorId",
                table: "Drivers",
                column: "VendorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Drivers_Vendors_VendorId",
                table: "Drivers",
                column: "VendorId",
                principalTable: "Vendors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Vehicles_Vendors_VendorId",
                table: "Vehicles",
                column: "VendorId",
                principalTable: "Vendors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Drivers_Vendors_VendorId",
                table: "Drivers");

            migrationBuilder.DropForeignKey(
                name: "FK_Vehicles_Vendors_VendorId",
                table: "Vehicles");

            migrationBuilder.DropTable(
                name: "Vendors");

            migrationBuilder.DropIndex(
                name: "IX_Vehicles_VendorId",
                table: "Vehicles");

            migrationBuilder.DropIndex(
                name: "IX_Drivers_VendorId",
                table: "Drivers");

            migrationBuilder.DropColumn(
                name: "Modal",
                table: "Vehicles");

            migrationBuilder.DropColumn(
                name: "VehicleCompany",
                table: "Vehicles");

            migrationBuilder.DropColumn(
                name: "VehicleNumber",
                table: "Vehicles");

            migrationBuilder.DropColumn(
                name: "VendorId",
                table: "Vehicles");

            migrationBuilder.DropColumn(
                name: "VendorId",
                table: "Drivers");
        }
    }
}
