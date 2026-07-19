using Sbdms.Ltr.Contracts.Booking;
using Sbdms.SharedLibrary.Common;

namespace Sbdms.Ltr.Core.Domain;

public class Booking : AggregateRoot<int>
{
    public Booking() { }

    private Booking(
        int userId,
        int vehicleId,
        int? tripId,
        string? purpose,
        DateTime startTime,
        DateTime endTime,
        DateTime bookedOn)
    {
        if (endTime <= startTime)
            throw new ArgumentException("EndTime must be after StartTime", nameof(endTime));

        UserId = userId;
        VehicleId = vehicleId;
        TripId = tripId;
        Status = BookingStatus.Started;
        Purpose = purpose;
        StartTime = startTime;
        EndTime = endTime;
        BookedOn = bookedOn;
        LastActivityOn = bookedOn;
    }

    public int UserId { get; private set; }
    public int VehicleId { get; private set; }

    // Null means this booking is itself the head/first rider of its trip — its own Id is the
    // effective trip id. A non-null value points at the head booking's Id (a pooled co-rider).
    public int? TripId { get; private set; }

    public BookingStatus Status { get; private set; }
    public string? Purpose { get; private set; }
    public DateTime StartTime { get; private set; }
    public DateTime EndTime { get; private set; }
    public DateTime BookedOn { get; private set; }

    // Only meaningful on a trip head — bumped every time a new rider pools into the ride;
    // used to detect whether the trip is still joinable or has gone stale.
    public DateTime LastActivityOn { get; private set; }

    public DateTime? ModifiedOn { get; private set; }

    public static Booking Create(int userId, int vehicleId, int? tripId, string? purpose, DateTime startTime, DateTime endTime, DateTime bookedOn) =>
        new(userId, vehicleId, tripId, purpose, startTime, endTime, bookedOn);

    public void BumpActivity(DateTime activityOn)
    {
        LastActivityOn = activityOn;
        ModifiedOn = activityOn;
    }

    public void Complete(DateTime modifiedOn)
    {
        Status = BookingStatus.Completed;
        ModifiedOn = modifiedOn;
    }
}
