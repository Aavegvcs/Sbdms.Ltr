namespace Sbdms.Ltr.Contracts.Vehicle;

// DriverId is nullable so a vehicle can also be explicitly unassigned (no driver).
public record ChangeVehicleDriverRequest(
    int? DriverId
);
