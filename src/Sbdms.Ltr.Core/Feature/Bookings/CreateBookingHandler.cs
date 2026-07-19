using Sbdms.Ltr.Contracts.Booking;
using Sbdms.Ltr.Core.Common.Errors;
using Sbdms.Ltr.Core.Domain;
using Sbdms.Ltr.Core.Interface;
using Sbdms.SharedLibrary.ApiResponse;
using Sbdms.SharedLibrary.Common;
using Sbdms.SharedLibrary.ResultPattern;

namespace Sbdms.Ltr.Core.Feature.Bookings;

// Authenticated path: the user is already identified from their access token.
public class CreateBookingHandler(IVehicleRepository vehicleRepository, IBookingRepository bookingRepository, IUnitOfWork unitOfWork)
{
    public async Task<Result<CoreResponse<BookingResponse>>> HandleAsync(int userId, CreateBookingRequest request)
    {
        var vehicle = await vehicleRepository.GetByAsync(v => v.QrUniqueCode == request.QrCode);
        if (vehicle is null)
            return BookingErrors.InvalidVehicle;

        if (request.EndTime <= request.StartTime)
            return BookingErrors.InvalidTimeRange;

        var now = DateTime.UtcNow;

        var tripResult = await BookingTripResolver.ResolveAsync(bookingRepository, vehicle.Id, now);
        if (tripResult.IsError)
            return tripResult.Errors;

        var booking = Booking.Create(userId, vehicle.Id, tripResult.Value, request.Purpose, request.StartTime, request.EndTime, now);

        var result = await bookingRepository.AddAsync(booking);
        if (result.IsError)
            return result.Errors;

        await unitOfWork.SaveChangesAsync();

        return new CoreResponse<BookingResponse>(booking.ToResponse(), true, "Booking created successfully.");
    }
}
