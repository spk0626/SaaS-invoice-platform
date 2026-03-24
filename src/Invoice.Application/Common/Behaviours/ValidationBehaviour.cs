using FluentValidation;
using MediatR;

namespace Invoice.Application.Common.Behaviours;

public sealed class ValidationBehaviour<TRequest, TResponse>
: IPipelineBehavior<TRequest, TResponse>
where TRequest : IRequest<TResponse>
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;  // Collection of validators for the request

    public ValidationBehaviour(IEnumerable<IValidator<TRequest>> validators) => // Constructor that takes a collection of validators and assigns it to the private field
        _validators = validators;

    public async Task<TResponse> Handle(    // Method that handles the validation logic for the request
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        if (!_validators.Any())    // If there are no validators for the request, simply call the next delegate in the pipeline and return its result
            return await next();

        var context = new ValidationContext<TRequest>(request); // Create a validation context for the request, which will be used by the validators to perform their validation logic

        var results = await Task.WhenAll(
            _validators.Select(v => v.ValidateAsync(context, cancellationToken)));  // Asynchronously validate the request using all the validators in the collection and wait for all validation tasks to complete

        var failures = results
        .SelectMany(r => r.Errors)
        .Where(f => f is not null)
        .ToList();

        if (failures.Count != 0)
        throw new ValidationException(failures);

        return await next();
    }
    // inputs: 
    // TRequest request - a request of type TRequest
    // RequestHandlerDelegate<TResponse> next - a delegate that represents the next handler in the pipeline
    // CancellationToken cancellationToken - a cancellation token that can be used to cancel the operation if needed. 
    
    // Process: The method performs validation on the request using the validators provided in the constructor, and if any validation failures occur, it throws a ValidationException with the details of the failures. If there are no validation failures, it calls the next delegate in the pipeline and returns its result.

    // Outputs: a response of type TResponse, which is the result of calling the next delegate in the pipeline if validation is successful. If validation fails, a ValidationException is thrown with the details of the failures.
}

// inputs of ValidationBehaviour - 
// TRequest - the type of the request being handled
// TResponse - the type of the response being returned by the handler

// Process of ValidationBehaviour -
// The ValidationBehaviour class implements the IPipelineBehavior interface, which allows it to be used as a middleware in the MediatR pipeline. When a request is sent through the pipeline, the Handle method of the ValidationBehaviour is called. This method checks if there are any validators for the request type and if so, it creates a validation context and validates the request using all the validators. If any validation failures occur, it throws a ValidationException with the details of the failures. If there are no validation failures, it calls the next delegate in the pipeline and returns its result.

// Outputs of ValidationBehaviour -
// The output of the ValidationBehaviour is a response of type TResponse, which is the result of calling the next delegate in the pipeline if validation is successful. If validation fails, a ValidationException is thrown with the details of the failures.



// IEnumerable<IValidator<TRequest>> meaning - This is a collection of validators that can be used to validate the request of type TRequest. Each validator in the collection implements the IValidator<TRequest> interface, which defines the validation logic for the request. The ValidationBehaviour class uses this collection to perform validation on the request before it is processed by the next handler in the pipeline.

// Delegate - a method with a specific signature. In the context of the ValidationBehaviour, the RequestHandlerDelegate<TResponse> is a delegate that represents the next handler in the MediatR pipeline. It is a function that takes no parameters and returns a Task<TResponse>, which is the result of processing the request. The ValidationBehaviour calls this delegate after performing validation to continue the processing of the request through the pipeline.

// The angle brackets <> are used to define generic types in C#. In the context of the ValidationBehaviour class, TRequest and TResponse are generic type parameters that allow the class to be flexible and work with any request and response types. When an instance of the ValidationBehaviour is created, the specific types for TRequest and TResponse will be provided, allowing the class to perform validation on the request and return a response of the appropriate type. This use of generics allows for code reusability and type safety in the MediatR pipeline.   

// <> a syntax for parameter type and return value type for the relevant class 

// Difference between (string text) and <TRequest>: (string text) specifies a concrete type for a parameter, while <TRequest> is a generic type parameter that can be replaced with any type when an instance of the class is created.
// Difference between concrete types and generic types: Concrete types are specific types that are defined in the code, such as string, int, or a custom class. Generic types, on the other hand, are placeholders that can be replaced with any type when an instance of a class or method is created. Generic types allow for code reusability and flexibility, as they can work with different types without needing to be rewritten for each specific type.

/* Example code of a class to understand generic types in a real world language:
public class Box<T>  // A generic class that can hold a value of any type T
{
    private T _value;  // A private field to store the value of type T

    public Box(T value)  // Constructor that takes a value of type T and assigns it to the private field
    {
        _value = value;
    }

    public T GetValue()  // A method that returns the value of type T
    {
        return _value;
    }
}

*/