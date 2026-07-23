using Sbdms.Ltr.Contracts.Booking;
using Sbdms.Ltr.Core.Common.Errors;
using Sbdms.Ltr.Core.Common.Helper;
using Sbdms.Ltr.Core.Interface;
using Sbdms.SharedLibrary.ApiResponse;
using Sbdms.SharedLibrary.Common;
using Sbdms.SharedLibrary.ResultPattern;

namespace Sbdms.Ltr.Core.Feature.Bookings;

// Explicit "I'm done with this ride" from the rider's own app — independent of any other
// rider pooled into the same trip; each booking is completed on its own.
public class CompleteBookingHandler(IBookingRepository bookingRepository, IUnitOfWork unitOfWork)
{
    public async Task<Result<CoreResponse<BookingResponse>>> HandleAsync(int userId, int bookingId)
    {
        var existing = await bookingRepository.FindByIdAsync(bookingId);
        if (existing.IsError)
            return existing.Errors;

        var booking = existing.Value;

        if (booking.UserId != userId)
            return BookingErrors.NotYourBooking;

        if (booking.Status != BookingStatus.Started)
            return BookingErrors.AlreadyCompleted;

        booking.Complete(IndianStandardTime.Now);

        await unitOfWork.SaveChangesAsync();

        return new CoreResponse<BookingResponse>(booking.ToResponse(), true, "Booking completed successfully.");
    }
}
