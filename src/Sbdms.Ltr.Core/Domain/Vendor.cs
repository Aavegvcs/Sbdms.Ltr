using Sbdms.SharedLibrary.Common;

namespace Sbdms.Ltr.Core.Domain;

public class Vendor : AggregateRoot<int>
{
    public Vendor() { }

    private Vendor(
        string name,
        string contactNumber,
        string? email,
        string? address,
        bool isActive,
        DateTime createdOn)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        ArgumentException.ThrowIfNullOrWhiteSpace(contactNumber);

        if (name.Length > 100)
            throw new ArgumentException("Name cannot exceed 100 characters", nameof(name));

        if (contactNumber.Length > 15)
            throw new ArgumentException("ContactNumber cannot exceed 15 characters", nameof(contactNumber));

        Name = name;
        ContactNumber = contactNumber;
        Email = email;
        Address = address;
        IsActive = isActive;
        CreatedOn = createdOn;
    }

    public string Name { get; private set; } = null!;
    public string ContactNumber { get; private set; } = null!;
    public string? Email { get; private set; }
    public string? Address { get; private set; }
    public bool IsActive { get; private set; }
    public DateTime CreatedOn { get; private set; }
    public DateTime? ModifiedOn { get; private set; }

    public static Vendor Create(string name, string contactNumber, string? email, string? address, bool isActive, DateTime createdOn) =>
        new(name, contactNumber, email, address, isActive, createdOn);

    public void Update(string name, string contactNumber, string? email, string? address, bool isActive, DateTime modifiedOn)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        ArgumentException.ThrowIfNullOrWhiteSpace(contactNumber);

        if (name.Length > 100)
            throw new ArgumentException("Name cannot exceed 100 characters", nameof(name));

        if (contactNumber.Length > 15)
            throw new ArgumentException("ContactNumber cannot exceed 15 characters", nameof(contactNumber));

        Name = name;
        ContactNumber = contactNumber;
        Email = email;
        Address = address;
        IsActive = isActive;
        ModifiedOn = modifiedOn;
    }
}
