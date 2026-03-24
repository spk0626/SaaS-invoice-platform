namespace Invoice.Domain.Common;

public interface IHasDomainEvents
{
    IReadOnlyCollection<IDomainEvent> DomainEvents { get;}  // a collection of domain events associated with the entity, allowing for tracking and handling of events that occur within the domain
    void ClearDomainEvents(); // a method to clear the domain events from the entity, typically called after the events have been dispatched and handled, ensuring that the entity does not retain stale events that have already been processed
}

// IReadOnlyCollection<IDomainEvent> : a read-only collection of domain events, ensuring that the events can be accessed but not modified directly from outside the entity, promoting encapsulation and integrity of the event data.

// The IHasDomainEvents interface is a common pattern in Domain-Driven Design (DDD) that allows entities to track and manage domain events that occur within the domain. By implementing this interface, entities can raise events when significant actions or changes occur, and these events can be handled by other parts of the application, such as event handlers or message queues, to trigger additional behavior or side effects in response to those events. This promotes a more decoupled and reactive architecture, where different components can respond to changes in the domain without being tightly coupled to the entities themselves.

// an interface that allows entities to track and manage domain events