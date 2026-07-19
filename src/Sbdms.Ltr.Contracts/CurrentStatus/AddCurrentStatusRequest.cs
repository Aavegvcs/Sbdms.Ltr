namespace Sbdms.Ltr.Contracts.CurrentStatus;

public record AddCurrentStatusRequest(
    string StatusName,
    bool Status,
    string CreatedBy
);
