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
        string vehicleNumber,
        string modal,
        string? driverNumber,
        string? driverName,
        decimal pickLatitude,
        decimal pickLongitude,
        decimal dropLatitude,
        decimal dropLongitude,
        string? purpose,
        DateTime startTime,
        DateTime endTime,
        DateTime bookedOn)
    {
        if (endTime <= startTime)
            throw new ArgumentException("EndTime must be after StartTime", nameof(endTime));

        ValidateLatitude(pickLatitude, nameof(pickLatitude));
        ValidateLongitude(pickLongitude, nameof(pickLongitude));
        ValidateLatitude(dropLatitude, nameof(dropLatitude));
        ValidateLongitude(dropLongitude, nameof(dropLongitude));

        UserId = userId;
        VehicleId = vehicleId;
        TripId = tripId;
        VehicleNumber = vehicleNumber;
        Modal = modal;
        DriverNumber = driverNumber;
        DriverName = driverName;
        PickLatitude = pickLatitude;
        PickLongitude = pickLongitude;
        DropLatitude = dropLatitude;
        DropLongitude = dropLongitude;
        Status = BookingStatus.Started;
        Purpose = purpose;
        StartTime = startTime;
        EndTime = endTime;
        BookedOn = bookedOn;
        LastActivityOn = bookedOn;
    }

    public int UserId { get; private set; }
    public int VehicleId { get; private set; }

    // Snapshotted from the Vehicle/Driver at booking time, so the ride record stays accurate
    // even if the vehicle is later reassigned or the driver's details change.
    public string VehicleNumber { get; private set; } = null!;
    public string Modal { get; private set; } = null!;
    public string? DriverNumber { get; private set; }
    public string? DriverName { get; private set; }

    public decimal PickLatitude { get; private set; }
    public decimal PickLongitude { get; private set; }
    public decimal DropLatitude { get; private set; }
    public decimal DropLongitude { get; private set; }

    // For the trip head/first rider, this equals the booking's own Id (set via AssignAsTripHead
    // right after creation, once Id is known). For a pooled co-rider, it points at the head
    // booking's Id instead. Stays null only until that post-creation assignment happens.
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

    public static Booking Create(
        int userId,
        int vehicleId,
        int? tripId,
        string vehicleNumber,
        string modal,
        string? driverNumber,
        string? driverName,
        decimal pickLatitude,
        decimal pickLongitude,
        decimal dropLatitude,
        decimal dropLongitude,
        string? purpose,
        DateTime startTime,
        DateTime endTime,
        DateTime bookedOn) =>
        new(userId, vehicleId, tripId, vehicleNumber, modal, driverNumber, driverName,
            pickLatitude, pickLongitude, dropLatitude, dropLongitude, purpose, startTime, endTime, bookedOn);

    // Called once, right after the first save, for a booking that starts a brand-new trip —
    // stamps TripId with its own (now DB-assigned) Id so the column is never left null.
    public void AssignAsTripHead()
    {
        TripId = Id;
    }

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

    private static void ValidateLatitude(decimal value, string paramName)
    {
        if (value < -90 || value > 90)
            throw new ArgumentException("Latitude must be between -90 and 90", paramName);
    }

    private static void ValidateLongitude(decimal value, string paramName)
    {
        if (value < -180 || value > 180)
            throw new ArgumentException("Longitude must be between -180 and 180", paramName);
    }
}
