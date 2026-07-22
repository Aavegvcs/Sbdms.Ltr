using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Options;
using Sbdms.Ltr.Infra.Common;

namespace Sbdms.Ltr.Api.Filters;

// Gates a single action behind a shared-secret header, for machine-to-machine callers (e.g. a
// GPS device) that have no user identity to authenticate as a JWT would require.
public class RequireInternalApiKeyAttribute : Attribute, IAsyncActionFilter
{
    private const string HeaderName = "X-Internal-Api-Key";

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var options = context.HttpContext.RequestServices.GetRequiredService<IOptions<InternalApiKeyOptions>>().Value;

        if (string.IsNullOrEmpty(options.Key) ||
            !context.HttpContext.Request.Headers.TryGetValue(HeaderName, out var providedKey) ||
            !FixedTimeEquals(providedKey.ToString(), options.Key))
        {
            context.Result = new UnauthorizedResult();
            return;
        }

        await next();
    }

    // Ordinary string comparison short-circuits on the first mismatched character, which leaks
    // how many leading characters were guessed correctly via response-time differences.
    private static bool FixedTimeEquals(string provided, string expected)
    {
        var providedBytes = Encoding.UTF8.GetBytes(provided);
        var expectedBytes = Encoding.UTF8.GetBytes(expected);

        if (providedBytes.Length != expectedBytes.Length)
            return false;

        return CryptographicOperations.FixedTimeEquals(providedBytes, expectedBytes);
    }
}
