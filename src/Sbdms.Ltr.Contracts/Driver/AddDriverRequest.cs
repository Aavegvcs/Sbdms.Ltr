namespace Sbdms.Ltr.Contracts.Driver;

public record AddDriverRequest(
    int VendorId,
    string DriverName,
    string DriverNumber,
    DateOnly Dob,
    string LicenceNumber,
    int CurrentStatusId
);
