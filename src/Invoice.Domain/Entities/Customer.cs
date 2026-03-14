using System.ComponentModel.DataAnnotations;
using Invoice.Domain.ValueObjects;

namespace Invoice.Domain.Entities;

public sealed class Customer // difference between sealed and private class is that sealed allows inheritance but prevents further subclassing, while private class cannot be accessed outside of its containing class. In this case, Customer is sealed to allow for potential future extensions while still preventing further subclassing.
{
    private readonly List<Invoice> _invoices = new(); // private list to hold the customer's invoices. This encapsulates the invoice collection and prevents external modification.

    public Guid Id { get; private set;} // get to allow reading the Id, but private set to prevent external modification after creation
    public string Name { get; private set;} 
    public string Email { get; private set;} 
    public string? Phone { get; private set;}
    public string? Address { get; private set;}
    public bool IsActive {get; private set;}
    public DateTime CreatedAt { get; private set;}
    public DateTime? UpdatedAt { get; private set;}
    public IReadOnlyCollection<Invoice> Invoices => _invoices.AsReadOnly(); // expose invoices as a read-only collection to prevent external modification

    private Customer() {} // required by EF Core for materialization. It allows EF Core to create instances of Customer when loading data from the database.

    public static Customer Create(string name, string email, string? phone = null, string? address = null) // factory method to create a new Customer instance
    {
        var validatedEmail = EmailAddress.Of(email);

        return new Customer
        {
            Id = Guid.NewGuid(), 
            Name = name.Trim(), // trim whitespace from the name
            Email = validatedEmail.Value, 
            Phone = phone?.Trim(),
            Address = address?.Trim(),
            IsActive = true, // new customers are active by default
            CreatedAt = DateTime.UtcNow // set the creation timestamp to the current UTC time
        };   return customer;
    }

    public void Update(string name, string email, string? phone = null, string? address = null) 
    {
        Name = name.Trim();
        Phone = phone?.Trim();
        Address = address?.Trim();
        UpdatedAt = DateTime.UtcNow; 
    }

    public void Deactivate() => IsActive = false; // Deactivate instead of deleting to preserve historical data and maintain referential integrity with invoices. This allows us to keep the customer's data and their associated invoices intact while marking the customer as inactive.

}
