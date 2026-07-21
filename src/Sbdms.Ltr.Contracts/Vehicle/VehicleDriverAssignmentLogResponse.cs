namespace Sbdms.Ltr.Contracts.Vehicle;

public record VehicleDriverAssignmentLogResponse(
    int Id,
    int VehicleId,
    string VehicleNumber,
    string VehicleCompany,
    string Modal,
    int VendorId,
    int? OldDriverId,
    string? OldDriverName,
    string? OldDriverNumber,
    string? OldLicenceNumber,
    int? NewDriverId,
    DateTime ChangedOn
);
