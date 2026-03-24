namespace Invoice.Application.Common.DTOs;

public record MonthlyRevenueDto(
    int Month,
    string MonthName,
    decimal TotalRevenue,
    int InvoiceCount
);