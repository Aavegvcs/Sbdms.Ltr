using Sbdms.Ltr.Contracts.User;
using Sbdms.Ltr.Core.Domain;

namespace Sbdms.Ltr.Core.Feature.Users;

public static class UserMappingExtensions
{
    public static UserResponse ToResponse(this User user) =>
        new(
            user.Id,
            user.MobileNumber,
            user.Name,
            user.EmployeeCode,
            user.CreatedOn,
            user.ModifiedOn
        );
}
