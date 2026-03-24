namespace Invoice.Domain.Common;

public interface IDomainEvent 
{
    Guid EventId { get; } 
    DateTime OccurredAt { get; }
}


// This interface defines a contract for domain events in the application, ensuring that all events have a unique identifier and a timestamp for when they occurred, and can be handled by MediatR.