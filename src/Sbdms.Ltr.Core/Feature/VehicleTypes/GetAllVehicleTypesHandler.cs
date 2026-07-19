using Sbdms.Ltr.Contracts.VehicleType;
using Sbdms.Ltr.Core.Interface;
using Sbdms.SharedLibrary.ApiResponse;
using Sbdms.SharedLibrary.ResultPattern;

namespace Sbdms.Ltr.Core.Feature.VehicleTypes;

public class GetAllVehicleTypesHandler(IVehicleTypeRepository vehicleTypeRepository)
{
    public async Task<Result<CoreResponse<IEnumerable<VehicleTypeResponse>>>> HandleAsync()
    {
        var vehicleTypes = await vehicleTypeRepository.GetAllAsync();
        var response = vehicleTypes.Select(vt => vt.ToResponse());

        return new CoreResponse<IEnumerable<VehicleTypeResponse>>(response, true, "Vehicle types retrieved successfully.");
    }
}
