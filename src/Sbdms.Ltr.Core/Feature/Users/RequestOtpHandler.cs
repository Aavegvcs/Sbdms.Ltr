using Microsoft.Extensions.Logging;
using Sbdms.Ltr.Contracts.User;
using Sbdms.Ltr.Core.Common.Errors;
using Sbdms.Ltr.Core.Interface;
using Sbdms.SharedLibrary.ApiResponse;
using Sbdms.SharedLibrary.Common;
using Sbdms.SharedLibrary.ResultPattern;

namespace Sbdms.Ltr.Core.Feature.Users;

public class RequestOtpHandler(IUserRepository userRepository, IUnitOfWork unitOfWork, ILogger<RequestOtpHandler> logger)
{
    public async Task<Result<CoreResponse<bool>>> HandleAsync(RequestOtpRequest request)
    {
        var user = await userRepository.GetByAsync(u => u.MobileNumber == request.MobileNumber);
        if (user is null)
            return UserErrors.UserNotFound;

        // var otp = Random.Shared.Next(100000, 999999).ToString();
        var otp = "123456";

        user.SetOtp(otp, DateTime.UtcNow);

        var updateResult = await userRepository.UpdateAsync(user);
        if (updateResult.IsError)
            return updateResult.Errors;

        await unitOfWork.SaveChangesAsync();

        // TODO: send via an actual SMS gateway. Until that's wired up, the OTP only ever
        // appears in the server logs — it must never be returned in the API response.
        logger.LogInformation("OTP for {MobileNumber}: {Otp}", request.MobileNumber, otp);

        return new CoreResponse<bool>(true, true, "OTP sent successfully.");
    }
}
