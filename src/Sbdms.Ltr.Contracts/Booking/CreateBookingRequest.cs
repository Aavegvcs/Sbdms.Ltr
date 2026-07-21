namespace Sbdms.Ltr.Contracts.Booking;

// Used by the authenticated path — the user is identified from the access token, not the body.
public record CreateBookingRequest(
    string QrCode,
    string? Purpose,
    decimal PickLatitude,
    decimal PickLongitude,
    decimal DropLatitude,
    decimal DropLongitude,
    DateTime StartTime,
    DateTime EndTime
);
