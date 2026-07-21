using Sbdms.SharedLibrary.ResultPattern;

namespace Sbdms.Ltr.Core.Common.Errors;

public static class DriverErrors
{
    public static readonly Error DriverNotFound =
        Error.NotFound("Driver.NotFound", "Driver was not found.");

    public static readonly Error DuplicateLicenceNumber =
        Error.Conflict("Driver.DuplicateLicenceNumber", "A driver with this licence number already exists.");

    public static readonly Error InvalidCurrentStatus =
        Error.Validation("Driver.InvalidCurrentStatus", "The specified current status does not exist.");

    public static readonly Error InvalidVendor =
        Error.Validation("Driver.InvalidVendor", "The specified vendor does not exist.");
}
