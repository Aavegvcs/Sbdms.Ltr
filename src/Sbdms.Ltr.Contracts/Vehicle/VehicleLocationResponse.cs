namespace Sbdms.Ltr.Contracts.Vehicle;

public record VehicleLocationResponse(
    int VehicleId,
    decimal Latitude,
    decimal Longitude,
    DateTime RecordedOn
);
