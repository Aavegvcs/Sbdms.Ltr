using Sbdms.Ltr.Contracts.Vehicle;
using Sbdms.Ltr.Core.Common.Errors;
using Sbdms.Ltr.Core.Interface;
using Sbdms.SharedLibrary.ApiResponse;
using Sbdms.SharedLibrary.ResultPattern;

namespace Sbdms.Ltr.Core.Feature.Vehicles;

public class GetVehicleLocationHandler(IVehicleLocationRepository locationRepository)
{
    public async Task<Result<CoreResponse<VehicleLocationResponse>>> HandleAsync(int vehicleId)
    {
        var location = await locationRepository.GetByVehicleIdAsync(vehicleId);
        if (location is null)
            return VehicleErrors.LocationNotFound;

        return new CoreResponse<VehicleLocationResponse>(location.ToResponse(), true, "Vehicle location retrieved successfully.");
    }
}
