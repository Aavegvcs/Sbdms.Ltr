using Sbdms.SharedLibrary.Common;

namespace Sbdms.Ltr.Core.Domain;

public class Vehicle : AggregateRoot<int>
{
    public Vehicle() { }

    private Vehicle(
        int vehicleTypeCode,
        int? driverId,
        int currentStatusId,
        string qrUniqueCode,
        DateTime createdOn)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(qrUniqueCode);

        if (qrUniqueCode.Length > 100)
            throw new ArgumentException("QrUniqueCode cannot exceed 100 characters", nameof(qrUniqueCode));

        VehicleTypeCode = vehicleTypeCode;
        DriverId = driverId;
        CurrentStatusId = currentStatusId;
        QrUniqueCode = qrUniqueCode;
        CreatedOn = createdOn;
    }

    public int VehicleTypeCode { get; private set; }
    public int? DriverId { get; private set; }
    public int CurrentStatusId { get; private set; }
    public string QrUniqueCode { get; private set; } = null!;
    public DateTime CreatedOn { get; private set; }
    public DateTime? ModifiedOn { get; private set; }

    public static Vehicle Create(int vehicleTypeCode, int? driverId, int currentStatusId, string qrUniqueCode, DateTime createdOn) =>
        new(vehicleTypeCode, driverId, currentStatusId, qrUniqueCode, createdOn);

    public void Update(int vehicleTypeCode, int? driverId, int currentStatusId, string qrUniqueCode, DateTime modifiedOn)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(qrUniqueCode);

        if (qrUniqueCode.Length > 100)
            throw new ArgumentException("QrUniqueCode cannot exceed 100 characters", nameof(qrUniqueCode));

        VehicleTypeCode = vehicleTypeCode;
        DriverId = driverId;
        CurrentStatusId = currentStatusId;
        QrUniqueCode = qrUniqueCode;
        ModifiedOn = modifiedOn;
    }
}
