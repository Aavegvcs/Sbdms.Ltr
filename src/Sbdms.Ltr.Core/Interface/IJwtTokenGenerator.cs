using Sbdms.Ltr.Core.Domain;

namespace Sbdms.Ltr.Core.Interface;

public interface IJwtTokenGenerator
{
    string GenerateAccessToken(User user);
    string GenerateRefreshToken();
}
