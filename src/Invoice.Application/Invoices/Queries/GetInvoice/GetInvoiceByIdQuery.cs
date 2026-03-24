using AutoMapper;
using Invoice.Application.Common.DTOs;
using Invoice.Domain.Exceptions;
using Invoice.Domain.Interfaces;
using MediatR;

namespace Invoice.Application.Invoices.Queries.GetInvoice;

public sealed record GetInvoiceByIdQuery(Guid InvoiceId) : IRequest<InvoiceDto>;

public sealed class GetInvoiceByIdQueryHandler
: IRequestHandler<GetInvoiceByIdQuery, InvoiceDto>
{
    private readonly IInvoiceRepository _invoices;
    private readonly IMapper _mapper;

    public GetInvoiceByIdQueryHandler(
        IInvoiceRepository invoices, IMapper mapper)
        {
        _invoices = invoices;
        _mapper = mapper;
        }

        public async Task<InvoiceDto> Handle(
            GetInvoiceByIdQuery request,
            CancellationToken cancellationToken)
            {
                var invoice = await _invoices.GetByIdAsync(request.Id, cancellationToken)
                ?? throw NotFoundException("Invoice", request.Id);

                return _mapper.Map<InvoiceDto>(invoice);
            }

}