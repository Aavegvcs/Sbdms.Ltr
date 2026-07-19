using Sbdms.SharedLibrary.ResultPattern;

namespace Sbdms.Ltr.Core.Common.Errors;

public static class VehicleTypeErrors
{
    public static readonly Error VehicleTypeNotFound =
        Error.NotFound("VehicleType.NotFound", "Vehicle type was not found.");

    public static readonly Error DuplicateVehicleTypeDesc =
        Error.Conflict("VehicleType.DuplicateDesc", "A vehicle type with this description already exists.");
}
