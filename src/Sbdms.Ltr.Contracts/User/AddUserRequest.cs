namespace Sbdms.Ltr.Contracts.User;

public record AddUserRequest(
    string MobileNumber,
    string Name,
    string EmployeeCode
);
