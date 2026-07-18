using Sbdms.SharedLibrary.Common;

namespace Sbdms.Ltr.Core.Domain;

public class Driver : AggregateRoot<int>
{
    public Driver() { }

    private Driver(
        string driverName,
        string driverNumber,
        DateOnly dob,
        string licenceNumber,
        int currentStatusId,
        DateTime createdOn)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(driverName);
        ArgumentException.ThrowIfNullOrWhiteSpace(driverNumber);
        ArgumentException.ThrowIfNullOrWhiteSpace(licenceNumber);

        if (driverName.Length > 100)
            throw new ArgumentException("DriverName cannot exceed 100 characters", nameof(driverName));

        if (driverNumber.Length > 20)
            throw new ArgumentException("DriverNumber cannot exceed 20 characters", nameof(driverNumber));

        if (licenceNumber.Length > 50)
            throw new ArgumentException("LicenceNumber cannot exceed 50 characters", nameof(licenceNumber));

        DriverName = driverName;
        DriverNumber = driverNumber;
        Dob = dob;
        LicenceNumber = licenceNumber;
        CurrentStatusId = currentStatusId;
        CreatedOn = createdOn;
    }

    public string DriverName { get; private set; } = null!;
    public string DriverNumber { get; private set; } = null!;
    public DateOnly Dob { get; private set; }
    public string LicenceNumber { get; private set; } = null!;
    public int CurrentStatusId { get; private set; }
    public DateTime CreatedOn { get; private set; }
    public DateTime? ModifiedOn { get; private set; }

    public static Driver Create(string driverName, string driverNumber, DateOnly dob, string licenceNumber, int currentStatusId, DateTime createdOn) =>
        new(driverName, driverNumber, dob, licenceNumber, currentStatusId, createdOn);

    public void Update(string driverName, string driverNumber, DateOnly dob, string licenceNumber, int currentStatusId, DateTime modifiedOn)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(driverName);
        ArgumentException.ThrowIfNullOrWhiteSpace(driverNumber);
        ArgumentException.ThrowIfNullOrWhiteSpace(licenceNumber);

        if (driverName.Length > 100)
            throw new ArgumentException("DriverName cannot exceed 100 characters", nameof(driverName));

        if (driverNumber.Length > 20)
            throw new ArgumentException("DriverNumber cannot exceed 20 characters", nameof(driverNumber));

        if (licenceNumber.Length > 50)
            throw new ArgumentException("LicenceNumber cannot exceed 50 characters", nameof(licenceNumber));

        DriverName = driverName;
        DriverNumber = driverNumber;
        Dob = dob;
        LicenceNumber = licenceNumber;
        CurrentStatusId = currentStatusId;
        ModifiedOn = modifiedOn;
    }
}
