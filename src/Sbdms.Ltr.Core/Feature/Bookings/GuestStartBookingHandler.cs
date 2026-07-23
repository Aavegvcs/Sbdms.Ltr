using Sbdms.Ltr.Contracts.Booking;
using Sbdms.Ltr.Core.Common.Errors;
using Sbdms.Ltr.Core.Domain;
using Sbdms.Ltr.Core.Interface;
using Sbdms.SharedLibrary.ApiResponse;
using Sbdms.SharedLibrary.Common;
using Sbdms.SharedLibrary.ResultPattern;

using Sbdms.Ltr.Core.Common.Helper;
namespace Sbdms.Ltr.Core.Feature.Bookings;

// "Scan QR as a new/unrecognized user" flow: identify (or register) the user by mobile number,
// log them in, and create the booking — all in one call, no OTP.
public class GuestStartBookingHandler(
    IVehicleRepository vehicleRepository,
    IDriverRepository driverRepository,
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

        var driver = vehicle.DriverId is not null
            ? await driverRepository.GetByAsync(d => d.Id == vehicle.DriverId)
            : null;

        var user = await userRepository.GetByAsync(u => u.MobileNumber == request.MobileNumber);

        if (user is null)
        {
            user = User.Create(request.MobileNumber, request.Name, request.EmployeeCode, IndianStandardTime.Now);

            var addResult = await userRepository.AddAsync(user);
            if (addResult.IsError)
                return addResult.Errors;

            // Flush now so user.Id is actually assigned before it's used as the booking's
            // UserId (and baked into the token) below.
            await unitOfWork.SaveChangesAsync();
        }

        var now = IndianStandardTime.Now;

        var tripResult = await BookingTripResolver.ResolveAsync(bookingRepository, vehicle.Id, now);
        if (tripResult.IsError)
            return tripResult.Errors;

        var accessToken = jwtTokenGenerator.GenerateAccessToken(user);
        var refreshToken = jwtTokenGenerator.GenerateRefreshToken();
        user.SetTokens(accessToken, refreshToken, now);

        var userUpdateResult = await userRepository.UpdateAsync(user);
        if (userUpdateResult.IsError)
            return userUpdateResult.Errors;

        var booking = Booking.Create(
            user.Id,
            vehicle.Id,
            tripResult.Value,
            vehicle.VehicleNumber,
            vehicle.Modal,
            driver?.DriverNumber,
            driver?.DriverName,
            request.PickLatitude,
            request.PickLongitude,
            request.DropLatitude,
            request.DropLongitude,
            request.Purpose,
            request.StartTime,
            request.EndTime,
            now);

        var bookingResult = await bookingRepository.AddAsync(booking);
        if (bookingResult.IsError)
            return bookingResult.Errors;

        await unitOfWork.SaveChangesAsync();

        if (tripResult.Value is null)
        {
            // Fresh trip: booking.Id is only known now, so stamp TripId with it in a second save.
            // booking is still tracked from AddAsync, so EF picks up this change on its own.
            booking.AssignAsTripHead();
            await unitOfWork.SaveChangesAsync();
        }

        return new CoreResponse<GuestBookingResponse>(
            new GuestBookingResponse(booking.ToResponse(), accessToken, refreshToken),
            true,
            "Booking confirmed successfully.");
    }
}
