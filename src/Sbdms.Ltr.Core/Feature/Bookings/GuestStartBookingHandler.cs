using Microsoft.Extensions.Logging;
using Sbdms.Ltr.Contracts.Booking;
using Sbdms.Ltr.Core.Common.Errors;
using Sbdms.Ltr.Core.Domain;
using Sbdms.Ltr.Core.Interface;
using Sbdms.SharedLibrary.ApiResponse;
using Sbdms.SharedLibrary.Common;
using Sbdms.SharedLibrary.ResultPattern;

namespace Sbdms.Ltr.Core.Feature.Bookings;

// Step 1 of the guest-scan flow: identify (or register) the user by mobile number and send an OTP.
public class GuestStartBookingHandler(
    IVehicleRepository vehicleRepository,
    IUserRepository userRepository,
    IUnitOfWork unitOfWork,
    ILogger<GuestStartBookingHandler> logger)
{
    public async Task<Result<CoreResponse<bool>>> HandleAsync(GuestStartBookingRequest request)
    {
        var vehicle = await vehicleRepository.GetByAsync(v => v.QrUniqueCode == request.QrCode);
        if (vehicle is null)
            return BookingErrors.InvalidVehicle;

        var user = await userRepository.GetByAsync(u => u.MobileNumber == request.MobileNumber);

        if (user is null)
        {
            var duplicateEmployeeCode = await userRepository.GetByAsync(u => u.EmployeeCode == request.EmployeeCode);
            if (duplicateEmployeeCode is not null)
                return UserErrors.DuplicateEmployeeCode;

            user = User.Create(request.MobileNumber, request.Name, request.EmployeeCode, DateTime.UtcNow);

            var addResult = await userRepository.AddAsync(user);
            if (addResult.IsError)
                return addResult.Errors;
        }

        var otp = Random.Shared.Next(100000, 999999).ToString();
        user.SetOtp(otp, DateTime.UtcNow);

        var updateResult = await userRepository.UpdateAsync(user);
        if (updateResult.IsError)
            return updateResult.Errors;

        await unitOfWork.SaveChangesAsync();

        // TODO: send via an actual SMS gateway — never return the OTP in the response.
        logger.LogInformation("OTP for {MobileNumber}: {Otp}", request.MobileNumber, otp);

        return new CoreResponse<bool>(true, true, "OTP sent successfully.");
    }
}
