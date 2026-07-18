using Sbdms.Ltr.Contracts.Driver;
using Sbdms.Ltr.Core.Interface;
using Sbdms.SharedLibrary.ApiResponse;
using Sbdms.SharedLibrary.ResultPattern;

namespace Sbdms.Ltr.Core.Feature.Drivers;

public class GetDriverByIdHandler(IDriverRepository driverRepository)
{
    public async Task<Result<CoreResponse<DriverResponse>>> HandleAsync(int id)
    {
        var result = await driverRepository.FindByIdAsync(id);
        if (result.IsError)
            return result.Errors;

        return new CoreResponse<DriverResponse>(result.Value.ToResponse(), true, "Driver retrieved successfully.");
    }
}
