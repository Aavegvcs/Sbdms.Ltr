using Sbdms.Ltr.Contracts.Driver;
using Sbdms.Ltr.Core.Domain;

namespace Sbdms.Ltr.Core.Feature.Drivers;

public static class DriverMappingExtensions
{
    public static DriverResponse ToResponse(this Driver driver) =>
        new(
            driver.Id,
            driver.DriverName,
            driver.DriverNumber,
            driver.Dob,
            driver.LicenceNumber,
            driver.CurrentStatusId,
            driver.CreatedOn,
            driver.ModifiedOn
        );
}
