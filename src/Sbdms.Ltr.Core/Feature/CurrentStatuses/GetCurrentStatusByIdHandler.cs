using Sbdms.Ltr.Contracts.CurrentStatus;
using Sbdms.Ltr.Core.Interface;
using Sbdms.SharedLibrary.ApiResponse;
using Sbdms.SharedLibrary.ResultPattern;

namespace Sbdms.Ltr.Core.Feature.CurrentStatuses;

public class GetCurrentStatusByIdHandler(ICurrentStatusRepository currentStatusRepository)
{
    public async Task<Result<CoreResponse<CurrentStatusResponse>>> HandleAsync(Guid id)
    {
        var result = await currentStatusRepository.FindByIdAsync(id);
        if (result.IsError)
            return result.Errors;

        return new CoreResponse<CurrentStatusResponse>(result.Value.ToResponse(), true, "Current status retrieved successfully.");
    }
}
