namespace Sbdms.Ltr.Core.Feature.Bookings;

public enum TripJoinDecision
{
    // Gap since the trip's last activity is small enough that this rider is sharing the ride.
    Pool,

    // Gap is in the ambiguous middle zone — the vehicle is presumed still in use, but too late
    // to safely pool a new rider into the same trip.
    Occupied,

    // Gap is long enough that the previous trip is considered over; this is a fresh trip.
    Fresh
}

public static class BookingTripPolicy
{
    public static readonly TimeSpan PoolWindow = TimeSpan.FromMinutes(2);
    public static readonly TimeSpan OccupiedWindow = TimeSpan.FromMinutes(5);

    public static TripJoinDecision Evaluate(DateTime lastActivityOn, DateTime now)
    {
        var gap = now - lastActivityOn;

        if (gap <= PoolWindow)
            return TripJoinDecision.Pool;

        if (gap <= OccupiedWindow)
            return TripJoinDecision.Occupied;

        return TripJoinDecision.Fresh;
    }

    // Used for the read-time fallback: a trip with no activity for longer than OccupiedWindow
    // is considered over even if nothing ever explicitly closed it out.
    public static bool IsStale(DateTime lastActivityOn, DateTime now) => now - lastActivityOn > OccupiedWindow;
}
