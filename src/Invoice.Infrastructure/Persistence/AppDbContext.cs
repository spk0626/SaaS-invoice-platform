using Invoice.Application.Common.Interfaces;
using Invoice.Domain.Common;
using Invoice.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using InvoiceEntity = Invoice.Domain.Entities.Invoice;


namespace Invoice.Infrastructure.Persistence;


public class AppDbContext : IdentityDbContext
{

    private readonly IMediator _mediator;
    private readonly ICurrentUserService _currentUser;

    public AppDbContext(
        DbContextOptions<AppDbContext> options, 
        IMediator mediator,
        ICurrentUserService currentUser) : base(options)
    {
        _mediator = mediator;
        _currentUser = currentUser;
    }


    public DbSet<Customer> Customers => Set<Customer>();   // it creates a DbSet property for the Customer entity, allowing performance of CRUD operations on the Customers table in the database using Entity Framework Core's DbContext.
    public DbSet<InvoiceEntity> Invoices => Set<InvoiceEntity>();
    public DbSet<InvoiceItem> InvoiceItems => Set<InvoiceItem>();
    public DbSet<Payment> Payments => Set<Payment>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Apply all configurations from the current assembly
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }

    public override async Task<int> SaveChangesAsync(
        CancellationToken cancellationToken = default)
    {
        StampAuditFields();
        var result = await base.SaveChangesAsync(cancellationToken);  // SaveChangesAsync method in DbContext class
        await DispatchDomainEventsAsync(cancellationToken);
        return result;
    }
    // input:
    // - CancellationToken cancellationToken

    // Process:
    // 1. Calls StampAuditFields() to automatically set CreatedBy, CreatedAt, UpdatedBy, UpdatedAt, DeletedBy, DeletedAt fields based on the entity state and current user.
    // 2. Calls base.SaveChangesAsync(cancellationToken) to save changes to the database and stores the result (number of affected rows).
    // 3. Calls DispatchDomainEventsAsync(cancellationToken) to dispatch any domain events that were generated during the save operation.
    // 4. Returns the result (number of affected rows) from the save operation.

    // Difference between base.SaveChangesAsync and SaveChangesAsync in this context is that base.SaveChangesAsync refers to the implementation of SaveChangesAsync in the base class (DbContext), while SaveChangesAsync refers to the overridden method in the AppDbContext class. By calling base.SaveChangesAsync, it ensures that the original functionality of saving changes to the database is preserved, while allowing for additional logic (like dispatching domain events) to be executed after the save operation.


    private void StampAuditFields()
    {
        var userId = _currentUser.UserId ?? "system";

        foreach (var entry in ChangeTracker.Entries<AuditableEntity>())
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.SetCreatedBy(userId);
                    break;
                case EntityState.Modified:
                    entry.Entity.SetUpdatedBy(userId);
                    break;
            }
        }
    }

    private async Task DispatchDomainEventsAsync(CancellationToken ct)
    {
        var aggregates = ChangeTracker
            .Entries<IHasDomainEvents>()
            .Where(e => e.Entity.DomainEvents.Any())
            .Select(e => e.Entity)
            .ToList();

        var events = aggregates
            .SelectMany(a => a.DomainEvents)
            .ToList();

        aggregates.ForEach(a => a.ClearDomainEvents());

        foreach (var domainEvent in events)
            await _mediator.Publish(domainEvent, ct);
    }

}