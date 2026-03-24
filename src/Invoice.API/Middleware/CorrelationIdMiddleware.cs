namespace Invoice.API.Middleware;

public sealed class CorrelationIdMiddleware         // Correlation IDs means that each request gets a unique ID that can be used to trace the request through logs and other systems. This is especially useful in distributed systems where a request may pass through multiple services.
{
    private const string CorrelationIdHeader = "X-Correlation-ID";
    private readonly RequestDelegate _next;

    public CorrelationIdMiddleware(RequestDelegate next) => _next = next;

    public async Task InvokeAsync(HttpContext context)
    {
        if (!context.Request.Headers
                .TryGetValue(CorrelationIdHeader, out var correlationId))
            correlationId = Guid.NewGuid().ToString();

        context.Items["CorrelationId"] = correlationId.ToString();
        context.Response.Headers[CorrelationIdHeader] = correlationId.ToString();

        using (Serilog.Context.LogContext.PushProperty(
                   "CorrelationId", correlationId.ToString()))
        {
            await _next(context);
        }
    }
}