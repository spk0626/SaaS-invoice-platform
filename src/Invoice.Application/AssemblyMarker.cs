namespace Invoice.Application;

/// <summary>
/// Marker class for assembly scanning.
/// Used by MediatR, AutoMapper, and FluentValidation to find
/// all handlers, profiles, and validators in this assembly.
/// </summary>
public sealed class AssemblyMarker { }


// Assmebly Scanning means automatically discovering and registering components (like MediatR handlers, AutoMapper profiles, FluentValidation validators) in an assembly without having to manually specify each one. By using a marker class like AssemblyMarker, we can tell these libraries to scan the assembly where this class is located for any relevant components to register them automatically. This helps reduce boilerplate code and makes it easier to maintain as the application grows.
