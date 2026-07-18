namespace Sbdms.Ltr.Contracts.VehicleType;

public record AddVehicleTypeRequest(
    int? LocCode,
    string VehicleTypeDesc,
    int Capacity,
    int BillingCode,
    bool IsActive,
    int Occupancy,
    string CreatedBy
);
