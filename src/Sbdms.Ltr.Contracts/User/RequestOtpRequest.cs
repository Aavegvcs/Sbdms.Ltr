namespace Sbdms.Ltr.Contracts.User;

// If MobileNumber isn't already registered, a new User is created using Name/EmployeeCode
// (register-on-first-login) — otherwise Name/EmployeeCode are ignored and the OTP is just (re)issued.
public record RequestOtpRequest(
    string MobileNumber,
    string Name,
    string EmployeeCode
);
