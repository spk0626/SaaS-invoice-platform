namespace Invoice.Application.Common.Interfaces;

public interface ICurrentUserService
{
    string? UserId { get; } // the currently authenticated user, nullable because there may not be a user authenticated in some contexts (e.g., background services)
    string? Email { get; } 
    IReadOnlyList<string> Roles { get; } //  can be used for authorization checks
    bool IsAuthenticated { get; } // for conditional logic in the application (e.g., showing different UI elements based on authentication status)
    bool IsInRole(string role); // for implementing role-based access control in the application
}