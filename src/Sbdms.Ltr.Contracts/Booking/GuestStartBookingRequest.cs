namespace Sbdms.Ltr.Contracts.Booking;

// "Scan QR as a new/unrecognized user" flow: identifies (or registers) the user by mobile
// number and creates the booking in the same call, no OTP step.
public record GuestStartBookingRequest(
    string QrCode,
    string MobileNumber,
    string Name,
    string EmployeeCode,
    string? Purpose,
    decimal PickLatitude,
    decimal PickLongitude,
    decimal DropLatitude,
    decimal DropLongitude,
    DateTime StartTime,
    DateTime EndTime
);
