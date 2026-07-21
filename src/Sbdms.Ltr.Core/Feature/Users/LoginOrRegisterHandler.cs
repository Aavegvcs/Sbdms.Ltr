using Sbdms.Ltr.Contracts.User;
using Sbdms.Ltr.Core.Domain;
using Sbdms.Ltr.Core.Interface;
using Sbdms.SharedLibrary.ApiResponse;
using Sbdms.SharedLibrary.Common;
using Sbdms.SharedLibrary.ResultPattern;

namespace Sbdms.Ltr.Core.Feature.Users;

// Finds the user by mobile number and logs them in; if none exists, registers one from
// Name/EmployeeCode first. Either way, returns a fresh access/refresh token pair.
public class LoginOrRegisterHandler(IUserRepository userRepository, IJwtTokenGenerator jwtTokenGenerator, IUnitOfWork unitOfWork)
{
    public async Task<Result<CoreResponse<TokenResponse>>> HandleAsync(LoginOrRegisterRequest request)
    {
        var user = await userRepository.GetByAsync(u => u.MobileNumber == request.MobileNumber);

        if (user is null)
        {
            user = User.Create(request.MobileNumber, request.Name, request.EmployeeCode, DateTime.UtcNow);

            var addResult = await userRepository.AddAsync(user);
            if (addResult.IsError)
                return addResult.Errors;

            // Flush now so user.Id is actually assigned before it's baked into the token below.
            await unitOfWork.SaveChangesAsync();
        }

        var now = DateTime.UtcNow;
        var accessToken = jwtTokenGenerator.GenerateAccessToken(user);
        var refreshToken = jwtTokenGenerator.GenerateRefreshToken();
        user.SetTokens(accessToken, refreshToken, now);

        var updateResult = await userRepository.UpdateAsync(user);
        if (updateResult.IsError)
            return updateResult.Errors;

        await unitOfWork.SaveChangesAsync();

        return new CoreResponse<TokenResponse>(new TokenResponse(accessToken, refreshToken), true, "Login successful.");
    }
}
