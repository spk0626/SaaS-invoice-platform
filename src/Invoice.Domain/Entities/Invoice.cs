using System.Reflection.Metadata.Ecma335;
using Invoice.Domain.Enums;
using Invoice.Domain.Exceptions;
using Invoice.Domain.ValueObjects;

namespace Invoice.Domain.Entities;

public sealed class Invoice
{
    private readonly List<InvoiceItem> _items = new(); 
    private readonly List<Payment> _payments = new();

    public Guid Id { get; private set; }
    public string InvoiceNumber { get; private set; } = string.Empty;
    public Guid CustomerId { get; private set; }
    public Customer Customer { get; private set; } = null!; //!null to satisfy EF Core's requirement for non-nullable reference types. It indicates that the Customer property will be initialized by EF Core when loading data from the database, so we can safely ignore the nullability warning.
    public DateTime IssuedDate { get; private set; }
    public DateTime DueDate { get; private set; }
    public InvoiceStatus Status { get; private set; }
    public string Currency { get; private set; } = string.Empty;
    public decimal TaxRate { get; private set; }
    public string? Notes { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }
    public IReadOnlyCollection<InvoiceItem> Items => _items.AsReadOnly();
    public IReadOnlyCollection<Payment> Payments => _payments.AsReadOnly();

    // computed from items - single source of truth for total amount
    public decimal SubTotal => _items.Sum(i => i.LineTotal);
    public decimal TaxAmount => Math.Round(SubTotal * TaxRate, 2);
    public decimal Total => SubTotal + TaxAmount;
    public decimal AmountPaid => _payments
        .Where(p => p.Status == PaymentStatus.Completed)
        .Sum(p => p.Amount);
    public decimal AmountDue => Total - AmountPaid;

    private Invoice() { } // so that EF Core can create instances when loading from the database

    public static Invoice Create(
        Guid customerId,
        DateTime dueDate,
        string currency,
        decimal taxRate,
        string? notes = null)
    {
        if (dueDate < DateTime.UtcNow.Date)
            throw new BusinessRuleException("Due date must be in the future.");
        if (taxRate is < 0 or > 1)
            throw new BusinessRuleException("Tax rate must be between 0 and 1.");

        return new Invoice
        {
            Id = Guid.NewGuid(),
            InvoiceNumber = GenerateNumber(),
            CustomerId = customerId,
            IssuedDate = DateTime.UtcNow,
            DueDate = dueDate,
            Status = InvoiceStatus.Draft,
            Currency = currency.ToUpperInvariant(),
            TaxRate = taxRate,
            Notes = notes,
            CreatedAt = DateTime.UtcNow
        };

    }

    public void AddItem(string description, decimal quantity, decimal unitPrice)
    {
        if (Status != InvoiceStatus.Draft)
            throw new BusinessRuleException("Items can only be added to draft invoices.");

        _items.Add(InvoiceItem.Create(Id, description, quantity, unitPrice));
        UpdatedAt = DateTime.UtcNow;
    }

    public void Send()
    {
        if (Status != InvoiceStatus.Draft)
            throw new BusinessRuleException("Only draft invoices can be sent.");
        if (!_items.Any())
            throw new BusinessRuleException("Cannot send an invoice with no items.");

        Status = InvoiceStatus.Sent;
        UpdatedAt = DateTime.UtcNow;
    }

    public void MarkAsPaid()
    {
        if (Status == InvoiceStatus.Cancelled)
            throw new BusinessRuleException("Cannot mark a cancelled invoice as paid.");

        Status = InvoiceStatus.Paid;
        UpdatedAt = DateTime.UtcNow;
    }

    public void MarkAsOverdue()
    {
        if (Status != InvoiceStatus.Sent)
            throw new BusinessRuleException("Only sent invoices can be marked as overdue.");

        Status = InvoiceStatus.Overdue;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Cancel()
    {
        if (Status == InvoiceStatus.Paid)
            throw new BusinessRuleException("Cannot cancel a paid invoice.");

        Status = InvoiceStatus.Cancelled;
        UpdatedAt = DateTime.UtcNow;
    }

    private static string GenerateNumber() => // simple invoice number generator using a timestamp
        $"INV-{DateTime.UtcNow:yyyyMM}-{Guid.NewGuid().ToString("N")[..6].ToUpper()}"; // generates a unique invoice number based on the current year and month, followed by a random 6-character string derived from a GUID
  

}