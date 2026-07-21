using Sbdms.SharedLibrary.Common;

namespace Sbdms.Ltr.Core.Domain;

public class User : AggregateRoot<int>
{
    public User() { }

    private User(string mobileNumber, string name, string employeeCode, DateTime createdOn)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(mobileNumber);
        ArgumentException.ThrowIfNullOrWhiteSpace(name);

        if (mobileNumber.Length > 15)
            throw new ArgumentException("MobileNumber cannot exceed 15 characters", nameof(mobileNumber));

        if (name.Length > 100)
            throw new ArgumentException("Name cannot exceed 100 characters", nameof(name));

        MobileNumber = mobileNumber;
        Name = name;
        EmployeeCode = employeeCode;
        CreatedOn = createdOn;
    }

    public string MobileNumber { get; private set; } = null!;
    public string Name { get; private set; } = null!;
    public string EmployeeCode { get; private set; } = null!;
    public string? Otp { get; private set; }
    public DateTime? OtpGeneratedOn { get; private set; }
    public string? AccessToken { get; private set; }
    public string? RefreshToken { get; private set; }
    public DateTime? RefreshTokenGeneratedOn { get; private set; }
    public DateTime CreatedOn { get; private set; }
    public DateTime? ModifiedOn { get; private set; }

    public static User Create(string mobileNumber, string name, string employeeCode, DateTime createdOn) =>
        new(mobileNumber, name, employeeCode, createdOn);

    public void SetOtp(string otp, DateTime generatedOn)
    {
        Otp = otp;
        OtpGeneratedOn = generatedOn;
        ModifiedOn = generatedOn;
    }

    public void SetTokens(string accessToken, string refreshToken, DateTime generatedOn)
    {
        AccessToken = accessToken;
        RefreshToken = refreshToken;
        RefreshTokenGeneratedOn = generatedOn;
        Otp = null;
        OtpGeneratedOn = null;
        ModifiedOn = generatedOn;
    }
}
