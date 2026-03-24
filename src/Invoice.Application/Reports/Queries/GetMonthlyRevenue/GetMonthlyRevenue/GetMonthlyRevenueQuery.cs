// src/Invoice.Application/Reports/Queries/GetMonthlyRevenue/GetMonthlyRevenueQuery.cs
using Invoice.Application.Common.DTOs;
using Invoice.Domain.Enums;
using Invoice.Domain.Interfaces;
using MediatR;

namespace Invoice.Application.Reports.Queries.GetMonthlyRevenue;

public sealed record GetMonthlyRevenueQuery(int Year) : IRequest<List<MonthlyRevenueDto>>;

public sealed class GetMonthlyRevenueQueryHandler
    : IRequestHandler<GetMonthlyRevenueQuery, List<MonthlyRevenueDto>>
{
    private readonly IInvoiceRepository _invoices;

    public GetMonthlyRevenueQueryHandler(IInvoiceRepository invoices) =>
        _invoices = invoices;

    public async Task<List<MonthlyRevenueDto>> Handle(
        GetMonthlyRevenueQuery request,
        CancellationToken cancellationToken)
    {
        var (items, _) = await _invoices.GetPagedAsync(
            1, int.MaxValue, status: InvoiceStatus.Paid, ct: cancellationToken);

        return items
            .Where(i => i.IssuedDate.Year == request.Year)
            .SelectMany(i => i.Payments.Where(p =>
                p.Status == PaymentStatus.Completed &&
                p.PaidAt.Year == request.Year))
            .GroupBy(p => p.PaidAt.Month)
            .Select(g => new MonthlyRevenueDto(
                g.Key,
                new DateTime(request.Year, g.Key, 1).ToString("MMMM"),
                g.Sum(p => p.Amount),
                g.Count()))
            .OrderBy(r => r.Month)
            .ToList();
    }
}