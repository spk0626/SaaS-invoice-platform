using FluentAssertions;
using Invoice.Application.Invoices.Commands.CreateInvoice;
using InvoiceEntity = Invoice.Domain.Entities.Invoice;
using Invoice.Domain.Exceptions;
using Invoice.Domain.Interfaces;
using Invoice.Domain.Entities;
using Moq;
using Xunit;

namespace Invoice.UnitTests.Application.Invoices;

public sealed class CreateInvoiceCommandHandlerTests
{
    private readonly Mock<IInvoiceRepository> _invoiceRepo = new();
    private readonly Mock<ICustomerRepository> _customerRepo = new();

    private CreateInvoiceCommandHandler CreateHandler() =>
        new(_invoiceRepo.Object, _customerRepo.Object);

    private static CreateInvoiceCommand ValidCommand(Guid customerId) =>
        new(
            customerId,
            DateTime.UtcNow.AddDays(30),
            "USD",
            0.1m,
            null,
            new List<CreateInvoiceItemRequest>
            {
                new("Web Development", 10, 150m),
                new("Hosting", 1, 50m)
            });

    [Fact]
    public async Task Handle_ValidCommand_CreatesInvoiceAndReturnsId()
    {
        var customerId = Guid.NewGuid();

        _customerRepo
            .Setup(r => r.GetByIdAsync(customerId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(Customer.Create("Test Corp", "test@test.com"));

        InvoiceEntity? savedInvoice = null;
        _invoiceRepo
            .Setup(r => r.AddAsync(
                It.IsAny<InvoiceEntity>(),
                It.IsAny<CancellationToken>()))
            .Callback<InvoiceEntity, CancellationToken>(
                (inv, _) => savedInvoice = inv)
            .Returns(Task.CompletedTask);

        var id = await CreateHandler()
            .Handle(ValidCommand(customerId), CancellationToken.None);

        id.Should().NotBeEmpty();
        savedInvoice.Should().NotBeNull();
        savedInvoice!.SubTotal.Should().Be(1550m);
        savedInvoice.TaxAmount.Should().Be(155m);
        savedInvoice.Total.Should().Be(1705m);

        _invoiceRepo.Verify(r =>
            r.AddAsync(It.IsAny<InvoiceEntity>(),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task Handle_NonExistentCustomer_ThrowsNotFoundException()
    {
        _customerRepo
            .Setup(r => r.GetByIdAsync(
                It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Customer?)null);

        var act = async () =>
            await CreateHandler()
                .Handle(ValidCommand(Guid.NewGuid()), CancellationToken.None);

        await act.Should().ThrowAsync<NotFoundException>();
    }
}