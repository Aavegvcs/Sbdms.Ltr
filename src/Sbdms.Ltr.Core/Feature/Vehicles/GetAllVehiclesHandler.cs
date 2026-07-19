using Sbdms.Ltr.Contracts.Vehicle;
using Sbdms.Ltr.Core.Interface;
using Sbdms.SharedLibrary.ApiResponse;
using Sbdms.SharedLibrary.ResultPattern;

namespace Sbdms.Ltr.Core.Feature.Vehicles;

public class GetAllVehiclesHandler(IVehicleRepository vehicleRepository)
{
    public async Task<Result<CoreResponse<IEnumerable<VehicleResponse>>>> HandleAsync()
    {
        var vehicles = await vehicleRepository.GetAllAsync();
        var response = vehicles.Select(v => v.ToResponse());

        return new CoreResponse<IEnumerable<VehicleResponse>>(response, true, "Vehicles retrieved successfully.");
    }
}
