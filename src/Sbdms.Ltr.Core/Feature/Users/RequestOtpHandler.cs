using Sbdms.Ltr.Contracts.User;
using Sbdms.Ltr.Core.Common.Errors;
using Sbdms.Ltr.Core.Domain;
using Sbdms.Ltr.Core.Interface;
using Sbdms.SharedLibrary.ApiResponse;
using Sbdms.SharedLibrary.Common;
using Sbdms.SharedLibrary.ResultPattern;

namespace Sbdms.Ltr.Core.Feature.Users;

public class RequestOtpHandler(IUserRepository userRepository, IUnitOfWork unitOfWork)
{
    public async Task<Result<CoreResponse<string>>> HandleAsync(RequestOtpRequest request)
    {
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

        // Dev/test convenience: the OTP is returned directly instead of being sent via SMS.
        return new CoreResponse<string>(otp, true, "OTP generated successfully.");
    }
}
