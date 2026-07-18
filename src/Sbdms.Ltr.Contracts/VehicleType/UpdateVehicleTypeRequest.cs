namespace Sbdms.Ltr.Contracts.VehicleType;

public record UpdateVehicleTypeRequest(
    Guid VehicleTypeId,
    int? LocCode,
    string VehicleTypeDesc,
    int Capacity,
    int BillingCode,
    bool IsActive,
    int Occupancy,
    string ModBy
);
