using Sbdms.SharedLibrary.ResultPattern;

namespace Sbdms.Ltr.Core.Common.Errors;

public static class CurrentStatusErrors
{
    public static readonly Error CurrentStatusNotFound =
        Error.NotFound("CurrentStatus.NotFound", "Current status was not found.");

    public static readonly Error DuplicateStatusName =
        Error.Conflict("CurrentStatus.DuplicateName", "A current status with this name already exists.");
}
