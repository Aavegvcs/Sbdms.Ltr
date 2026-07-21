using Sbdms.Ltr.Contracts.Booking;
using Sbdms.Ltr.Core.Domain;
using Sbdms.Ltr.Core.Interface;
using Sbdms.SharedLibrary.ResultPattern;

namespace Sbdms.Ltr.Core.Feature.Bookings;

// Shared by both booking-creation paths (guest-start and authenticated-create): decides whether
// a new booking for this vehicle pools into an existing trip or starts a fresh one — closing out
// the previous trip first if it's no longer poolable.
public static class BookingTripResolver
{
    public static async Task<Result<int?>> ResolveAsync(IBookingRepository bookingRepository, int vehicleId, DateTime now)
    {
        var head = await bookingRepository.GetActiveTripHeadAsync(vehicleId);
        if (head is null)
            return (int?)null;

        var decision = BookingTripPolicy.Evaluate(head.LastActivityOn, now);

        switch (decision)
        {
            case TripJoinDecision.Pool:
                head.BumpActivity(now);
                return head.Id;

            default: // Fresh — the previous trip is over, close it out.
                var members = await bookingRepository.GetTripMembersAsync(head.Id);
                foreach (var member in members)
                    member.Complete(now);

                return (int?)null;
        }
    }

    // Read-time fallback: if a trip has had no activity for longer than the pool window,
    // treat it as over even though nothing has scanned that vehicle since to trigger closure.
    public static async Task ReconcileIfStaleAsync(IBookingRepository bookingRepository, Booking booking, DateTime now)
    {
        if (booking.Status != BookingStatus.Started)
            return;

        var headId = booking.TripId ?? booking.Id;
        var isHead = booking.TripId is null || booking.TripId == booking.Id;
        var head = isHead ? booking : await bookingRepository.GetByAsync(b => b.Id == headId);
        if (head is null || !BookingTripPolicy.IsStale(head.LastActivityOn, now))
            return;

        var members = await bookingRepository.GetTripMembersAsync(headId);
        foreach (var member in members)
            member.Complete(now);
    }
}
