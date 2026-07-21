using Sbdms.Ltr.Contracts.Vendor;
using Sbdms.Ltr.Core.Interface;
using Sbdms.SharedLibrary.ApiResponse;
using Sbdms.SharedLibrary.Common;
using Sbdms.SharedLibrary.ResultPattern;

namespace Sbdms.Ltr.Core.Feature.Vendors;

public class UpdateVendorHandler(IVendorRepository vendorRepository, IUnitOfWork unitOfWork)
{
    public async Task<Result<CoreResponse<VendorResponse>>> HandleAsync(UpdateVendorRequest request)
    {
        var existing = await vendorRepository.FindByIdAsync(request.Id);
        if (existing.IsError)
            return existing.Errors;

        var vendor = existing.Value;
        vendor.Update(request.Name, request.ContactNumber, request.Email, request.Address, request.IsActive, DateTime.UtcNow);

        var result = await vendorRepository.UpdateAsync(vendor);
        if (result.IsError)
            return result.Errors;

        await unitOfWork.SaveChangesAsync();

        return new CoreResponse<VendorResponse>(vendor.ToResponse(), true, "Vendor updated successfully.");
    }
}
