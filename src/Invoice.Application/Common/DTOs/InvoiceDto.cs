namespace Invoice.Application.Common.DTOs;

public record InvoiceDto(
    Guid Id,
    string InvoiceNumber,
    Guid CustomerId,
    string CustomerName,
    DateTime IssuedDate,
    DateTime DueDate,
    string StatusName,
    string Currency,
    decimal SubTotal,
    decimal TaxAmount,
    decimal Total,
    decimal AmountPaid,
    decimal AmountDue,
    string? Notes,
    List<InvoiceItemDto> Items,
    List<PaymentDto> Payments
    
);

public record InvoiceItemDto(
    Guid Id,
    string Description,
    int Quantity,
    decimal UnitPrice,
    decimal LineTotal
);

public record PaymentDto(
    Guid Id,
    decimal Amount,
    DateTime PaidAt,
    string Status,
    string Method,
    string? Reference
);