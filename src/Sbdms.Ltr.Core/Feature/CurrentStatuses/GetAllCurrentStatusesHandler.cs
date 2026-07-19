using Sbdms.Ltr.Contracts.CurrentStatus;
using Sbdms.Ltr.Core.Interface;
using Sbdms.SharedLibrary.ApiResponse;
using Sbdms.SharedLibrary.ResultPattern;

namespace Sbdms.Ltr.Core.Feature.CurrentStatuses;

public class GetAllCurrentStatusesHandler(ICurrentStatusRepository currentStatusRepository)
{
    public async Task<Result<CoreResponse<IEnumerable<CurrentStatusResponse>>>> HandleAsync()
    {
        var currentStatuses = await currentStatusRepository.GetAllAsync();
        var response = currentStatuses.Select(cs => cs.ToResponse());

        return new CoreResponse<IEnumerable<CurrentStatusResponse>>(response, true, "Current statuses retrieved successfully.");
    }
}
