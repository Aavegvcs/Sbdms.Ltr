namespace Sbdms.Ltr.Contracts.Booking;

public record BookingResponse(
    int Id,
    int UserId,
    int VehicleId,
    int TripId,
    string VehicleNumber,
    string Modal,
    string? DriverNumber,
    string? DriverName,
    decimal PickLatitude,
    decimal PickLongitude,
    decimal DropLatitude,
    decimal DropLongitude,
    BookingStatus Status,
    string? Purpose,
    DateTime StartTime,
    DateTime EndTime,
    DateTime BookedOn,
    DateTime? ModifiedOn
);
