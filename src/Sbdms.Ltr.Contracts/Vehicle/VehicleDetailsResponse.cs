using Sbdms.Ltr.Contracts.Driver;

namespace Sbdms.Ltr.Contracts.Vehicle;

// Combined vehicle + driver details for scan-time display, before booking is created.
public record VehicleDetailsResponse(
    VehicleResponse Vehicle,
    DriverResponse? Driver
);
