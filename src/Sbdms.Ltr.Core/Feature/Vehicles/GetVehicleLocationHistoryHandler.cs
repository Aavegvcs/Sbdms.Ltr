using Sbdms.Ltr.Contracts.Vehicle;
using Sbdms.Ltr.Core.Interface;
using Sbdms.SharedLibrary.ApiResponse;
using Sbdms.SharedLibrary.ResultPattern;

namespace Sbdms.Ltr.Core.Feature.Vehicles;

public class GetVehicleLocationHistoryHandler(IVehicleLocationHistoryRepository historyRepository)
{
    public async Task<Result<CoreResponse<IEnumerable<VehicleLocationHistoryResponse>>>> HandleAsync(int vehicleId)
    {
        var history = await historyRepository.GetByVehicleIdAsync(vehicleId);
        var response = history.Select(h => h.ToResponse());

        return new CoreResponse<IEnumerable<VehicleLocationHistoryResponse>>(response, true, "Vehicle location history retrieved successfully.");
    }
}
