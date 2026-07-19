namespace Sbdms.Ltr.Contracts.Driver;

public record AddDriverRequest(
    string DriverName,
    string DriverNumber,
    DateOnly Dob,
    string LicenceNumber,
    int CurrentStatusId
);
