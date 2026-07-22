namespace Sbdms.Ltr.Contracts.Vehicle;

// Keyed by VehicleNumber, not the internal Vehicle.Id — the reporting GPS device only knows
// the vehicle's registration number, not our database id.
public record UpdateVehicleLocationRequest(
    string VehicleNumber,
    decimal Latitude,
    decimal Longitude
);
