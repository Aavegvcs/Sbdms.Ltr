using Sbdms.Ltr.Contracts.User;
using Sbdms.Ltr.Core.Interface;
using Sbdms.SharedLibrary.ApiResponse;
using Sbdms.SharedLibrary.ResultPattern;

namespace Sbdms.Ltr.Core.Feature.Users;

public class GetAllUsersHandler(IUserRepository userRepository)
{
    public async Task<Result<CoreResponse<IEnumerable<UserResponse>>>> HandleAsync()
    {
        var users = await userRepository.GetAllAsync();
        var response = users.Select(u => u.ToResponse());

        return new CoreResponse<IEnumerable<UserResponse>>(response, true, "Users retrieved successfully.");
    }
}
