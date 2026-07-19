namespace Sbdms.Ltr.Contracts.Booking;

// Second step: verifies the OTP, logs the user in (issues tokens), and creates the booking.
public record GuestConfirmBookingRequest(
    string QrCode,
    string MobileNumber,
    string Otp,
    string? Purpose,
    DateTime StartTime,
    DateTime EndTime
);
