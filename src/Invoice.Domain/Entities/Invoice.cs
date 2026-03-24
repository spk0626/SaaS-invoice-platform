using Invoice.Domain.Common;
using Invoice.Domain.Enums;
using Invoice.Domain.Exceptions;
using Invoice.Domain.Events;


namespace Invoice.Domain.Entities;

public sealed class Invoice: AggregateRoot
{
    private readonly List<InvoiceItem> _items = new(); 
    private readonly List<Payment> _payments = new();

    public string InvoiceNumber { get; private set; } = string.Empty;
    public Guid CustomerId { get; private set; }
    public Customer Customer { get; private set; } = null!; //!null to satisfy EF Core's requirement for non-nullable reference types. It indicates that the Customer property will be initialized by EF Core when loading data from the database, so we can safely ignore the nullability warning.
    public DateTime IssuedDate { get; private set; }
    public DateTime DueDate { get; private set; }
    public InvoiceStatus Status { get; private set; }
    public string Currency { get; private set; } = string.Empty;
    public decimal TaxRate { get; private set; }
    public string? Notes { get; private set; }

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

    private Invoice() { } // this is a constructor so that EF Core can create instances when loading from the database

    public static Invoice Create(
        Guid customerId,
        DateTime dueDate,
        string currency,
        decimal taxRate,
        string? notes = null)
    {
        if (dueDate <= DateTime.UtcNow.Date)
            throw new BusinessRuleException("Due date must be in the future.");
        if (taxRate is < 0 or > 1)
            throw new BusinessRuleException("Tax rate must be between 0 and 1.");

        return new Invoice
        {
            InvoiceNumber = GenerateNumber(),
            CustomerId = customerId,
            IssuedDate = DateTime.UtcNow,
            DueDate = dueDate,
            Status = InvoiceStatus.Draft,
            Currency = currency.ToUpperInvariant(),
            TaxRate = taxRate,
            Notes = notes,
        };

    }
    // inputs: customerId, dueDate, currency, taxRate, and optional notes. 
    // process: validate inputs, create new Invoice instance, set properties, return the instance. It validates the due date and tax rate, then creates and returns a new Invoice instance with the provided values and some default values for other properties (like Id, InvoiceNumber, IssuedDate, Status, and CreatedAt).
    // outputs: a new Invoice object initialized with the provided and default values.


    public void AddItem(string description, decimal quantity, decimal unitPrice)
    {
        if (Status != InvoiceStatus.Draft)
            throw new BusinessRuleException("Items can only be added to draft invoices.");

        _items.Add(InvoiceItem.Create(Id, description, quantity, unitPrice));
    }
    // inputs: description, quantity, unitPrice. 
    // process: validate invoice status, create new InvoiceItem, add to list, update timestamp. It checks if the invoice is in Draft status, then creates a new InvoiceItem and adds it to the _items list, and updates the UpdatedAt timestamp.
    // outputs: none (void method, but it modifies the state of the Invoice by adding an item and updating the timestamp)


    public void Send()
    {
        if (Status != InvoiceStatus.Draft)
            throw new BusinessRuleException("Only draft invoices can be sent.");
        if (!_items.Any())
            throw new BusinessRuleException("Cannot send an invoice with no items.");

        Status = InvoiceStatus.Sent;

        RaiseDomainEvent(new InvoiceSentDomainEvent(
            Id,
            Customer.Email,
            Customer.Name,
            InvoiceNumber,
            Total));
    }
    // inputs: none.
    // process: It checks if the invoice is in Draft status and has at least one item, then updates the status to Sent and updates the UpdatedAt timestamp.
    // outputs: none (void method, but it modifies the state of the Invoice by changing its status and updating the timestamp)


    public void RecordPayment(decimal amount, PaymentMethod method, string? reference)
    {
        if (Status == InvoiceStatus.Cancelled)
        throw new BusinessRuleException("Cannot record payment on a cancelled invoice.");

        if (amount > AmountDue)
        throw new BusinessRuleException(
            $"Payment amount {amount} exceeds amount due {AmountDue}.");

        _payments.Add(Payment.Create(Id, amount, method, reference));  // creates a new Payment entity and adds it to the _payments list. The Payment.Create method will handle the validation of the payment details and set the appropriate properties on the Payment entity.

        if (AmountDue <= 0)
        Status = InvoiceStatus.Paid;

    }
    // inputs: amount, method, reference.
    // process: It checks if the invoice is not Cancelled, then adds a new payment with the provided details and updates the status to Paid and updates the UpdatedAt timestamp.
    // outputs: none (void method, but it modifies the state of the Invoice by changing its status to Paid and updating the timestamp)


    public void MarkAsOverdue()
    {
        if (Status != InvoiceStatus.Sent)
            throw new BusinessRuleException("Only sent invoices can be marked as overdue.");

        Status = InvoiceStatus.Overdue;

        RaiseDomainEvent(new InvoiceOverdueDomainEvent(
            Id,
            Customer.Email,
            Customer.Name,
            InvoiceNumber,
            AmountDue,
            DueDate));

    }
    // inputs: none.
    // process: It checks if the invoice is in Sent status, then updates the status to Overdue and updates the UpdatedAt timestamp.
    // outputs: none (void method, but it modifies the state of the Invoice by changing its status to Overdue and updating the timestamp)


    public void Cancel()
    {
        if (Status == InvoiceStatus.Paid)
            throw new BusinessRuleException("Cannot cancel a paid invoice.");

        Status = InvoiceStatus.Cancelled;
    }
    // inputs: none.
    // process: It checks if the invoice is not Paid, then updates the status to Cancelled and updates the UpdatedAt timestamp.
    // outputs: none (void method, but it modifies the state of the Invoice by changing its status to Cancelled and updating the timestamp)
    

    private static string GenerateNumber() => // simple invoice number generator using a timestamp
        $"INV-{DateTime.UtcNow:yyyyMM}-{Guid.NewGuid().ToString("N")[..6].ToUpper()}"; // generates a unique invoice number based on the current year and month, followed by a random 6-character string derived from a GUID
  
}