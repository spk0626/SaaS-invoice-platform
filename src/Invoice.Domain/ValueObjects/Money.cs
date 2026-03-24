namespace Invoice.Domain.ValueObjects;

public sealed record Money
{
    public decimal Amount {get;}
    public string Currency {get;}

    private Money(decimal amount, string currency)
    {
        if (amount < 0)
        throw new ArgumentException("Amount cannot be negative.", nameof(amount));
        if (string.IsNullOrWhiteSpace(currency) || currency.Length != 3)
        throw new ArgumentException("Currency must be a 3-letter ISO code.", nameof(currency));

        Amount = Math.Round(amount, 2);
        Currency = currency.ToUpperInvariant();
    }

    public static Money Of(decimal amount, string currency) => new Money(amount, currency); // factory method to create Money instances
    public static Money Zero(string currency) => new Money(0, currency); // factory method to create a zero amount of a specific currency

    public Money Add(Money other) // method to add two Money instances together
    {
        EnsureSameCurrency(other);   // check whether they have the same currency
        return new Money(Amount + other.Amount, Currency); 
    }

    public Money Subtract(Money other) // method to subtract one Money instance from another
    {
        EnsureSameCurrency(other);   // check whether they have the same currency
        return new Money(Amount - other.Amount, Currency);
    }

    public Money Multiply(decimal factor) // method to multiply a Money instance by a factor
    {
        return new Money(Amount * factor, Currency);
    }

    private void EnsureSameCurrency(Money other) // helper method to ensure that two Money instances have the same currency
    {
        if (Currency != other.Currency)
        throw new InvalidOperationException("Cannot operate on Money with different currencies.");
    }

    public override string ToString() => $"{Amount:F2} {Currency}"; // To make money readable when printed


}

// 1. Sealed: The record cannot be inherited. This is useful for value objects where you want to ensure that the behavior and properties are consistent and not altered through inheritance.
// 2. Record: Records in C# are reference types that provide built-in functionality for value equality, immutability, and concise syntax for defining data objects. By using a record, you get features like value-based equality (two Money instances with the same Amount and Currency will be considered equal) and immutability (the properties cannot be changed after the instance is created).
// 3. Factory Methods: The static methods Of and Zero provide a clear and controlled way to create instances of Money, ensuring that the necessary validation is performed and that the instances are created in a consistent manner.
// 4. Currency Validation: The constructor includes validation to ensure that the amount is not negative and that the currency is a valid 3-letter code, which helps maintain the integrity of the Money instances.
// 5. Arithmetic Operations: The Add, Subtract, and Multiply methods provide a way to perform arithmetic operations on Money instances while ensuring that the operations are only performed on instances with the same currency, which helps prevent errors and maintain consistency in financial calculations.

// Factory methods: static methods that provide a way to create instances of the Money record. They allow for encapsulating the creation logic and ensuring that any necessary validation or formatting is applied when creating new instances. For example, the Of method allows you to create a Money instance with a specific amount and currency, while the Zero method provides a convenient way to create a Money instance with a zero amount for a given currency.