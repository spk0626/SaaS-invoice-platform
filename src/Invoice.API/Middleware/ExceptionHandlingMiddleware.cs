using System.Text.Json;
using FluentValidation;
using Invoice.Domain.Exceptions;

namespace Invoice.API.Middleware;

public sealed class ExceptionHandlingMiddleware     // This middleware is responsible for catching exceptions that occur during the processing of HTTP requests and converting them into standardized JSON error responses. It handles specific exceptions like ValidationException, NotFoundException, BusinessRuleException, and UnauthorizedAccessException, as well as any unhandled exceptions, logging them appropriately.
{
    private readonly RequestDelegate _next;  // pipeline behaviour 
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    private static readonly JsonSerializerOptions JsonOptions = new()   // This configuration ensures that the JSON response uses camelCase for property names and ignores null values, resulting in cleaner and more consistent error responses.
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        DefaultIgnoreCondition =
            System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
    };

    public ExceptionHandlingMiddleware(
        RequestDelegate next,
        ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (ValidationException ex)
        {
            await WriteAsync(context, StatusCodes.Status400BadRequest,
                "Validation failed",
                errors: ex.Errors.Select(e => e.ErrorMessage));
        }
        catch (NotFoundException ex)
        {
            await WriteAsync(context, StatusCodes.Status404NotFound, ex.Message);
        }
        catch (BusinessRuleException ex)
        {
            await WriteAsync(context,
                StatusCodes.Status422UnprocessableEntity, ex.Message);
        }
        catch (UnauthorizedAccessException)
        {
            await WriteAsync(context, StatusCodes.Status401Unauthorized,
                "Unauthorized.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,
                "Unhandled exception. TraceId: {TraceId}",
                context.TraceIdentifier);

            await WriteAsync(context,
                StatusCodes.Status500InternalServerError,
                "An unexpected error occurred.",
                traceId: context.TraceIdentifier);
        }
    }
    // inputs: HttpContext
    // Process: Set the response status code and content type, create a standardized error response object containing the status code, error message, optional list of errors, trace ID, and timestamp, and write the JSON-serialized response to the HTTP response body.
    // Output: JSON error response with appropriate status code and error details.

    private static async Task WriteAsync(
        HttpContext context,
        int statusCode,
        string message,
        IEnumerable<string>? errors = null,
        string? traceId = null)
    {
        context.Response.StatusCode = statusCode;
        context.Response.ContentType = "application/json";

        var response = new
        {
            status = statusCode,
            error = message,
            errors,
            traceId,
            timestamp = DateTime.UtcNow
        };

        await context.Response.WriteAsync(
            JsonSerializer.Serialize(response, JsonOptions));
    }
    // inputs: HttpContext, statusCode, message, optional list of errors, optional traceId
    // Process: Set the response status code and content type, create a standardized error response object containing the status code, error message, optional list of errors, trace ID, and timestamp, and write the JSON-serialized response to the HTTP response body.
    // Output: JSON error response with appropriate status code and error details.
}

// InvokeAsync is the main method of the middleware that gets called for each HTTP request, 
// while WriteAsync is a helper method used to generate and send the standardized JSON error response when an exception is caught. 

//InvokeAsync handles the overall flow of catching exceptions and determining the appropriate response, 
// while WriteAsync focuses on constructing and sending the error response based on the provided parameters.