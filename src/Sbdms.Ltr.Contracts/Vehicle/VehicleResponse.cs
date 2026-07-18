namespace Sbdms.Ltr.Contracts.Vehicle;

public record VehicleResponse(
    int Id,
    int VehicleTypeCode,
    int? DriverId,
    int CurrentStatusId,
    string QrUniqueCode,
    DateTime CreatedOn,
    DateTime? ModifiedOn
);
