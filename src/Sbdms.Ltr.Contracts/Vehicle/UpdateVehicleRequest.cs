namespace Sbdms.Ltr.Contracts.Vehicle;

public record UpdateVehicleRequest(
    int Id,
    int VehicleTypeCode,
    int? DriverId,
    int CurrentStatusId,
    string QrUniqueCode
);
