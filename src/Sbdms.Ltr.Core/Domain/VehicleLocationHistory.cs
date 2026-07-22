using Sbdms.SharedLibrary.Common;

namespace Sbdms.Ltr.Core.Domain;

// Append-only: one row per reported position, forever. Companion to VehicleLocation (which
// holds only the latest position for O(1) lookups) — this is the full track/history log.
public class VehicleLocationHistory : AggregateRoot<int>
{
    public VehicleLocationHistory() { }

    private VehicleLocationHistory(int vehicleId, decimal latitude, decimal longitude, DateTime recordedOn)
    {
        ValidateLatitude(latitude);
        ValidateLongitude(longitude);

        VehicleId = vehicleId;
        Latitude = latitude;
        Longitude = longitude;
        RecordedOn = recordedOn;
    }

    public int VehicleId { get; private set; }
    public decimal Latitude { get; private set; }
    public decimal Longitude { get; private set; }
    public DateTime RecordedOn { get; private set; }

    public static VehicleLocationHistory Create(int vehicleId, decimal latitude, decimal longitude, DateTime recordedOn) =>
        new(vehicleId, latitude, longitude, recordedOn);

    private static void ValidateLatitude(decimal value)
    {
        if (value < -90 || value > 90)
            throw new ArgumentException("Latitude must be between -90 and 90", nameof(value));
    }

    private static void ValidateLongitude(decimal value)
    {
        if (value < -180 || value > 180)
            throw new ArgumentException("Longitude must be between -180 and 180", nameof(value));
    }
}
