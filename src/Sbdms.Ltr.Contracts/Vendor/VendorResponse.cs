namespace Sbdms.Ltr.Contracts.Vendor;

public record VendorResponse(
    int Id,
    string Name,
    string ContactNumber,
    string? Email,
    string? Address,
    bool IsActive,
    DateTime CreatedOn,
    DateTime? ModifiedOn
);
