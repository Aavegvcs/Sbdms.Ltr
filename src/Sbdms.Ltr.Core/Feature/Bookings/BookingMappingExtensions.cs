using Sbdms.Ltr.Contracts.Booking;
using Sbdms.Ltr.Core.Domain;

namespace Sbdms.Ltr.Core.Feature.Bookings;

public static class BookingMappingExtensions
{
    public static BookingResponse ToResponse(this Booking booking) =>
        new(
            booking.Id,
            booking.UserId,
            booking.VehicleId,
            booking.TripId ?? booking.Id,
            booking.VehicleNumber,
            booking.Modal,
            booking.DriverNumber,
            booking.DriverName,
            booking.PickLatitude,
            booking.PickLongitude,
            booking.DropLatitude,
            booking.DropLongitude,
            booking.Status,
            booking.Purpose,
            booking.StartTime,
            booking.EndTime,
            booking.BookedOn,
            booking.ModifiedOn
        );
}
