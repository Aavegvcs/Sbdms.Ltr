using Sbdms.Ltr.Contracts.User;
using Sbdms.Ltr.Core.Common.Errors;
using Sbdms.Ltr.Core.Interface;
using Sbdms.SharedLibrary.ApiResponse;
using Sbdms.SharedLibrary.Common;
using Sbdms.SharedLibrary.ResultPattern;

namespace Sbdms.Ltr.Core.Feature.Users;

public class VerifyOtpHandler(IUserRepository userRepository, IJwtTokenGenerator jwtTokenGenerator, IUnitOfWork unitOfWork)
{
    private const int OtpExpiryMinutes = 5;

    public async Task<Result<CoreResponse<TokenResponse>>> HandleAsync(VerifyOtpRequest request)
    {
        var user = await userRepository.GetByAsync(u => u.MobileNumber == request.MobileNumber);
        if (user is null)
            return UserErrors.UserNotFound;

        if (string.IsNullOrEmpty(user.Otp) || user.OtpGeneratedOn is null)
            return UserErrors.OtpNotRequested;

        if (DateTime.UtcNow > user.OtpGeneratedOn.Value.AddMinutes(OtpExpiryMinutes))
            return UserErrors.OtpExpired;

        if (user.Otp != request.Otp)
            return UserErrors.InvalidOtp;

        var accessToken = jwtTokenGenerator.GenerateAccessToken(user);
        var refreshToken = jwtTokenGenerator.GenerateRefreshToken();

        user.SetTokens(accessToken, refreshToken, DateTime.UtcNow);

        var result = await userRepository.UpdateAsync(user);
        if (result.IsError)
            return result.Errors;

        await unitOfWork.SaveChangesAsync();

        return new CoreResponse<TokenResponse>(new TokenResponse(accessToken, refreshToken), true, "Login successful.");
    }
}
