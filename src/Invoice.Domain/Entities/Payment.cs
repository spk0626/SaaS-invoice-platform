using Invoice.Domain.Common;
using Invoice.Domain.Enums;

namespace Invoice.Domain.Entities;

public sealed class Payment: AuditableEntity
{
    public Guid InvoiceId { get; private set; }
    public Invoice Invoice { get; private set; } = null!; //!null to satisfy EF Core's requirement for non-nullable reference types. 
    public decimal Amount { get; private set; }
    public DateTime PaidAt { get; private set; }
    public PaymentStatus Status { get; private set; }
    public PaymentMethod Method { get; private set; }
    public string? Reference { get; private set; } 

    private Payment() { }

    public static Payment Create(
        Guid invoiceId, 
        decimal amount, 
        PaymentMethod method, 
        string? reference = null)
    {
        if (amount <= 0) 
        throw new ArgumentException("Payment amount must be greater than zero.");

        return new Payment
        {
            InvoiceId = invoiceId,
            Amount = amount,
            PaidAt = DateTime.UtcNow,
            Status = PaymentStatus.Completed,
            Method = method,
            Reference = reference
        };
    }
    // inputs: invoiceId, amount, method, reference
    // process: create new Payment instance with generated Id and current PaidAt timestamp
    // outputs: a new Payment object initialized with the provided values and current PaidAt timestamp

}