namespace Sbdms.Ltr.Contracts.User;

public record TokenResponse(
    string AccessToken,
    string RefreshToken
);
