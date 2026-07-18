using Sbdms.SharedLibrary.ResultPattern;

namespace Sbdms.Ltr.Core.Common.Errors;

public static class VehicleErrors
{
    public static readonly Error VehicleNotFound =
        Error.NotFound("Vehicle.NotFound", "Vehicle was not found.");

    public static readonly Error DuplicateQrCode =
        Error.Conflict("Vehicle.DuplicateQrCode", "A vehicle with this QR code already exists.");

    public static readonly Error InvalidVehicleType =
        Error.Validation("Vehicle.InvalidVehicleType", "The specified vehicle type does not exist.");

    public static readonly Error InvalidCurrentStatus =
        Error.Validation("Vehicle.InvalidCurrentStatus", "The specified current status does not exist.");

    public static readonly Error InvalidDriver =
        Error.Validation("Vehicle.InvalidDriver", "The specified driver does not exist.");
}
