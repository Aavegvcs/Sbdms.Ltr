using Sbdms.Ltr.Contracts.Booking;
using Sbdms.Ltr.Core.Interface;
using Sbdms.SharedLibrary.ApiResponse;
using Sbdms.SharedLibrary.Common;
using Sbdms.SharedLibrary.ResultPattern;

using Sbdms.Ltr.Core.Common.Helper;
namespace Sbdms.Ltr.Core.Feature.Bookings;

public class GetBookingByIdHandler(IBookingRepository bookingRepository, IUnitOfWork unitOfWork)
{
    public async Task<Result<CoreResponse<BookingResponse>>> HandleAsync(int id)
    {
        var result = await bookingRepository.FindByIdAsync(id);
        if (result.IsError)
            return result.Errors;

        // Read-time fallback: close out the trip if it's gone stale, even though nothing
        // has scanned this vehicle again to trigger that closure.
        await BookingTripResolver.ReconcileIfStaleAsync(bookingRepository, result.Value, IndianStandardTime.Now);
        await unitOfWork.SaveChangesAsync();

        return new CoreResponse<BookingResponse>(result.Value.ToResponse(), true, "Booking retrieved successfully.");
    }
}
