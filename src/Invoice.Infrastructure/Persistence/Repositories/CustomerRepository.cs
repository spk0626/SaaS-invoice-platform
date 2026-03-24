using Invoice.Domain.Entities;
using Invoice.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Invoice.Infrastructure.Persistence.Repositories;

public sealed class CustomerRepository : ICustomerRepository
{
    private readonly AppDbContext _context;

    public CustomerRepository(AppDbContext context) => _context = context;

    public async Task<Customer?> GetByIdAsync(
        Guid id, CancellationToken ct = default) =>
        await _context.Customers
            .FirstOrDefaultAsync(c => c.Id == id, ct);

    public async Task<Customer?> GetByEmailAsync(
        string email, CancellationToken ct = default) =>
        await _context.Customers
            .FirstOrDefaultAsync(c => c.Email == email.ToLowerInvariant(), ct);

    public async Task<(IEnumerable<Customer> Items, int TotalCount)> GetPagedAsync(
        int page, int pageSize, string? search = null,
        CancellationToken ct = default)
    {
        var query = _context.Customers.AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
        {
            var term = search.ToLowerInvariant();
            query = query.Where(c =>
                c.Name.ToLower().Contains(term) ||
                c.Email.Contains(term));
        }

        var totalCount = await query.CountAsync(ct);

        var items = await query
            .OrderBy(c => c.Name)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);

        return (items, totalCount);
    }

    public async Task AddAsync(Customer customer, CancellationToken ct = default)
    {
        await _context.Customers.AddAsync(customer, ct);
        await _context.SaveChangesAsync(ct);
    }

    public async Task UpdateAsync(Customer customer, CancellationToken ct = default)
    {
        _context.Customers.Update(customer);
        await _context.SaveChangesAsync(ct);
    }

    public async Task<bool> EmailExistsAsync(
        string email, CancellationToken ct = default) =>
        await _context.Customers
            .AnyAsync(c => c.Email == email.ToLowerInvariant(), ct);
}