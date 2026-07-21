namespace Sbdms.Ltr.Contracts.Vehicle;

public record UpdateVehicleRequest(
    int Id,
    int VendorId,
    int VehicleTypeCode,
    int? DriverId,
    int CurrentStatusId,
    string VehicleNumber,
    string VehicleCompany,
    string Modal,
    string QrUniqueCode
);
