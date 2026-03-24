using Invoice.Domain.Common;

namespace Invoice.Application.Common.Messaging;

public sealed record DomainEventNotification<TDomainEvent>(
    TDomainEvent DomainEvent
) : IDomainEventNotification<TDomainEvent>
    where TDomainEvent : IDomainEvent;