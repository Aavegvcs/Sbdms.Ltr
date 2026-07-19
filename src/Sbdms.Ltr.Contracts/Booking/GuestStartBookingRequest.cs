namespace Sbdms.Ltr.Contracts.Booking;

// First step of the "scan QR as a new/unrecognized user" flow: identifies (or registers) the
// user by mobile number and sends an OTP. No booking is created yet.
public record GuestStartBookingRequest(
    string QrCode,
    string MobileNumber,
    string Name,
    string EmployeeCode
);
