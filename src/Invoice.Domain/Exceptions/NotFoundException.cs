namespace Invoice.Domain.Exceptions;

public sealed class NotFoundException : DomainException // represents a specific type of domain exception that indicates that a requested entity was not found. It inherits from the DomainException base class and can be thrown when an operation fails to find the required data, such as when looking up an invoice or customer that does not exist.
{
    public NotFoundException(string entityName, object key) : base($"{entityName} with id '{key}' was not found.") { } // constructor that takes a message and passes it to the base DomainException class
}