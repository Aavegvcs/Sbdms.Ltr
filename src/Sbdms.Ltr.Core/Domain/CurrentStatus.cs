using Sbdms.SharedLibrary.Common;

namespace Sbdms.Ltr.Core.Domain;

// Table: CurrentStatus. Id (AggregateRoot<int>) maps to the identity column;
// CurrentStatusId is the Guid surrogate used as the API-facing identity (matches IGenericInterface<T>).
public class CurrentStatus : AggregateRoot<int>
{
    public CurrentStatus() { }

    private CurrentStatus(
        Guid currentStatusId,
        string statusName,
        bool status,
        string createdBy,
        DateTime createdOn)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(statusName);

        if (statusName.Length > 50)
            throw new ArgumentException("StatusName cannot exceed 50 characters", nameof(statusName));

        CurrentStatusId = currentStatusId;
        StatusName = statusName;
        Status = status;
        CreatedBy = createdBy;
        CreatedOn = createdOn;
    }

    public Guid CurrentStatusId { get; private set; }
    public string StatusName { get; private set; } = null!;
    public bool Status { get; private set; }
    public string CreatedBy { get; private set; } = null!;
    public DateTime CreatedOn { get; private set; }
    public string? ModBy { get; private set; }
    public DateTime? ModOn { get; private set; }

    public static CurrentStatus Create(string statusName, bool status, string createdBy, DateTime createdOn) =>
        new(Guid.NewGuid(), statusName, status, createdBy, createdOn);

    public void Update(string statusName, bool status, string modBy, DateTime modOn)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(statusName);

        if (statusName.Length > 50)
            throw new ArgumentException("StatusName cannot exceed 50 characters", nameof(statusName));

        StatusName = statusName;
        Status = status;
        ModBy = modBy;
        ModOn = modOn;
    }
}
