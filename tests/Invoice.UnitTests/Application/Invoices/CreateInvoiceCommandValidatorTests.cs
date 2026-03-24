using FluentAssertions;
using Invoice.Application.Invoices.Commands.CreateInvoice;
using Xunit;

namespace Invoice.UnitTests.Application.Invoices;

public sealed class CreateInvoiceCommandValidatorTests
{
    private readonly CreateInvoiceCommandValidator _validator = new();

    [Fact]
    public void Validate_ValidCommand_Passes()
    {
        var command = new CreateInvoiceCommand(
            Guid.NewGuid(),
            DateTime.UtcNow.AddDays(30),
            "USD",
            0.1m,
            null,
            new List<CreateInvoiceItemRequest>
            {
                new("Consulting", 5, 200m)
            });

        var result = _validator.Validate(command);

        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void Validate_EmptyItems_Fails()
    {
        var command = new CreateInvoiceCommand(
            Guid.NewGuid(), DateTime.UtcNow.AddDays(10),
            "USD", 0.1m, null,
            new List<CreateInvoiceItemRequest>());

        var result = _validator.Validate(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Items");
    }

    [Fact]
    public void Validate_PastDueDate_Fails()
    {
        var command = new CreateInvoiceCommand(
            Guid.NewGuid(), DateTime.UtcNow.AddDays(-1),
            "USD", 0.1m, null,
            new List<CreateInvoiceItemRequest> { new("Item", 1, 100m) });

        var result = _validator.Validate(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "DueDate");
    }

    [Theory]
    [InlineData("us")]
    [InlineData("usd")]
    [InlineData("US")]
    [InlineData("")]
    public void Validate_InvalidCurrency_Fails(string currency)
    {
        var command = new CreateInvoiceCommand(
            Guid.NewGuid(), DateTime.UtcNow.AddDays(30),
            currency, 0.1m, null,
            new List<CreateInvoiceItemRequest> { new("Item", 1, 100m) });

        var result = _validator.Validate(command);

        result.IsValid.Should().BeFalse();
    }
}