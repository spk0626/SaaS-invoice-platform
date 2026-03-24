using AutoMapper;
using Invoice.Application.Common.DTOs;
using Invoice.Domain.Entities;
using InvoiceEntity = Invoice.Domain.Entities.Invoice;


namespace Invoice.Application.Common.Mappings;

public sealed class MappingProfile: Profile
{
    public MappingProfile()
    {
        CreateMap<Customer, CustomerDto>();

        CreateMap<InvoiceEntity, InvoiceDto>()
        .ForMember(d => d.StatusName, o => o.MapFrom(s => s.Status.ToString()))
        .ForMember(d => d.CustomerName, o => o.MapFrom(s => s.Customer.Name))
        .ForMember(d => d.Items, o => o.MapFrom(s => s.Items))
        .ForMember(d => d.Payments, o => o.MapFrom(s => s.Payments));

        CreateMap<InvoiceItem, InvoiceItemDto>();

        CreateMap<Payment, PaymentDto>()
        .ForMember(d => d.Status, o => o.MapFrom(s => s.Status.ToString()))
        .ForMember(d => d.Method, o => o.MapFrom(s => s.Method.ToString()));
    }
}

// defines mapping configurations using AutoMapper to convert between domain entities (Customer, Invoice, InvoiceItem, Payment) and their corresponding Data Transfer Objects (DTOs) used in the application layer. 
// This allows for clean separation of concerns and simplifies data transformation when sending data to clients or receiving data from clients.

// DTOs are simple objects that carry data between processes, without any business logic.

// AutoMapper is a library that helps to map properties from one object to another, reducing the need for manual mapping code and improving maintainability.