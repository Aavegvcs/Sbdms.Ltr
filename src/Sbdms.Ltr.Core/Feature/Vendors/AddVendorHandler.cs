using Sbdms.Ltr.Contracts.Vendor;
using Sbdms.Ltr.Core.Domain;
using Sbdms.Ltr.Core.Interface;
using Sbdms.SharedLibrary.ApiResponse;
using Sbdms.SharedLibrary.Common;
using Sbdms.SharedLibrary.ResultPattern;

namespace Sbdms.Ltr.Core.Feature.Vendors;

public class AddVendorHandler(IVendorRepository vendorRepository, IUnitOfWork unitOfWork)
{
    public async Task<Result<CoreResponse<VendorResponse>>> HandleAsync(AddVendorRequest request)
    {
        var vendor = Vendor.Create(request.Name, request.ContactNumber, request.Email, request.Address, request.IsActive, DateTime.UtcNow);

        var result = await vendorRepository.AddAsync(vendor);
        if (result.IsError)
            return result.Errors;

        await unitOfWork.SaveChangesAsync();

        return new CoreResponse<VendorResponse>(vendor.ToResponse(), true, "Vendor added successfully.");
    }
}
