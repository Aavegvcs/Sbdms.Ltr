namespace Sbdms.Ltr.Contracts.Vehicle;

public record AddVehicleRequest(
    int VehicleTypeCode,
    int? DriverId,
    int CurrentStatusId,
    string QrUniqueCode
);
