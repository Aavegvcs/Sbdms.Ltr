namespace Sbdms.Ltr.Contracts.Driver;

public record DriverResponse(
    int Id,
    string DriverName,
    string DriverNumber,
    DateOnly Dob,
    string LicenceNumber,
    int CurrentStatusId,
    DateTime CreatedOn,
    DateTime? ModifiedOn
);
