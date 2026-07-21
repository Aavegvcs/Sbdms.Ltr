using Sbdms.Ltr.Contracts.Booking;
using Sbdms.Ltr.Core.Common.Errors;
using Sbdms.Ltr.Core.Interface;
using Sbdms.SharedLibrary.ApiResponse;
using Sbdms.SharedLibrary.Common;
using Sbdms.SharedLibrary.ResultPattern;

namespace Sbdms.Ltr.Core.Feature.Bookings;

public class GetLatestBookingByUserHandler(IBookingRepository bookingRepository, IUnitOfWork unitOfWork)
{
    public async Task<Result<CoreResponse<BookingResponse>>> HandleAsync(int userId)
    {
        var booking = await bookingRepository.GetLatestByUserAsync(userId);
        if (booking is null)
            return BookingErrors.BookingNotFound;

        // Read-time fallback: close out the trip if it's gone stale, even though nothing
        // has scanned this vehicle again to trigger that closure.
        await BookingTripResolver.ReconcileIfStaleAsync(bookingRepository, booking, DateTime.UtcNow);
        await unitOfWork.SaveChangesAsync();

        return new CoreResponse<BookingResponse>(booking.ToResponse(), true, "Latest booking retrieved successfully.");
    }
}
