using System.Security.Cryptography.X509Certificates;
using FluentValidation;

namespace Invoice.Application.Customers.Commands.CreateCustomer;

public sealed class CreateCustomerCommandValidator
: AbstractValidator<CreateCustomerCommand>
{
    public CreateCustomerCommandValidator() //the constructor of the CreateCustomerCommandValidator class
    {
    RuleFor(x => x.Name)
        .NotEmpty().WithMessage("Name is required.")
        .MaximumLength(200).WithMessage("Name must not exceed 200 characters.");

    RuleFor(x => x.Email)
        .NotEmpty().WithMessage("Email is required.")
        .EmailAddress().WithMessage("Email must be a valid email address.")
        .MaximumLength(300);

    RuleFor(x => x.Phone)
        .MaximumLength(30).When(x => x.Phone is not null);

    RuleFor(x => x.Address)
        .MaximumLength(500).When(x => x.Address is not null);
        
        }

    // inputs: None (the constructor initializes the validation rules for the CreateCustomerCommand)
    
    // process: 
    // responsible for defining the validation rules for the CreateCustomerCommand. 
    // It uses the FluentValidation library to specify the rules for each property of the command, such as Name, Email, Phone, and Address. 
    
    // outputs: None (the constructor sets up the validation rules, but does not return any
}
// inputs of CreateCustomerCommandValidator - None (the constructor initializes the validation rules for the CreateCustomerCommand)
// process of CreateCustomerCommandValidator - The CreateCustomerCommandValidator class is responsible for defining the validation rules for the CreateCustomerCommand. It uses the FluentValidation library to specify the rules for each property of the command, such as Name, Email, Phone, and Address. The constructor of the class sets up these validation rules, which will be used to validate instances of CreateCustomerCommand when they are processed in the application.
// outputs of CreateCustomerCommandValidator - None (the constructor sets up the validation rules, but