using FluentAssertions;
using Invoice.Domain.Exceptions;
using Xunit;

namespace Invoice.UnitTests.Domain;

public sealed class CustomerTests
{
    [Fact]
    public void Create_WithValidData_ReturnsActiveCustomer()
    {
        var customer = Invoice.Domain.Entities.Customer.Create(
            "Acme Corp", "billing@acme.com");

        customer.IsActive.Should().BeTrue();
        customer.IsDeleted.Should().BeFalse();
        customer.Email.Should().Be("billing@acme.com");
    }

    [Fact]
    public void Create_WithInvalidEmail_ThrowsArgumentException()
    {
        var act = () => Invoice.Domain.Entities.Customer.Create(
            "Acme Corp", "not-an-email");

        act.Should().Throw<ArgumentException>()
            .WithMessage("*valid email*");
    }

    [Fact]
    public void Delete_ActiveCustomer_MarksAsDeleted()
    {
        var customer = Invoice.Domain.Entities.Customer.Create(
            "Acme", "test@test.com");

        customer.Delete("admin-user-id");

        customer.IsDeleted.Should().BeTrue();
        customer.IsActive.Should().BeFalse();
        customer.DeletedBy.Should().Be("admin-user-id");
    }

    [Fact]
    public void Delete_AlreadyDeletedCustomer_DoesNothing()
    {
        var customer = Invoice.Domain.Entities.Customer.Create(
            "Acme", "test@test.com");

        customer.Delete("user-1");
        var deletedAt = customer.DeletedAt;

        customer.Delete("user-2"); // second call

        customer.DeletedBy.Should().Be("user-1"); // original value preserved
    }
}