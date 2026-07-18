namespace Sbdms.Ltr.Contracts.VehicleType;

public record VehicleTypeResponse(
    int VehicleTypeCode,
    Guid VehicleTypeId,
    int? LocCode,
    string VehicleTypeDesc,
    int Capacity,
    int BillingCode,
    bool IsActive,
    int Occupancy,
    string CreatedBy,
    DateTime CreatedOn,
    string? ModBy,
    DateTime? ModOn
);
