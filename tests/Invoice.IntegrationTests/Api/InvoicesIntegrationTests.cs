using System.Net;
using FluentAssertions;
using Invoice.Application.Customers.Commands.CreateCustomer;
using Invoice.Application.Invoices.Commands.CreateInvoice;
using Invoice.Domain.Enums;
using Invoice.IntegrationTests.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Invoice.IntegrationTests.Api;

public sealed class InvoicesIntegrationTests
    : BaseIntegrationTest
{
    public InvoicesIntegrationTests(IntegrationTestWebAppFactory factory)
        : base(factory) { }

    [Fact]
    public async Task GET_Invoices_WithoutToken_Returns401()
    {
        var response = await Client.GetAsync("/api/v1/invoices");

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task CreateInvoice_WithValidData_PersistsAllFields()
    {
        // Arrange — create a customer first
        var customerId = await Mediator.Send(new CreateCustomerCommand(
            "Integration Test Corp",
            $"test-{Guid.NewGuid()}@corp.com",
            null,
            null));

        var command = new CreateInvoiceCommand(
            customerId,
            DateTime.UtcNow.AddDays(30),
            "USD",
            0.1m,
            "Integration test invoice",
            new List<CreateInvoiceItemRequest>
            {
                new("Consulting services", 10, 150m),
                new("Hosting setup", 1, 500m)
            });

        // Act
        var invoiceId = await Mediator.Send(command);

        // Assert
        var saved = await DbContext.Invoices
            .Include(i => i.Items)
            .FirstOrDefaultAsync(i => i.Id == invoiceId);

        saved.Should().NotBeNull();
        saved!.CustomerId.Should().Be(customerId);
        saved.Currency.Should().Be("USD");
        saved.TaxRate.Should().Be(0.1m);
        saved.Status.Should().Be(InvoiceStatus.Draft);
        saved.Items.Should().HaveCount(2);
        saved.SubTotal.Should().Be(2000m);
        saved.TaxAmount.Should().Be(200m);
        saved.Total.Should().Be(2200m);
        saved.Notes.Should().Be("Integration test invoice");
    }

    [Fact]
    public async Task CreateInvoice_WithInvalidCommand_ThrowsValidationException()
    {
        var command = new CreateInvoiceCommand(
            Guid.NewGuid(),
            DateTime.UtcNow.AddDays(-1), // past due date — invalid
            "USD",
            0.1m,
            null,
            new List<CreateInvoiceItemRequest>());   // empty items — invalid

        var act = async () => await Mediator.Send(command);

        await act.Should().ThrowAsync<FluentValidation.ValidationException>();
    }

    [Fact]
    public async Task CreateCustomer_DuplicateEmail_ThrowsBusinessRuleException()
    {
        var email = $"duplicate-{Guid.NewGuid()}@test.com";

        await Mediator.Send(new CreateCustomerCommand("First", email, null, null));

        var act = async () =>
            await Mediator.Send(new CreateCustomerCommand("Second", email, null, null));

        await act.Should()
            .ThrowAsync<Invoice.Domain.Exceptions.BusinessRuleException>()
            .WithMessage("*already exists*");
    }
}