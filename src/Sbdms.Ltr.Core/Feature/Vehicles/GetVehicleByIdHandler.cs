using Sbdms.Ltr.Contracts.Vehicle;
using Sbdms.Ltr.Core.Interface;
using Sbdms.SharedLibrary.ApiResponse;
using Sbdms.SharedLibrary.ResultPattern;

namespace Sbdms.Ltr.Core.Feature.Vehicles;

public class GetVehicleByIdHandler(IVehicleRepository vehicleRepository)
{
    public async Task<Result<CoreResponse<VehicleResponse>>> HandleAsync(int id)
    {
        var result = await vehicleRepository.FindByIdAsync(id);
        if (result.IsError)
            return result.Errors;

        return new CoreResponse<VehicleResponse>(result.Value.ToResponse(), true, "Vehicle retrieved successfully.");
    }
}
