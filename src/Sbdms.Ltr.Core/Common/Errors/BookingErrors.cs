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

    public static readonly Error NotYourBooking =
        Error.Forbidden("Booking.NotYourBooking", "This booking does not belong to you.");

    public static readonly Error AlreadyCompleted =
        Error.Conflict("Booking.AlreadyCompleted", "This booking is already completed.");
}
