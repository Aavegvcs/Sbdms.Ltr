namespace Sbdms.Ltr.Contracts.User;

// Deliberately excludes Otp/AccessToken/RefreshToken — those are sensitive and only
// ever returned directly from the login/refresh endpoints that issued them.
public record UserResponse(
    int Id,
    string MobileNumber,
    string Name,
    string EmployeeCode,
    DateTime CreatedOn,
    DateTime? ModifiedOn
);
