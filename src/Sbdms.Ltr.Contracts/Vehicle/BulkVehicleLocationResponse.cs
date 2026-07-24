namespace Sbdms.Ltr.Core.Feature.Vehicles;

public record BulkVehicleLocationResponse(
    int SuccessCount,
    int ErrorCount,
    List<string> Errors,
    List<VehicleCreationInfo> CreatedVehicles
);

public record VehicleCreationInfo(
    string VehicleNumber,
    int VehicleId,
    string Message
);