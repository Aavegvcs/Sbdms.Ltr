namespace Sbdms.Ltr.Contracts.Booking;

public record BookingResponse(
    int Id,
    int UserId,
    int VehicleId,
    int TripId,
    BookingStatus Status,
    string? Purpose,
    DateTime StartTime,
    DateTime EndTime,
    DateTime BookedOn,
    DateTime? ModifiedOn
);
