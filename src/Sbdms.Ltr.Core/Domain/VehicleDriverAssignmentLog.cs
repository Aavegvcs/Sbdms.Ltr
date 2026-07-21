using Sbdms.SharedLibrary.Common;

namespace Sbdms.Ltr.Core.Domain;

// Written once per re-pairing, right before the Vehicle's DriverId actually changes — a full
// snapshot of the mapping being replaced, plus what it was changed to.
public class VehicleDriverAssignmentLog : AggregateRoot<int>
{
    public VehicleDriverAssignmentLog() { }

    private VehicleDriverAssignmentLog(
        int vehicleId,
        string vehicleNumber,
        string vehicleCompany,
        string modal,
        int vendorId,
        int? oldDriverId,
        string? oldDriverName,
        string? oldDriverNumber,
        string? oldLicenceNumber,
        int? newDriverId,
        DateTime changedOn)
    {
        VehicleId = vehicleId;
        VehicleNumber = vehicleNumber;
        VehicleCompany = vehicleCompany;
        Modal = modal;
        VendorId = vendorId;
        OldDriverId = oldDriverId;
        OldDriverName = oldDriverName;
        OldDriverNumber = oldDriverNumber;
        OldLicenceNumber = oldLicenceNumber;
        NewDriverId = newDriverId;
        ChangedOn = changedOn;
    }

    public int VehicleId { get; private set; }
    public string VehicleNumber { get; private set; } = null!;
    public string VehicleCompany { get; private set; } = null!;
    public string Modal { get; private set; } = null!;
    public int VendorId { get; private set; }

    public int? OldDriverId { get; private set; }
    public string? OldDriverName { get; private set; }
    public string? OldDriverNumber { get; private set; }
    public string? OldLicenceNumber { get; private set; }

    public int? NewDriverId { get; private set; }
    public DateTime ChangedOn { get; private set; }

    public static VehicleDriverAssignmentLog Create(
        int vehicleId,
        string vehicleNumber,
        string vehicleCompany,
        string modal,
        int vendorId,
        int? oldDriverId,
        string? oldDriverName,
        string? oldDriverNumber,
        string? oldLicenceNumber,
        int? newDriverId,
        DateTime changedOn) =>
        new(vehicleId, vehicleNumber, vehicleCompany, modal, vendorId,
            oldDriverId, oldDriverName, oldDriverNumber, oldLicenceNumber, newDriverId, changedOn);
}
