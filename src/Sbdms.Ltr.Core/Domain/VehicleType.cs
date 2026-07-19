using Sbdms.SharedLibrary.Common;

namespace Sbdms.Ltr.Core.Domain;

// Table: VehicleType. Id (AggregateRoot<int>) maps to the VehicleTypeCode identity column;
// VehicleTypeId is the Guid surrogate used as the API-facing identity (matches IGenericInterface<T>).
public class VehicleType : AggregateRoot<int>
{
    public VehicleType() { }

    private VehicleType(
        Guid vehicleTypeId,
        int? locCode,
        string vehicleTypeDesc,
        int capacity,
        int billingCode,
        bool isActive,
        int occupancy,
        string createdBy,
        DateTime createdOn)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(vehicleTypeDesc);

        if (vehicleTypeDesc.Length > 100)
            throw new ArgumentException("VehicleTypeDesc cannot exceed 100 characters", nameof(vehicleTypeDesc));

        VehicleTypeId = vehicleTypeId;
        LocCode = locCode;
        VehicleTypeDesc = vehicleTypeDesc;
        Capacity = capacity;
        BillingCode = billingCode;
        IsActive = isActive;
        Occupancy = occupancy;
        CreatedBy = createdBy;
        CreatedOn = createdOn;
    }

    public Guid VehicleTypeId { get; private set; }
    public int? LocCode { get; private set; }
    public string VehicleTypeDesc { get; private set; } = null!;
    public int Capacity { get; private set; }
    public int BillingCode { get; private set; }
    public bool IsActive { get; private set; }
    public int Occupancy { get; private set; }
    public string CreatedBy { get; private set; } = null!;
    public DateTime CreatedOn { get; private set; }
    public string? ModBy { get; private set; }
    public DateTime? ModOn { get; private set; }

    public static VehicleType Create(
        int? locCode,
        string vehicleTypeDesc,
        int capacity,
        int billingCode,
        bool isActive,
        int occupancy,
        string createdBy,
        DateTime createdOn) =>
        new(Guid.NewGuid(), locCode, vehicleTypeDesc, capacity, billingCode, isActive, occupancy, createdBy, createdOn);

    public void Update(
        int? locCode,
        string vehicleTypeDesc,
        int capacity,
        int billingCode,
        bool isActive,
        int occupancy,
        string modBy,
        DateTime modOn)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(vehicleTypeDesc);

        if (vehicleTypeDesc.Length > 100)
            throw new ArgumentException("VehicleTypeDesc cannot exceed 100 characters", nameof(vehicleTypeDesc));

        LocCode = locCode;
        VehicleTypeDesc = vehicleTypeDesc;
        Capacity = capacity;
        BillingCode = billingCode;
        IsActive = isActive;
        Occupancy = occupancy;
        ModBy = modBy;
        ModOn = modOn;
    }
}
