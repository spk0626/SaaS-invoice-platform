using Asp.Versioning;
using Invoice.Application.Common.DTOs;
using Invoice.Application.Invoices.Commands.CreateInvoice;
using Invoice.Application.Invoices.Commands.RecordPayment;
using Invoice.Application.Invoices.Commands.SendInvoice;
using Invoice.Application.Invoices.Queries.GetInvoice;
using Invoice.Application.Invoices.Queries.GetInvoices;
using Invoice.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Invoice.API.Controllers.V1;

[ApiController]
[Route("api/v{version:apiVersion}/invoices")]
[ApiVersion("1.0")]
[Authorize]
public sealed class InvoicesController : ControllerBase
{
    private readonly IMediator _mediator;

    public InvoicesController(IMediator mediator) => _mediator = mediator;

    /// Get a paged list of invoices
    [HttpGet]
    [ProducesResponseType(typeof(PagedResult<InvoiceDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        [FromQuery] Guid? customerId = null,
        [FromQuery] InvoiceStatus? status = null,
        CancellationToken ct = default)
    {
        var result = await _mediator.Send(
            new GetInvoicesQuery(page, pageSize, customerId, status), ct);
        return Ok(result);
    }

    /// Get a single invoice by ID
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(InvoiceDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid id, CancellationToken ct)
    {
        var result = await _mediator.Send(new GetInvoiceByIdQuery(id), ct);
        return Ok(result);
    }

    /// Create a new invoice
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create(
        [FromBody] CreateInvoiceCommand command,
        CancellationToken ct)
    {
        var id = await _mediator.Send(command, ct);
        return CreatedAtAction(nameof(GetById), new { id }, new { id });
    }

    /// Send an invoice to the customer
    [HttpPost("{id:guid}/send")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> Send(Guid id, CancellationToken ct)
    {
        await _mediator.Send(new SendInvoiceCommand(id), ct);
        return NoContent();
    }

    /// Record a payment against an invoice
    [HttpPost("{id:guid}/payments")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> RecordPayment(
        Guid id,
        [FromBody] RecordPaymentRequest request,
        CancellationToken ct)
    {
        await _mediator.Send(
            new RecordPaymentCommand(id, request.Amount, request.Method, request.Reference), ct);
        return NoContent();
    }
}

public record RecordPaymentRequest(
    decimal Amount,
    PaymentMethod Method,
    string? Reference);