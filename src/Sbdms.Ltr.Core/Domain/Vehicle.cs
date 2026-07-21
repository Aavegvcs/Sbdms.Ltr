using Sbdms.SharedLibrary.Common;

namespace Sbdms.Ltr.Core.Domain;

public class Vehicle : AggregateRoot<int>
{
    public Vehicle() { }

    private Vehicle(
        int vendorId,
        int vehicleTypeCode,
        int? driverId,
        int currentStatusId,
        string vehicleNumber,
        string vehicleCompany,
        string modal,
        string qrUniqueCode,
        DateTime createdOn)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(vehicleNumber);
        ArgumentException.ThrowIfNullOrWhiteSpace(vehicleCompany);
        ArgumentException.ThrowIfNullOrWhiteSpace(modal);
        ArgumentException.ThrowIfNullOrWhiteSpace(qrUniqueCode);

        if (vehicleNumber.Length > 20)
            throw new ArgumentException("VehicleNumber cannot exceed 20 characters", nameof(vehicleNumber));

        if (vehicleCompany.Length > 100)
            throw new ArgumentException("VehicleCompany cannot exceed 100 characters", nameof(vehicleCompany));

        if (modal.Length > 100)
            throw new ArgumentException("Modal cannot exceed 100 characters", nameof(modal));

        if (qrUniqueCode.Length > 100)
            throw new ArgumentException("QrUniqueCode cannot exceed 100 characters", nameof(qrUniqueCode));

        VendorId = vendorId;
        VehicleTypeCode = vehicleTypeCode;
        DriverId = driverId;
        CurrentStatusId = currentStatusId;
        VehicleNumber = vehicleNumber;
        VehicleCompany = vehicleCompany;
        Modal = modal;
        QrUniqueCode = qrUniqueCode;
        CreatedOn = createdOn;
    }

    public int VendorId { get; private set; }
    public string VehicleNumber { get; private set; } = null!;
    public string VehicleCompany { get; private set; } = null!;
    public string Modal { get; private set; } = null!;

    public int VehicleTypeCode { get; private set; }
    public int? DriverId { get; private set; }
    public int CurrentStatusId { get; private set; }
    public string QrUniqueCode { get; private set; } = null!;
    public DateTime CreatedOn { get; private set; }
    public DateTime? ModifiedOn { get; private set; }

    public static Vehicle Create(
        int vendorId,
        int vehicleTypeCode,
        int? driverId,
        int currentStatusId,
        string vehicleNumber,
        string vehicleCompany,
        string modal,
        string qrUniqueCode,
        DateTime createdOn) =>
        new(vendorId, vehicleTypeCode, driverId, currentStatusId, vehicleNumber, vehicleCompany, modal, qrUniqueCode, createdOn);

    public void Update(
        int vendorId,
        int vehicleTypeCode,
        int? driverId,
        int currentStatusId,
        string vehicleNumber,
        string vehicleCompany,
        string modal,
        string qrUniqueCode,
        DateTime modifiedOn)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(vehicleNumber);
        ArgumentException.ThrowIfNullOrWhiteSpace(vehicleCompany);
        ArgumentException.ThrowIfNullOrWhiteSpace(modal);
        ArgumentException.ThrowIfNullOrWhiteSpace(qrUniqueCode);

        if (vehicleNumber.Length > 20)
            throw new ArgumentException("VehicleNumber cannot exceed 20 characters", nameof(vehicleNumber));

        if (vehicleCompany.Length > 100)
            throw new ArgumentException("VehicleCompany cannot exceed 100 characters", nameof(vehicleCompany));

        if (modal.Length > 100)
            throw new ArgumentException("Modal cannot exceed 100 characters", nameof(modal));

        if (qrUniqueCode.Length > 100)
            throw new ArgumentException("QrUniqueCode cannot exceed 100 characters", nameof(qrUniqueCode));

        VendorId = vendorId;
        VehicleTypeCode = vehicleTypeCode;
        DriverId = driverId;
        CurrentStatusId = currentStatusId;
        VehicleNumber = vehicleNumber;
        VehicleCompany = vehicleCompany;
        Modal = modal;
        QrUniqueCode = qrUniqueCode;
        ModifiedOn = modifiedOn;
    }

    public void RegenerateQrCode(string qrUniqueCode, DateTime modifiedOn)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(qrUniqueCode);

        if (qrUniqueCode.Length > 100)
            throw new ArgumentException("QrUniqueCode cannot exceed 100 characters", nameof(qrUniqueCode));

        QrUniqueCode = qrUniqueCode;
        ModifiedOn = modifiedOn;
    }

    public void ReassignDriver(int? driverId, DateTime modifiedOn)
    {
        DriverId = driverId;
        ModifiedOn = modifiedOn;
    }
}
