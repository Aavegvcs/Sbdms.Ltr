using Sbdms.Ltr.Contracts.VehicleType;
using Sbdms.Ltr.Core.Interface;
using Sbdms.SharedLibrary.ApiResponse;
using Sbdms.SharedLibrary.ResultPattern;

namespace Sbdms.Ltr.Core.Feature.VehicleTypes;

public class GetVehicleTypeByIdHandler(IVehicleTypeRepository vehicleTypeRepository)
{
    public async Task<Result<CoreResponse<VehicleTypeResponse>>> HandleAsync(Guid id)
    {
        var result = await vehicleTypeRepository.FindByIdAsync(id);
        if (result.IsError)
            return result.Errors;

        return new CoreResponse<VehicleTypeResponse>(result.Value.ToResponse(), true, "Vehicle type retrieved successfully.");
    }
}
