using Invoice.Domain.Enums;
using Invoice.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Invoice.Infrastructure.BackgroundJobs;

public sealed class OverdueInvoiceDetector : BackgroundService
{
    private static readonly TimeSpan CheckInterval = TimeSpan.FromHours(6);

    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<OverdueInvoiceDetector> _logger;

    public OverdueInvoiceDetector(
        IServiceScopeFactory scopeFactory,
        ILogger<OverdueInvoiceDetector> logger)
    {
        _scopeFactory = scopeFactory;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Overdue invoice detector started.");

        while (!stoppingToken.IsCancellationRequested)
        {
            await ProcessOverdueInvoicesAsync(stoppingToken);
            await Task.Delay(CheckInterval, stoppingToken);
        }
    }

    private async Task ProcessOverdueInvoicesAsync(CancellationToken ct)
    {
        using var scope = _scopeFactory.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        var now = DateTime.UtcNow;

        var overdueInvoices = await db.Invoices
            .Include(i => i.Customer)
            .Where(i => i.Status == InvoiceStatus.Sent && i.DueDate < now)
            .ToListAsync(ct);

        if (!overdueInvoices.Any()) return;

        _logger.LogInformation(
            "Processing {Count} overdue invoices.", overdueInvoices.Count);

        foreach (var invoice in overdueInvoices)
        {
            try
            {
                invoice.MarkAsOverdue();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,
                    "Failed to mark invoice {InvoiceId} as overdue.", invoice.Id);
            }
        }

        await db.SaveChangesAsync(ct);
    }
}