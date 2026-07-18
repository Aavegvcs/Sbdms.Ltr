using Sbdms.Ltr.Contracts.VehicleType;
using Sbdms.Ltr.Core.Domain;

namespace Sbdms.Ltr.Core.Feature.VehicleTypes;

public static class VehicleTypeMappingExtensions
{
    public static VehicleTypeResponse ToResponse(this VehicleType vehicleType) =>
        new(
            vehicleType.Id,
            vehicleType.VehicleTypeId,
            vehicleType.LocCode,
            vehicleType.VehicleTypeDesc,
            vehicleType.Capacity,
            vehicleType.BillingCode,
            vehicleType.IsActive,
            vehicleType.Occupancy,
            vehicleType.CreatedBy,
            vehicleType.CreatedOn,
            vehicleType.ModBy,
            vehicleType.ModOn
        );
}
