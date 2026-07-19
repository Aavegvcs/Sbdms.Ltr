using Sbdms.Ltr.Contracts.Booking;
using Sbdms.Ltr.Core.Interface;
using Sbdms.SharedLibrary.ApiResponse;
using Sbdms.SharedLibrary.Common;
using Sbdms.SharedLibrary.ResultPattern;

namespace Sbdms.Ltr.Core.Feature.Bookings;

public class GetAllBookingsHandler(IBookingRepository bookingRepository, IUnitOfWork unitOfWork)
{
    public async Task<Result<CoreResponse<IEnumerable<BookingResponse>>>> HandleAsync()
    {
        var now = DateTime.UtcNow;

        var bookings = await bookingRepository.GetAllAsync();

        // Read-time fallback: close out any stale trip heads before returning results.
        var staleHeadIds = bookings
            .Where(b => b.TripId is null && b.Status == BookingStatus.Started && BookingTripPolicy.IsStale(b.LastActivityOn, now))
            .Select(b => b.Id)
            .ToList();

        if (staleHeadIds.Count > 0)
        {
            foreach (var headId in staleHeadIds)
            {
                var members = await bookingRepository.GetTripMembersAsync(headId);
                foreach (var member in members)
                    member.Complete(now);
            }

            await unitOfWork.SaveChangesAsync();

            bookings = await bookingRepository.GetAllAsync();
        }

        var response = bookings.Select(b => b.ToResponse());

        return new CoreResponse<IEnumerable<BookingResponse>>(response, true, "Bookings retrieved successfully.");
    }
}
