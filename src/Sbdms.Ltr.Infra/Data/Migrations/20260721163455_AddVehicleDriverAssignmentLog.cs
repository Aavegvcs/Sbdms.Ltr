using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sbdms.Ltr.Infra.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddVehicleDriverAssignmentLog : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "VehicleDriverAssignmentLogs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    VehicleId = table.Column<int>(type: "int", nullable: false),
                    VehicleNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    VehicleCompany = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Modal = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    VendorId = table.Column<int>(type: "int", nullable: false),
                    OldDriverId = table.Column<int>(type: "int", nullable: true),
                    OldDriverName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    OldDriverNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    OldLicenceNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    NewDriverId = table.Column<int>(type: "int", nullable: true),
                    ChangedOn = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VehicleDriverAssignmentLogs", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_VehicleDriverAssignmentLogs_VehicleId",
                table: "VehicleDriverAssignmentLogs",
                column: "VehicleId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "VehicleDriverAssignmentLogs");
        }
    }
}
