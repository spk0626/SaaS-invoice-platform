namespace Invoice.Domain.Common;

public abstract class AggregateRoot : AuditableEntity, IHasDomainEvents
{
    private readonly List<IDomainEvent> _domainEvents = new(); // a private list to hold the domain events that are raised by the aggregate root, initialized as an empty list

    public IReadOnlyCollection<IDomainEvent> DomainEvents =>
        _domainEvents.AsReadOnly();    // a public property that exposes the domain events as a read-only collection, allowing external code to access the events without modifying the underlying list, ensuring encapsulation and integrity of the event data

    protected void RaiseDomainEvent(IDomainEvent domainEvent) =>
        _domainEvents.Add(domainEvent);  // a protected method that allows derived classes to raise domain events by adding them to the private list, ensuring that events are properly tracked and can be dispatched to interested parties when necessary
    

    public void ClearDomainEvents() => _domainEvents.Clear();
    
}

// What this does:
// 1. Serve as a base class for aggregate roots in the domain, providing common properties and methods for managing domain events and auditing information.
// 2. Inherit from AuditableEntity to include properties for tracking creation and update timestamps, as well as user information for auditing purposes.
// 3. Implement the IHasDomainEvents interface to allow the aggregate root to manage a collection of domain events, enabling the raising and clearing of events as needed within the domain logic.

// what problem this address:
// 1. Centralizes the management of domain events within aggregate roots, allowing for a consistent way to raise and handle events across the domain.
// 2. Provides a common base for aggregate roots, reducing code duplication and promoting a consistent structure for entities that represent aggregates in the domain.
// 3. Facilitates the implementation of Domain-Driven Design (DDD) principles by providing a clear structure for aggregate roots and their associated domain events, enabling better organization and maintainability of the domain logic.

// In DDD, an aggregate root is the main entity that serves as the entry point for a cluster of related entities. It is responsible for maintaining the integrity of the aggregate and ensuring that all operations on the aggregate are performed through it. By inheriting from AggregateRoot, entities can become aggregate roots and benefit from the common functionality provided by the base class, such as domain event management and auditing capabilities.

// DDD means Domain-Driven Design, which is an approach to software development that emphasizes the importance of modeling the domain and its complexities in a way that reflects the real-world business processes and rules. The AggregateRoot class is a key component in DDD, as it helps to define the structure and behavior of aggregate roots, which are central to the design of the domain model. By using this base class, developers can ensure that their aggregate roots are properly structured and can effectively manage domain events, leading to a more maintainable and scalable application architecture.