namespace Invoice.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;


public class AppDbContext : DbContext
{

    public AppDbContext(DbContextOptions<AppDbContext> options, IMediator mediator) : base(options)
    {
    }

    public DbSet<Invoice.Domain.Entities.Invoice> Invoices => Set<Invoice.Domain.Entities.Invoice>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Apply all configurations from the current assembly
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }

    private async Task DispatchDomainEventsAsync(CancellationToken ct)
    {
        var aggregates = ChangeTracker
        .Entries<IHasDomainEvents>()
        .Where(e => e.Entity.DomainEvents.Any())
        .Select(e => e.Entity)
        .ToList();

        var domainEvents = aggregates
            .SelectMany(a => a.DomainEvents)
            .ToList();

        aggregates.ForEach(a => a.ClearDomainEvents());

        foreach (var domainEvent in domainEvents)
        {
            var notificationType = typeof(IDomainEventNotification<>)
            .MakeGenericType(domainEvent.GetType());

            var notification = Activator.CreateInstance(notificationType, domainEvent);

            await _mediator.Publish(notification, ct);
        }
    }
}