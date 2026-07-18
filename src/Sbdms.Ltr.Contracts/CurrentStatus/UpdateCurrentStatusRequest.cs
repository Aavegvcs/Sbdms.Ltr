namespace Sbdms.Ltr.Contracts.CurrentStatus;

public record UpdateCurrentStatusRequest(
    Guid CurrentStatusId,
    string StatusName,
    bool Status,
    string ModBy
);
