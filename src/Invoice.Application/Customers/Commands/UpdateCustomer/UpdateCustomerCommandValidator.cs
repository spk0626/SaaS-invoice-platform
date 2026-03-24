using FluentValidation;

namespace Invoice.Application.Customers.Commands.UpdateCustomer;

public sealed class UpdateCustomerCommandValidator
: AbstractValidator<UpdateCustomerCommand>
{
    public UpdateCustomerCommandValidator() //the constructor of the UpdateCustomerCommandValidator class
    {

        RuleFor(x => x.Id).NotEmpty();

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required.")
            .MaximumLength(200);

        RuleFor(x => x.Phone)
            .MaximumLength(30).When(x => x.Phone is not null);

        RuleFor(x => x.Address)
            .MaximumLength(500).When(x => x.Address is not null);
        
        }

    // inputs: None (the constructor initializes the validation rules for the UpdateCustomerCommand)
    
    // process: 
    // responsible for defining the validation rules for the UpdateCustomerCommand. 
    // It uses the FluentValidation library to specify the rules for each property of the command, such as Name, Email, Phone, and Address. 
    
    // outputs: None (the constructor sets up the validation rules, but does not return any
}