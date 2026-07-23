using Sbdms.Ltr.Contracts.Driver;
using Sbdms.Ltr.Core.Common.Errors;
using Sbdms.Ltr.Core.Domain;
using Sbdms.Ltr.Core.Interface;
using Sbdms.SharedLibrary.ApiResponse;
using Sbdms.SharedLibrary.Common;
using Sbdms.SharedLibrary.ResultPattern;

using Sbdms.Ltr.Core.Common.Helper;
namespace Sbdms.Ltr.Core.Feature.Drivers;

public class AddDriverHandler(
    IDriverRepository driverRepository,
    IVendorRepository vendorRepository,
    ICurrentStatusRepository currentStatusRepository,
    IUnitOfWork unitOfWork)
{
    public async Task<Result<CoreResponse<DriverResponse>>> HandleAsync(AddDriverRequest request)
    {
        var vendor = await vendorRepository.GetByAsync(v => v.Id == request.VendorId);
        if (vendor is null)
            return DriverErrors.InvalidVendor;

        var currentStatus = await currentStatusRepository.GetByAsync(cs => cs.Id == request.CurrentStatusId);
        if (currentStatus is null)
            return DriverErrors.InvalidCurrentStatus;

        var driver = Driver.Create(request.VendorId, request.DriverName, request.DriverNumber, request.Dob, request.LicenceNumber, request.CurrentStatusId, IndianStandardTime.Now);

        var result = await driverRepository.AddAsync(driver);
        if (result.IsError)
            return result.Errors;

        await unitOfWork.SaveChangesAsync();

        return new CoreResponse<DriverResponse>(driver.ToResponse(), true, "Driver added successfully.");
    }
}
