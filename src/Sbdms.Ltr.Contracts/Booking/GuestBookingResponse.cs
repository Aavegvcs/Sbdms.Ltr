namespace Sbdms.Ltr.Contracts.Booking;

public record GuestBookingResponse(
    BookingResponse Booking,
    string AccessToken,
    string RefreshToken
);
