using Sbdms.Ltr.Contracts.Vendor;
using Sbdms.Ltr.Core.Interface;
using Sbdms.SharedLibrary.ApiResponse;
using Sbdms.SharedLibrary.ResultPattern;

namespace Sbdms.Ltr.Core.Feature.Vendors;

public class GetVendorByIdHandler(IVendorRepository vendorRepository)
{
    public async Task<Result<CoreResponse<VendorResponse>>> HandleAsync(int id)
    {
        var result = await vendorRepository.FindByIdAsync(id);
        if (result.IsError)
            return result.Errors;

        return new CoreResponse<VendorResponse>(result.Value.ToResponse(), true, "Vendor retrieved successfully.");
    }
}
