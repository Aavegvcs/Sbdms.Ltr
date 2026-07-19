namespace Sbdms.Ltr.Contracts.User;

public record VerifyOtpRequest(
    string MobileNumber,
    string Otp
);
