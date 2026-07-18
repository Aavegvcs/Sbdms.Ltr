namespace Sbdms.Ltr.Contracts.Driver;

public record UpdateDriverRequest(
    int Id,
    string DriverName,
    string DriverNumber,
    DateOnly Dob,
    string LicenceNumber,
    int CurrentStatusId
);
