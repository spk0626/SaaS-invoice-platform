namespace Invoice.Application.Common.DTOs;

public record CustomerDto(
    Guid Id,
    string Name,
    string Email,
    string? Phone,
    string? Address,
    bool isActive,
    DateTime CreatedAt

);