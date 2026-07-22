namespace Sbdms.Ltr.Contracts.Vehicle;

public record VehicleLocationHistoryResponse(
    int Id,
    int VehicleId,
    decimal Latitude,
    decimal Longitude,
    DateTime RecordedOn
);
