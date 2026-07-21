using Microsoft.EntityFrameworkCore;
using Sbdms.Ltr.Contracts.Booking;
using Sbdms.Ltr.Core.Interface;
using Sbdms.SharedLibrary.ApiResponse;
using Sbdms.SharedLibrary.Common;
using Sbdms.SharedLibrary.ResultPattern;

namespace Sbdms.Ltr.Core.Feature.Bookings;

// Returns this user's completed/cancelled bookings — their currently active booking (if any)
// is served separately by GetLatestBookingByUserHandler.
public class GetBookingHistoryByUserHandler(IBookingRepository bookingRepository, IUnitOfWork unitOfWork)
{
    public async Task<Result<CoreResponse<IEnumerable<BookingResponse>>>> HandleAsync(int userId)
    {
        var now = DateTime.UtcNow;

        var bookings = await bookingRepository.GetAllAsQueryable()
            .Where(b => b.UserId == userId)
            .ToListAsync();

        var startedBookings = bookings.Where(b => b.Status == BookingStatus.Started).ToList();
        if (startedBookings.Count > 0)
        {
            // Read-time fallback: close out any of this user's bookings whose trip has gone
            // stale, even though nothing has scanned the vehicle again to trigger that closure.
            foreach (var booking in startedBookings)
                await BookingTripResolver.ReconcileIfStaleAsync(bookingRepository, booking, now);

            await unitOfWork.SaveChangesAsync();

            bookings = await bookingRepository.GetAllAsQueryable()
                .Where(b => b.UserId == userId)
                .ToListAsync();
        }

        var response = bookings
            .Where(b => b.Status != BookingStatus.Started)
            .OrderByDescending(b => b.BookedOn)
            .Select(b => b.ToResponse());

        return new CoreResponse<IEnumerable<BookingResponse>>(response, true, "Booking history retrieved successfully.");
    }
}
