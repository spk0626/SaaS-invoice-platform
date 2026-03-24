using FluentValidation;

namespace Invoice.Application.Invoices.Commands.CreateInvoice;

public sealed class CreateInvoiceCommandValidator
: AbstractValidator<CreateInvoiceCommand>
{
    public CreateInvoiceCommandValidator()
    {
        RuleFor(x => x.CustomerId)
            .NotEmpty().WithMessage("CustomerId is required.");

        RuleFor(x => x.DueDate)
            .GreaterThan(DateTime.UtcNow).WithMessage("DueDate must be in the future.");

        RuleFor(x => x.Currency)
            .NotEmpty()
            .Length(3).WithMessage("Currency must be a 3-letter ISO code.")
            .Matches("^[A-Z]{3}$")
            .WithMessage("Currency must be uppercase (e.g. USD).");

        RuleFor(x => x.TaxRate)
            .InclusiveBetween(0m, 1m)
            .WithMessage("TaxRate must be between 0 and 1.");

        RuleFor(x => x.Items)
            .NotEmpty().WithMessage("Invoice must have at least one line item.");

        RuleForEach(x => x.Items).ChildRules(item =>
        {
            item.RuleFor(i => i.Description)
                .NotEmpty().MaximumLength(500);

            item.RuleFor(i => i.Quantity)
                .GreaterThan(0);

            item.RuleFor(i => i.UnitPrice)
                .GreaterThan(0);
        });
    }
}