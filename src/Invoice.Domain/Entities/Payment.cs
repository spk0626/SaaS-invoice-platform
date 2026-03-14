using Invoice.Domain.Enums;

namespace Invoice.Domain.Entities;

public sealed class Payment
{
    public Guid Id { get; private set; }
    public Guid InvoiceId { get; private set; }
    public Invoice Invoice { get; private set; } = null!; //!null to satisfy EF Core's requirement for non-nullable reference types. 
    public decimal Amount { get; private set; }
    public DateTime PaidAt { get; private set; }
    public PaymentStatus Status { get; private set; }
    public PaymentMethod Method { get; private set; }
    public string? Reference { get; private set; } 

    private Payment() { }

    public static Payment Create(Guid invoiceId, decimal amount, PaymentMethod method, string? reference = null)
    {
        return new Payment
        {
            Id = Guid.NewGuid(),
            InvoiceId = invoiceId,
            Amount = amount,
            PaidAt = DateTime.UtcNow,
            Status = PaymentStatus.Completed,
            Method = method,
            Reference = reference
        };
    }

}