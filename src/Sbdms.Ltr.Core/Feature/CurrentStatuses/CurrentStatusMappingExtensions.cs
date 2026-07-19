using Sbdms.Ltr.Contracts.CurrentStatus;
using Sbdms.Ltr.Core.Domain;

namespace Sbdms.Ltr.Core.Feature.CurrentStatuses;

public static class CurrentStatusMappingExtensions
{
    public static CurrentStatusResponse ToResponse(this CurrentStatus currentStatus) =>
        new(
            currentStatus.Id,
            currentStatus.CurrentStatusId,
            currentStatus.StatusName,
            currentStatus.Status,
            currentStatus.CreatedBy,
            currentStatus.CreatedOn,
            currentStatus.ModBy,
            currentStatus.ModOn
        );
}
