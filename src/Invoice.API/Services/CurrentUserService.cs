using Invoice.Application.Common.Interfaces;
using System.Security.Claims;

namespace Invoice.API.Services;

public sealed class CurrentUserService : ICurrentUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUserService(IHttpContextAccessor httpContextAccessor) =>
        _httpContextAccessor = httpContextAccessor;

    private ClaimsPrincipal? User =>
        _httpContextAccessor.HttpContext?.User;

    public string? UserId =>
        User?.FindFirstValue(ClaimTypes.NameIdentifier);

    public string? Email =>
        User?.FindFirstValue(ClaimTypes.Email);

    public IReadOnlyList<string> Roles =>
        User?.FindAll(ClaimTypes.Role)
             .Select(c => c.Value)
             .ToList()
             .AsReadOnly()
        ?? new List<string>().AsReadOnly();

    public bool IsAuthenticated =>
        User?.Identity?.IsAuthenticated ?? false;

    public bool IsInRole(string role) =>
        User?.IsInRole(role) ?? false;
}