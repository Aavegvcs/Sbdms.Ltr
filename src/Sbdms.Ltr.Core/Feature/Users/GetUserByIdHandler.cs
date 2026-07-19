using Sbdms.Ltr.Contracts.User;
using Sbdms.Ltr.Core.Interface;
using Sbdms.SharedLibrary.ApiResponse;
using Sbdms.SharedLibrary.ResultPattern;

namespace Sbdms.Ltr.Core.Feature.Users;

public class GetUserByIdHandler(IUserRepository userRepository)
{
    public async Task<Result<CoreResponse<UserResponse>>> HandleAsync(int id)
    {
        var result = await userRepository.FindByIdAsync(id);
        if (result.IsError)
            return result.Errors;

        return new CoreResponse<UserResponse>(result.Value.ToResponse(), true, "User retrieved successfully.");
    }
}
