using Sbdms.Ltr.Contracts.Vehicle;
using Sbdms.Ltr.Core.Domain;

namespace Sbdms.Ltr.Core.Feature.Vehicles;

public static class VehicleLocationMappingExtensions
{
    public static VehicleLocationResponse ToResponse(this VehicleLocation location) =>
        new(
            location.VehicleId,
            location.Latitude,
            location.Longitude,
            location.RecordedOn
        );

    public static VehicleLocationHistoryResponse ToResponse(this VehicleLocationHistory history) =>
        new(
            history.Id,
            history.VehicleId,
            history.Latitude,
            history.Longitude,
            history.RecordedOn
        );
}
