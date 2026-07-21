using Sbdms.Ltr.Contracts.Vendor;
using Sbdms.Ltr.Core.Interface;
using Sbdms.SharedLibrary.ApiResponse;
using Sbdms.SharedLibrary.ResultPattern;

namespace Sbdms.Ltr.Core.Feature.Vendors;

public class GetAllVendorsHandler(IVendorRepository vendorRepository)
{
    public async Task<Result<CoreResponse<IEnumerable<VendorResponse>>>> HandleAsync()
    {
        var vendors = await vendorRepository.GetAllAsync();
        var response = vendors.Select(v => v.ToResponse());

        return new CoreResponse<IEnumerable<VendorResponse>>(response, true, "Vendors retrieved successfully.");
    }
}
