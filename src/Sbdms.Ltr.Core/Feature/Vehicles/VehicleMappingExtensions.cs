using Sbdms.Ltr.Contracts.Vehicle;
using Sbdms.Ltr.Core.Domain;

namespace Sbdms.Ltr.Core.Feature.Vehicles;

public static class VehicleMappingExtensions
{
    public static VehicleResponse ToResponse(this Vehicle vehicle) =>
        new(
            vehicle.Id,
            vehicle.VehicleTypeCode,
            vehicle.DriverId,
            vehicle.CurrentStatusId,
            vehicle.QrUniqueCode,
            vehicle.CreatedOn,
            vehicle.ModifiedOn
        );
}
