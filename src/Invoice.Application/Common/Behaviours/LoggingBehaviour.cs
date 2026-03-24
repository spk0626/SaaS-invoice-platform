using MediatR;
using Microsoft.Extensions.Logging;

namespace Invoice.Application.Common.Behaviours;

public sealed class LoggingBehaviour<TRequest, TResponse>  // A pipeline behavior that logs the handling of requests and their responses
: IPipelineBehavior<TRequest, TResponse>
where TRequest : IRequest<TResponse>
{
    private readonly ILogger<LoggingBehaviour<TRequest, TResponse>> _logger; // A logger instance that will be used to log information about the handling of requests and their responses

    public LoggingBehaviour(
        ILogger<LoggingBehaviour<TRequest, TResponse>> logger) =>
        _logger = logger;               // Constructor that takes a logger instance and assigns it to the private field

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        var requestName = typeof(TRequest).Name;
        _logger.LogInformation("Handling {RequestName} with content: {@Request}", requestName, request);

        var response = await next();

        _logger.LogInformation("Handled {RequestName} with response: {@Response}", requestName, response);

        return response;
    }
    // inputs: 
    // TRequest request - a request of type TRequest
    // RequestHandlerDelegate<TResponse> next - a delegate that represents the next handler in the pipeline
    // CancellationToken cancellationToken - a cancellation token that can be used to cancel the operation if needed.

    // Process: The method logs the handling of the request by logging the request name and its content before calling the next delegate in the pipeline. After the next delegate is called and a response is received, it logs the response along with the request name. Finally, it returns the response.
    
    // Outputs: a response of type TResponse, which is the result of calling the next delegate in the pipeline after logging the handling of the request and its response.
}

// inputs of LoggingBehaviour -
// TRequest - the type of the request being handled
// TResponse - the type of the response being returned by the handler

// Process of LoggingBehaviour -
// The LoggingBehaviour class implements the IPipelineBehavior interface, which allows it to be used as a middleware in the MediatR pipeline. When a request is sent through the pipeline, the Handle method of the LoggingBehaviour is called. This method logs the handling of the request by logging the request name and its content before calling the next delegate in the pipeline. After the next delegate is called and a response is received, it logs the response along with the request name. Finally, it returns the response.

// Outputs of LoggingBehaviour -
// a response of type TResponse, which is the result of calling the next delegate in the pipeline after logging the handling of the request and its response.

// A delegate is a type that represents references to methods with a specific parameter list and return type. It allows you to pass methods as arguments to other methods, store them in variables, and call them dynamically at runtime. In the context of the MediatR pipeline, a RequestHandlerDelegate<TResponse> is a delegate that represents the next handler in the pipeline that will process the request and return a response of type TResponse.