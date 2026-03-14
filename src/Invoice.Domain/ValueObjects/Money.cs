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
        throw new ArgumentException("Currency must be a 3-letter code.", nameof(currency));

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