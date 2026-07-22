using Sbdms.Ltr.Contracts.Vehicle;
using Sbdms.Ltr.Core.Common.Errors;
using Sbdms.Ltr.Core.Domain;
using Sbdms.Ltr.Core.Interface;
using Sbdms.SharedLibrary.ApiResponse;
using Sbdms.SharedLibrary.Common;
using Sbdms.SharedLibrary.ResultPattern;

namespace Sbdms.Ltr.Core.Feature.Vehicles;

// Upserts the vehicle's current position (one row per vehicle, overwritten on every report)
// and, alongside it, appends a permanent row to VehicleLocationHistory so every reported
// position is kept, not just the latest. The vehicle is resolved by VehicleNumber since the
// reporting GPS device doesn't know our internal Vehicle.Id.
public class UpdateVehicleLocationHandler(
    IVehicleRepository vehicleRepository,
    IVehicleLocationRepository locationRepository,
    IVehicleLocationHistoryRepository historyRepository,
    IUnitOfWork unitOfWork)
{
    public async Task<Result<CoreResponse<VehicleLocationResponse>>> HandleAsync(UpdateVehicleLocationRequest request)
    {
        var vehicle = await vehicleRepository.GetByAsync(v => v.VehicleNumber == request.VehicleNumber);
        if (vehicle is null)
            return VehicleErrors.VehicleNotFound;

        var now = DateTime.UtcNow;
        var location = await locationRepository.GetByVehicleIdAsync(vehicle.Id);

        if (location is null)
        {
            location = VehicleLocation.Create(vehicle.Id, request.Latitude, request.Longitude, now);
            locationRepository.Add(location);
        }
        else
        {
            location.UpdatePosition(request.Latitude, request.Longitude, now);
        }

        historyRepository.Add(VehicleLocationHistory.Create(vehicle.Id, request.Latitude, request.Longitude, now));

        await unitOfWork.SaveChangesAsync();

        return new CoreResponse<VehicleLocationResponse>(location.ToResponse(), true, "Vehicle location updated successfully.");
    }
}
