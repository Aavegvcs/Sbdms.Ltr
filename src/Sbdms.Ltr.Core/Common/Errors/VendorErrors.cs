using Sbdms.SharedLibrary.ResultPattern;

namespace Sbdms.Ltr.Core.Common.Errors;

public static class VendorErrors
{
    public static readonly Error VendorNotFound =
        Error.NotFound("Vendor.NotFound", "Vendor was not found.");
}
