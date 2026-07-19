namespace Sbdms.Ltr.Contracts.CurrentStatus;

public record CurrentStatusResponse(
    int Id,
    Guid CurrentStatusId,
    string StatusName,
    bool Status,
    string CreatedBy,
    DateTime CreatedOn,
    string? ModBy,
    DateTime? ModOn
);
