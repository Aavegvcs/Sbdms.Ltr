namespace Sbdms.Ltr.Infra.Common;

public class JwtOptions
{
    public const string SectionName = "JwtOptions";

    public string Secret { get; init; } = null!;
    public int ExpiryMinutes { get; init; } = 60;
    public string Issuer { get; init; } = null!;
    public string Audience { get; init; } = null!;
}
