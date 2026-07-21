using Sbdms.Ltr.Contracts.Vendor;
using Sbdms.Ltr.Core.Domain;

namespace Sbdms.Ltr.Core.Feature.Vendors;

public static class VendorMappingExtensions
{
    public static VendorResponse ToResponse(this Vendor vendor) =>
        new(
            vendor.Id,
            vendor.Name,
            vendor.ContactNumber,
            vendor.Email,
            vendor.Address,
            vendor.IsActive,
            vendor.CreatedOn,
            vendor.ModifiedOn
        );
}
