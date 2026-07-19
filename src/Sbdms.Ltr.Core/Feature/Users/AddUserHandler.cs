using Sbdms.Ltr.Contracts.User;
using Sbdms.Ltr.Core.Common.Errors;
using Sbdms.Ltr.Core.Domain;
using Sbdms.Ltr.Core.Interface;
using Sbdms.SharedLibrary.ApiResponse;
using Sbdms.SharedLibrary.Common;
using Sbdms.SharedLibrary.ResultPattern;

namespace Sbdms.Ltr.Core.Feature.Users;

public class AddUserHandler(IUserRepository userRepository, IUnitOfWork unitOfWork)
{
    public async Task<Result<CoreResponse<UserResponse>>> HandleAsync(AddUserRequest request)
    {
        var duplicateEmployeeCode = await userRepository.GetByAsync(u => u.EmployeeCode == request.EmployeeCode);
        if (duplicateEmployeeCode is not null)
            return UserErrors.DuplicateEmployeeCode;

        var user = User.Create(request.MobileNumber, request.Name, request.EmployeeCode, DateTime.UtcNow);

        var result = await userRepository.AddAsync(user);
        if (result.IsError)
            return result.Errors;

        await unitOfWork.SaveChangesAsync();

        return new CoreResponse<UserResponse>(user.ToResponse(), true, "User added successfully.");
    }
}
