using Sbdms.Ltr.Contracts.CurrentStatus;
using Sbdms.Ltr.Core.Common.Errors;
using Sbdms.Ltr.Core.Domain;
using Sbdms.Ltr.Core.Interface;
using Sbdms.SharedLibrary.ApiResponse;
using Sbdms.SharedLibrary.Common;
using Sbdms.SharedLibrary.ResultPattern;

namespace Sbdms.Ltr.Core.Feature.CurrentStatuses;

public class AddCurrentStatusHandler(ICurrentStatusRepository currentStatusRepository, IUnitOfWork unitOfWork)
{
    public async Task<Result<CoreResponse<CurrentStatusResponse>>> HandleAsync(AddCurrentStatusRequest request)
    {
        var duplicate = await currentStatusRepository.GetByAsync(cs => cs.StatusName == request.StatusName);
        if (duplicate is not null)
            return CurrentStatusErrors.DuplicateStatusName;

        var currentStatus = CurrentStatus.Create(request.StatusName, request.Status, request.CreatedBy, DateTime.UtcNow);

        var result = await currentStatusRepository.AddAsync(currentStatus);
        if (result.IsError)
            return result.Errors;

        await unitOfWork.SaveChangesAsync();

        return new CoreResponse<CurrentStatusResponse>(currentStatus.ToResponse(), true, "Current status added successfully.");
    }
}
