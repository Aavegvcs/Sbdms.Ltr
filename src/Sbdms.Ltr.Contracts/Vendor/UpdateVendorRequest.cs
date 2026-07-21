namespace Sbdms.Ltr.Contracts.Vendor;

public record UpdateVendorRequest(
    int Id,
    string Name,
    string ContactNumber,
    string? Email,
    string? Address,
    bool IsActive
);
