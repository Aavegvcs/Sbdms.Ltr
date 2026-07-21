namespace Sbdms.Ltr.Contracts.Vendor;

public record AddVendorRequest(
    string Name,
    string ContactNumber,
    string? Email,
    string? Address,
    bool IsActive
);
