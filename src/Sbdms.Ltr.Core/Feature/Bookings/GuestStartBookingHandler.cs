using Sbdms.Ltr.Contracts.Booking;
using Sbdms.Ltr.Core.Common.Errors;
using Sbdms.Ltr.Core.Domain;
using Sbdms.Ltr.Core.Interface;
using Sbdms.SharedLibrary.ApiResponse;
using Sbdms.SharedLibrary.Common;
using Sbdms.SharedLibrary.ResultPattern;

namespace Sbdms.Ltr.Core.Feature.Bookings;

// "Scan QR as a new/unrecognized user" flow: identify (or register) the user by mobile number,
// log them in, and create the booking — all in one call, no OTP.
public class GuestStartBookingHandler(
    IVehicleRepository vehicleRepository,
    IUserRepository userRepository,
    IBookingRepository bookingRepository,
    IJwtTokenGenerator jwtTokenGenerator,
    IUnitOfWork unitOfWork)
{
    public async Task<Result<CoreResponse<GuestBookingResponse>>> HandleAsync(GuestStartBookingRequest request)
    {
        var vehicle = await vehicleRepository.GetByAsync(v => v.QrUniqueCode == request.QrCode);
        if (vehicle is null)
            return BookingErrors.InvalidVehicle;

        if (request.EndTime <= request.StartTime)
            return BookingErrors.InvalidTimeRange;

        var user = await userRepository.GetByAsync(u => u.MobileNumber == request.MobileNumber);

        if (user is null)
        {
            user = User.Create(request.MobileNumber, request.Name, request.EmployeeCode, DateTime.UtcNow);

            var addResult = await userRepository.AddAsync(user);
            if (addResult.IsError)
                return addResult.Errors;
        }

        var now = DateTime.UtcNow;

        var tripResult = await BookingTripResolver.ResolveAsync(bookingRepository, vehicle.Id, now);
        if (tripResult.IsError)
            return tripResult.Errors;

        var accessToken = jwtTokenGenerator.GenerateAccessToken(user);
        var refreshToken = jwtTokenGenerator.GenerateRefreshToken();
        user.SetTokens(accessToken, refreshToken, now);

        var userUpdateResult = await userRepository.UpdateAsync(user);
        if (userUpdateResult.IsError)
            return userUpdateResult.Errors;

        var booking = Booking.Create(user.Id, vehicle.Id, tripResult.Value, request.Purpose, request.StartTime, request.EndTime, now);

        var bookingResult = await bookingRepository.AddAsync(booking);
        if (bookingResult.IsError)
            return bookingResult.Errors;

        await unitOfWork.SaveChangesAsync();

        return new CoreResponse<GuestBookingResponse>(
            new GuestBookingResponse(booking.ToResponse(), accessToken, refreshToken),
            true,
            "Booking confirmed successfully.");
    }
}
