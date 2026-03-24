using InvoiceEntity = Invoice.Domain.Entities.Invoice;
using Invoice.Domain.Enums;
using Invoice.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Invoice.Infrastructure.Persistence.Repositories;

public sealed class InvoiceRepository : IInvoiceRepository
{
    private readonly AppDbContext _context;

    public InvoiceRepository(AppDbContext context) => _context = context;

    public async Task<InvoiceEntity?> GetByIdAsync(
        Guid id, CancellationToken ct = default) =>
        await _context.Invoices
            .Include(i => i.Items)
            .Include(i => i.Customer)
            .Include(i => i.Payments)
            .FirstOrDefaultAsync(i => i.Id == id, ct);

    public async Task<(IEnumerable<InvoiceEntity> Items, int TotalCount)>
        GetPagedAsync(
            int page,
            int pageSize,
            Guid? customerId = null,
            InvoiceStatus? status = null,
            CancellationToken ct = default)
    {
        var query = _context.Invoices
            .Include(i => i.Customer)
            .AsQueryable();

        if (customerId.HasValue)
            query = query.Where(i => i.CustomerId == customerId);

        if (status.HasValue)
            query = query.Where(i => i.Status == status);

        var totalCount = await query.CountAsync(ct);

        var items = await query
            .OrderByDescending(i => i.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);

        return (items, totalCount);
    }

    public async Task AddAsync(
        InvoiceEntity invoice, CancellationToken ct = default)
    {
        await _context.Invoices.AddAsync(invoice, ct);
        await _context.SaveChangesAsync(ct);
    }

    public async Task UpdateAsync(
        InvoiceEntity invoice, CancellationToken ct = default)
    {
        _context.Invoices.Update(invoice);
        await _context.SaveChangesAsync(ct);
    }

    public async Task<bool> ExistsAsync(
        Guid id, CancellationToken ct = default) =>
        await _context.Invoices.AnyAsync(i => i.Id == id, ct);
}