using Asp.Versioning;
using Invoice.Application.Common.DTOs;
using Invoice.Application.Reports.Queries.GetMonthlyRevenue;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Invoice.API.Controllers.V1;

[ApiController]
[Route("api/v{version:apiVersion}/reports")]
[ApiVersion("1.0")]
[Authorize]
public sealed class ReportsController : ControllerBase
{
    private readonly IMediator _mediator;

    public ReportsController(IMediator mediator) => _mediator = mediator;

    /// Get monthly revenue breakdown for a given year
    [HttpGet("revenue")]
    [ProducesResponseType(typeof(List<MonthlyRevenueDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetMonthlyRevenue(
        [FromQuery] int? year = null,
        CancellationToken ct = default)
    {
        var result = await _mediator.Send(
            new GetMonthlyRevenueQuery(year ?? DateTime.UtcNow.Year), ct);
        return Ok(result);
    }
}