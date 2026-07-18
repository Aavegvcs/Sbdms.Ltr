using Sbdms.Ltr.Contracts.Driver;
using Sbdms.Ltr.Core.Interface;
using Sbdms.SharedLibrary.ApiResponse;
using Sbdms.SharedLibrary.ResultPattern;

namespace Sbdms.Ltr.Core.Feature.Drivers;

public class GetAllDriversHandler(IDriverRepository driverRepository)
{
    public async Task<Result<CoreResponse<IEnumerable<DriverResponse>>>> HandleAsync()
    {
        var drivers = await driverRepository.GetAllAsync();
        var response = drivers.Select(d => d.ToResponse());

        return new CoreResponse<IEnumerable<DriverResponse>>(response, true, "Drivers retrieved successfully.");
    }
}
