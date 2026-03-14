namespace Invoice.Domain.Exceptions;

public sealed class BusinessRuleException : DomainException // represents a violation of a business rule. It inherits from the DomainException base class and can be thrown when an operation violates a defined business rule, such as attempting to create an invoice with a negative amount or trying to delete an invoice that has already been paid.
{
    public BusinessRuleException(string message) : base(message) { } // constructor that takes a message and passes it to the base DomainException class
}