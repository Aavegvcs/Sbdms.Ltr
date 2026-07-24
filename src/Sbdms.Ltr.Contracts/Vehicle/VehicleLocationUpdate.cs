namespace Sbdms.Ltr.Contracts.Vehicle;

public record VehicleLocationUpdate(
    string VehicleNumber,
    decimal Latitude,
    decimal Longitude
);