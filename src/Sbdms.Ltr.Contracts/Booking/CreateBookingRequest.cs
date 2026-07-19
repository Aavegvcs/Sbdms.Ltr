namespace Sbdms.Ltr.Contracts.Booking;

// Used by the authenticated path — the user is identified from the access token, not the body.
public record CreateBookingRequest(
    string QrCode,
    string? Purpose,
    DateTime StartTime,
    DateTime EndTime
);
