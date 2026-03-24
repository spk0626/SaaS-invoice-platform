using System.Diagnostics;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Invoice.Application.Common.Behaviours;

public sealed class PerformanceBehaviour<TRequest, TResponse>
: IPipelineBehavior<TRequest, TResponse>
where TRequest : IRequest<TResponse>
{
    private const int SlowREquestThresholdMs = 500; // Threshold in milliseconds to consider a request as slow

    private readonly Stopwatch _timer = new(); // Stopwatch to measure the time taken to handle the request

    private readonly ILogger<PerformanceBehaviour<TRequest, TResponse>> _logger;

    public PerformanceBehaviour(
        ILogger<PerformanceBehaviour<TRequest, TResponse>> logger) =>
        _logger = logger;
    
    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        _timer.Restart(); // Start the timer to measure the time taken to handle the request
        var response = await next(); 
        _timer.Stop(); // Stop the timer after the request has been handled

        if (_timer.ElapsedMilliseconds > SlowREquestThresholdMs)
        {
            _logger.LogWarning(
                "Slow request: {RequestName} took {ElapsedMs}ms. {@Request}",
                typeof(TRequest).Name,
                _timer.ElapsedMilliseconds,
                request
            );
        }

        return response;
    }
    // input: TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken
    // process: The method starts a timer before calling the next delegate in the pipeline to handle the request. After the request has been handled, it stops the timer and checks if the elapsed time exceeds a predefined threshold. If it does, it logs a warning message with the request name, elapsed time, and request details. Finally, it returns the response from the next delegate in the pipeline.
    // output: TResponse response, which is the result of calling the next delegate in the pipeline after measuring the time taken to handle the request and logging a warning if it exceeds the threshold.
}
// input of class: TRequest - the type of the request being handled, TResponse - the type of the response being returned by the handler
// process of class: The PerformanceBehaviour class implements the IPipelineBehavior interface, which allows it to be used as a middleware in the MediatR pipeline. When a request is sent through the pipeline, the Handle method of the PerformanceBehaviour is called. This method starts a timer before calling the next delegate in the pipeline to handle the request. After the request has been handled, it stops the timer and checks if the elapsed time exceeds a predefined threshold. If it does, it logs a warning message with the request name, elapsed time, and request details. Finally, it returns the response from the next delegate in the pipeline.
// output of class: a response of type TResponse, which is the result of calling the next delegate in the pipeline after measuring the time taken to handle the request and logging a warning if it exceeds the threshold.