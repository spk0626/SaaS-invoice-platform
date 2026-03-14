namespace Invoice.Domain.Exceptions;

public abstract class DomainException : Exception // base class for all domain-specific exceptions in the Invoice system. It inherits from the standard Exception class and can be extended to create specific exceptions related to domain logic, such as validation errors or business rule violations.
{
    protected DomainException(string message) : base(message) { } // constructor that takes a message and passes it to the base Exception class
}