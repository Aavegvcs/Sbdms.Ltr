using Sbdms.SharedLibrary.ResultPattern;

namespace Sbdms.Ltr.Core.Common.Errors;

public static class BookingErrors
{
    public static readonly Error BookingNotFound =
        Error.NotFound("Booking.NotFound", "Booking was not found.");

    public static readonly Error InvalidVehicle =
        Error.NotFound("Booking.InvalidVehicle", "No vehicle was found for the scanned QR code.");

    public static readonly Error InvalidTimeRange =
        Error.Validation("Booking.InvalidTimeRange", "EndTime must be after StartTime.");

    public static readonly Error VehicleOccupied =
        Error.Conflict("Booking.VehicleOccupied", "This vehicle is currently occupied. Please try again shortly.");
}
