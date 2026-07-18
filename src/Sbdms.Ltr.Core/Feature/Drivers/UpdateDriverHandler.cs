using Sbdms.Ltr.Contracts.Driver;
using Sbdms.Ltr.Core.Common.Errors;
using Sbdms.Ltr.Core.Interface;
using Sbdms.SharedLibrary.ApiResponse;
using Sbdms.SharedLibrary.Common;
using Sbdms.SharedLibrary.ResultPattern;

namespace Sbdms.Ltr.Core.Feature.Drivers;

public class UpdateDriverHandler(
    IDriverRepository driverRepository,
    ICurrentStatusRepository currentStatusRepository,
    IUnitOfWork unitOfWork)
{
    public async Task<Result<CoreResponse<DriverResponse>>> HandleAsync(UpdateDriverRequest request)
    {
        var existing = await driverRepository.FindByIdAsync(request.Id);
        if (existing.IsError)
            return existing.Errors;

        var currentStatus = await currentStatusRepository.GetByAsync(cs => cs.Id == request.CurrentStatusId);
        if (currentStatus is null)
            return DriverErrors.InvalidCurrentStatus;

        var duplicate = await driverRepository.GetByAsync(d =>
            d.LicenceNumber == request.LicenceNumber && d.Id != request.Id);
        if (duplicate is not null)
            return DriverErrors.DuplicateLicenceNumber;

        var driver = existing.Value;
        driver.Update(request.DriverName, request.DriverNumber, request.Dob, request.LicenceNumber, request.CurrentStatusId, DateTime.UtcNow);

        var result = await driverRepository.UpdateAsync(driver);
        if (result.IsError)
            return result.Errors;

        await unitOfWork.SaveChangesAsync();

        return new CoreResponse<DriverResponse>(driver.ToResponse(), true, "Driver updated successfully.");
    }
}
