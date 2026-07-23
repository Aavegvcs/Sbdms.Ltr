using Sbdms.Ltr.Contracts.User;
using Sbdms.Ltr.Core.Common.Errors;
using Sbdms.Ltr.Core.Interface;
using Sbdms.SharedLibrary.ApiResponse;
using Sbdms.SharedLibrary.Common;
using Sbdms.SharedLibrary.ResultPattern;

using Sbdms.Ltr.Core.Common.Helper;
namespace Sbdms.Ltr.Core.Feature.Users;

public class RefreshTokenHandler(IUserRepository userRepository, IJwtTokenGenerator jwtTokenGenerator, IUnitOfWork unitOfWork)
{
    private const int RefreshTokenExpiryDays = 30;

    public async Task<Result<CoreResponse<TokenResponse>>> HandleAsync(RefreshTokenRequest request)
    {
        var user = await userRepository.GetByAsync(u => u.RefreshToken == request.RefreshToken);
        if (user is null)
            return UserErrors.InvalidRefreshToken;

        if (user.RefreshTokenGeneratedOn is null ||
            user.RefreshTokenGeneratedOn.Value.AddDays(RefreshTokenExpiryDays) < IndianStandardTime.Now)
            return UserErrors.InvalidRefreshToken;

        var accessToken = jwtTokenGenerator.GenerateAccessToken(user);
        var refreshToken = jwtTokenGenerator.GenerateRefreshToken();

        user.SetTokens(accessToken, refreshToken, IndianStandardTime.Now);

        var result = await userRepository.UpdateAsync(user);
        if (result.IsError)
            return result.Errors;

        await unitOfWork.SaveChangesAsync();

        return new CoreResponse<TokenResponse>(new TokenResponse(accessToken, refreshToken), true, "Token refreshed successfully.");
    }
}
