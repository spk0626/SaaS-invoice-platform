using Invoice.Domain.Entities;
using Invoice.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Invoice.Infrastructure.Persistence.Repositories;

public sealed class PaymentRepository : IPaymentRepository
{
    private readonly AppDbContext _context;

    public PaymentRepository(AppDbContext context) => _context = context;

    public async Task<IEnumerable<Payment>> GetByInvoiceIdAsync(
        Guid invoiceId, CancellationToken ct = default) =>
        await _context.Payments
            .Where(p => p.InvoiceId == invoiceId)
            .OrderByDescending(p => p.PaidAt)
            .ToListAsync(ct);

    public async Task AddAsync(Payment payment, CancellationToken ct = default)
    {
        await _context.Payments.AddAsync(payment, ct);
        await _context.SaveChangesAsync(ct);
    }
}