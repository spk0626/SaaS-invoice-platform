using Invoice.Domain.Common;
using MediatR;

namespace Invoice.Application.Common.Messaging
{
    public interface IDomainEventNotification<TDomainEvent> 
    : INotification 
    where TDomainEvent : IDomainEvent
    {
        TDomainEvent DomainEvent { get; }
    }
}




// This interface defines a contract for domain event notifications in the application, ensuring that all notifications contain a specific type of domain event and can be handled by MediatR.
// Difference between IDomainEvent and IDomainEventNotification is that the former represents the actual event that occurs within the domain, while the latter represents a notification that is sent to interested parties when a domain event occurs. The IDomainEventNotification interface allows for decoupling the event handling logic from the event definition, enabling a more flexible and maintainable architecture.