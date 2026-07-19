using Sbdms.Ltr.Contracts.Booking;
using Sbdms.Ltr.Core.Common.Errors;
using Sbdms.Ltr.Core.Domain;
using Sbdms.Ltr.Core.Interface;
using Sbdms.SharedLibrary.ApiResponse;
using Sbdms.SharedLibrary.Common;
using Sbdms.SharedLibrary.ResultPattern;

namespace Sbdms.Ltr.Core.Feature.Bookings;

// Step 2 of the guest-scan flow: verify the OTP, log the user in, and create the booking.
public class GuestConfirmBookingHandler(
    IVehicleRepository vehicleRepository,
    IUserRepository userRepository,
    IBookingRepository bookingRepository,
    IJwtTokenGenerator jwtTokenGenerator,
    IUnitOfWork unitOfWork)
{
    private const int OtpExpiryMinutes = 5;

    public async Task<Result<CoreResponse<GuestBookingResponse>>> HandleAsync(GuestConfirmBookingRequest request)
    {
        var vehicle = await vehicleRepository.GetByAsync(v => v.QrUniqueCode == request.QrCode);
        if (vehicle is null)
            return BookingErrors.InvalidVehicle;

        if (request.EndTime <= request.StartTime)
            return BookingErrors.InvalidTimeRange;

        var now = DateTime.UtcNow;

        // Resolve trip pooling/occupied/fresh before touching the OTP — a rejected "occupied"
        // attempt shouldn't consume the rider's OTP.
        var tripResult = await BookingTripResolver.ResolveAsync(bookingRepository, vehicle.Id, now);
        if (tripResult.IsError)
            return tripResult.Errors;

        var user = await userRepository.GetByAsync(u => u.MobileNumber == request.MobileNumber);
        if (user is null)
            return UserErrors.UserNotFound;

        if (string.IsNullOrEmpty(user.Otp) || user.OtpGeneratedOn is null)
            return UserErrors.OtpNotRequested;

        if (now > user.OtpGeneratedOn.Value.AddMinutes(OtpExpiryMinutes))
            return UserErrors.OtpExpired;

        if (user.Otp != request.Otp)
            return UserErrors.InvalidOtp;

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
