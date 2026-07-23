using Sbdms.Ltr.Contracts.CurrentStatus;
using Sbdms.Ltr.Core.Common.Errors;
using Sbdms.Ltr.Core.Interface;
using Sbdms.SharedLibrary.ApiResponse;
using Sbdms.SharedLibrary.Common;
using Sbdms.SharedLibrary.ResultPattern;

using Sbdms.Ltr.Core.Common.Helper;
namespace Sbdms.Ltr.Core.Feature.CurrentStatuses;

public class UpdateCurrentStatusHandler(ICurrentStatusRepository currentStatusRepository, IUnitOfWork unitOfWork)
{
    public async Task<Result<CoreResponse<CurrentStatusResponse>>> HandleAsync(UpdateCurrentStatusRequest request)
    {
        var existing = await currentStatusRepository.FindByIdAsync(request.CurrentStatusId);
        if (existing.IsError)
            return existing.Errors;

        var duplicate = await currentStatusRepository.GetByAsync(cs =>
            cs.StatusName == request.StatusName && cs.CurrentStatusId != request.CurrentStatusId);
        if (duplicate is not null)
            return CurrentStatusErrors.DuplicateStatusName;

        var currentStatus = existing.Value;
        currentStatus.Update(request.StatusName, request.Status, request.ModBy, IndianStandardTime.Now);

        var result = await currentStatusRepository.UpdateAsync(currentStatus);
        if (result.IsError)
            return result.Errors;

        await unitOfWork.SaveChangesAsync();

        return new CoreResponse<CurrentStatusResponse>(currentStatus.ToResponse(), true, "Current status updated successfully.");
    }
}
