namespace Sbdms.Ltr.Contracts.User;

// If MobileNumber already belongs to a user, Name/EmployeeCode are ignored and that
// existing user is just logged in. Otherwise a new user is created from all three fields.
public record LoginOrRegisterRequest(
    string MobileNumber,
    string Name,
    string EmployeeCode
);
