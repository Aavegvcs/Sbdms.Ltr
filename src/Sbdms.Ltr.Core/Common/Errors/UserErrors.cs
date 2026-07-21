using Sbdms.SharedLibrary.ResultPattern;

namespace Sbdms.Ltr.Core.Common.Errors;

public static class UserErrors
{
    public static readonly Error UserNotFound =
        Error.NotFound("User.NotFound", "User was not found.");

    public static readonly Error DuplicateMobileNumber =
        Error.Conflict("User.DuplicateMobileNumber", "A user with this mobile number already exists.");

    public static readonly Error OtpNotRequested =
        Error.Validation("User.OtpNotRequested", "No OTP has been requested for this mobile number.");

    public static readonly Error OtpExpired =
        Error.Validation("User.OtpExpired", "The OTP has expired.");

    public static readonly Error InvalidOtp =
        Error.Validation("User.InvalidOtp", "The OTP provided is incorrect.");

    public static readonly Error InvalidRefreshToken =
        Error.Unauthorized("User.InvalidRefreshToken", "The refresh token is invalid or has expired.");
}
