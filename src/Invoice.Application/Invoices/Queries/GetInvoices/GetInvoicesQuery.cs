using AutoMapper;
using Invoice.Application.Common.DTOs;
using Invoice.Domain.Enums;
using Invoice.Domain.Interfaces;
using MediatR;

namespace Invoice.Application.Invoices.Queries.GetInvoices;

public sealed record GetInvoicesQuery(
    int Page = 1,
    int PageSize = 20,
    Guid? CustomerId = null,
    InvoiceStatus? Status = null
): IRequest<PagedResult<InvoiceDto>>;

public sealed class GetInvoicesQueryHandler
: IRequestHandler<GetInvoicesQuery, PagedResult<InvoiceDto>>
{
    private readonly IInvoiceRepository _invoices;
    private readonly IMapper _mapper;

    public GetInvoicesQueryHandler(
        IInvoiceRepository invoices, IMapper mapper
    )
    {
        _invoices = invoices;
        _mapper = mapper;
    }

    public async Task<PagedResult<InvoiceDto>> Handle(
        GetInvoicesQuery request,
        CancellationToken cancellationToken)
        {
            var (items, totalCount) = await _invoices.GetPagedAsync(
                request.Page,
                request.PageSize,
                request.CustomerId,
                request.Status,
                cancellationToken);
            
            var dtos = _mapper.Map<IEnumerable<InvoiceDto>>(items);

            return PagedResult<InvoiceDto>.Create(
                dtos, request.Page, request.PageSize, totalCount);

        }
    
}