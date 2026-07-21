namespace Sbdms.Ltr.Contracts.Vehicle;

public record VehicleResponse(
    int Id,
    int VendorId,
    int VehicleTypeCode,
    int? DriverId,
    int CurrentStatusId,
    string VehicleNumber,
    string VehicleCompany,
    string Modal,
    string QrUniqueCode,
    DateTime CreatedOn,
    DateTime? ModifiedOn
);
