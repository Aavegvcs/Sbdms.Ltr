using System.Collections.Generic;

namespace Sbdms.Ltr.Contracts.Vehicle;

public record BulkUpdateVehicleLocationRequest(
    List<VehicleLocationUpdate> VehicleLocations
);