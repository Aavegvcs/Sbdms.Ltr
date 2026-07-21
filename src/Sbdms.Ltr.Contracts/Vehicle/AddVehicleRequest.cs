namespace Sbdms.Ltr.Contracts.Vehicle;

public record AddVehicleRequest(
    int VendorId,
    int VehicleTypeCode,
    int? DriverId,
    int CurrentStatusId,
    string VehicleNumber,
    string VehicleCompany,
    string Modal,
    string QrUniqueCode
);
