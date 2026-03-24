using System.ComponentModel.DataAnnotations;
using Invoice.Domain.Common;
using Invoice.Domain.ValueObjects;

namespace Invoice.Domain.Entities;

public sealed class Customer: AggregateRoot, ISoftDeletable // difference between sealed and private class is that sealed allows inheritance but prevents further subclassing, while private class cannot be accessed outside of its containing class. In this case, Customer is sealed to allow for potential future extensions while still preventing further subclassing.
{
    private readonly List<Invoice> _invoices = new(); // private list to hold the customer's invoices. This encapsulates the invoice collection and prevents external modification.

    public string Name { get; private set;} = string.Empty;
    public string Email { get; private set;} = string.Empty;
    public string? Phone { get; private set;}
    public string? Address { get; private set;}
    public bool IsActive {get; private set;}
    public bool IsDeleted { get; private set;}
    public DateTime? DeletedAt { get; private set;}
    public string? DeletedBy { get; private set;}
    public IReadOnlyCollection<Invoice> Invoices => _invoices.AsReadOnly(); // expose invoices as a read-only collection to prevent external modification

    private Customer() {} // required by EF Core for materialization. It allows EF Core to create instances of Customer when loading data from the database.

    public static Customer Create(
        string name, 
        string email, 
        string? phone = null, 
        string? address = null) // factory method to create a new Customer instance
    {
        var validatedEmail = EmailAddress.Of(email);

        return new Customer
        {
            Name = name.Trim(), // trim whitespace from the name
            Email = validatedEmail.Value, 
            Phone = phone?.Trim(),
            Address = address?.Trim(),
            IsActive = true, // new customers are active by default
        };  
    }

    public void Update(string name, string? phone, string? address) 
    {
        Name = name.Trim();
        Phone = phone?.Trim();
        Address = address?.Trim();
    }

    public void Deactivate() => IsActive = false; // Deactivate instead of deleting to preserve historical data and maintain referential integrity with invoices. This allows us to keep the customer's data and their associated invoices intact while marking the customer as inactive.

    public void Reactivate() => IsActive = true;

    public void Delete(string deletedBy)
    {
        if (IsDeleted) return; // prevent multiple deletions
        IsDeleted = true;
        DeletedAt = DateTime.UtcNow; 
        DeletedBy = deletedBy;
        IsActive = false; // also deactivate the customer when deleted

}

}

