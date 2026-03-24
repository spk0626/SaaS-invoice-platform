using Invoice.Domain.Common;

namespace Invoice.Domain.Events;

public sealed record InvoiceSentDomainEvent(
    Guid InvoiceId,
    string CustomerEmail,
    string CustomerName,
    string InvoiceNumber,
    decimal Total
) : IDomainEvent
{
    public Guid EventId { get; } = Guid.NewGuid();
    public DateTime OccurredAt { get; } = DateTime.UtcNow;
}

// This record defines a domain event
// triggered when an invoice is sent
// It contains relevant information about the invoice, such as the invoice ID, customer email, customer name, invoice number, and total amount. 
// The record implements the IDomainEvent interface, which requires it to have an EventId and OccurredAt property to uniquely identify the event and track when it occurred.