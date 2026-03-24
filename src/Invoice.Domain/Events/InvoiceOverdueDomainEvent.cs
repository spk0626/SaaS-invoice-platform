using Invoice.Domain.Common;

namespace Invoice.Domain.Events;

public sealed record InvoiceOverdueDomainEvent(
    Guid InvoiceId,
    string CustomerEmail,
    string CustomerName,
    string InvoiceNumber,
    decimal AmountDue,
    DateTime DueDate
) : IDomainEvent
{
    public Guid EventId { get; } = Guid.NewGuid();
    public DateTime OccurredAt { get; } = DateTime.UtcNow;
}