using Sbdms.SharedLibrary.Common;

namespace Sbdms.Ltr.Core.Domain;

// One row per vehicle — always the vehicle's latest known position, upserted on every report.
// Not a location history/track log; that's a different feature if it's ever needed.
public class VehicleLocation : AggregateRoot<int>
{
    public VehicleLocation() { }

    private VehicleLocation(int vehicleId, decimal latitude, decimal longitude, DateTime recordedOn)
    {
        ValidateLatitude(latitude);
        ValidateLongitude(longitude);

        VehicleId = vehicleId;
        Latitude = latitude;
        Longitude = longitude;
        RecordedOn = recordedOn;
        CreatedOn = recordedOn;
    }

    public int VehicleId { get; private set; }
    public decimal Latitude { get; private set; }
    public decimal Longitude { get; private set; }

    // When this position was reported — distinct from ModifiedOn, which is a pure row-audit
    // timestamp; RecordedOn is the value callers actually care about.
    public DateTime RecordedOn { get; private set; }

    public DateTime CreatedOn { get; private set; }
    public DateTime? ModifiedOn { get; private set; }

    public static VehicleLocation Create(int vehicleId, decimal latitude, decimal longitude, DateTime recordedOn) =>
        new(vehicleId, latitude, longitude, recordedOn);

    public void UpdatePosition(decimal latitude, decimal longitude, DateTime recordedOn)
    {
        ValidateLatitude(latitude);
        ValidateLongitude(longitude);

        Latitude = latitude;
        Longitude = longitude;
        RecordedOn = recordedOn;
        ModifiedOn = recordedOn;
    }

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
