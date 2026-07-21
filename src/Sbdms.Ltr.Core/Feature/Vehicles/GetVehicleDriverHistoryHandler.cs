using Sbdms.Ltr.Contracts.Vehicle;
using Sbdms.Ltr.Core.Interface;
using Sbdms.SharedLibrary.ApiResponse;
using Sbdms.SharedLibrary.ResultPattern;

namespace Sbdms.Ltr.Core.Feature.Vehicles;

public class GetVehicleDriverHistoryHandler(IVehicleDriverAssignmentLogRepository logRepository)
{
    public async Task<Result<CoreResponse<IEnumerable<VehicleDriverAssignmentLogResponse>>>> HandleAsync(int vehicleId)
    {
        var logs = await logRepository.GetByVehicleIdAsync(vehicleId);
        var response = logs.Select(l => l.ToResponse());

        return new CoreResponse<IEnumerable<VehicleDriverAssignmentLogResponse>>(response, true, "Driver assignment history retrieved successfully.");
    }
}
