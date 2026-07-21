using Sbdms.Ltr.Contracts.User;
using Sbdms.Ltr.Core.Common.Errors;
using Sbdms.Ltr.Core.Interface;
using Sbdms.SharedLibrary.ApiResponse;
using Sbdms.SharedLibrary.ResultPattern;

namespace Sbdms.Ltr.Core.Feature.Users;

public class GetUserByMobileNumberHandler(IUserRepository userRepository)
{
    public async Task<Result<CoreResponse<UserLookupResponse>>> HandleAsync(string mobileNumber)
    {
        var user = await userRepository.GetByAsync(u => u.MobileNumber == mobileNumber);
        if (user is null)
            return UserErrors.UserNotFound;

        var response = new UserLookupResponse(user.EmployeeCode, user.Name);

        return new CoreResponse<UserLookupResponse>(response, true, "User retrieved successfully.");
    }
}
