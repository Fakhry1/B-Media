using System.Diagnostics;
using BMedia.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BMedia.Application.Common.Behaviors;

public class PerformanceBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    private readonly ILogger<PerformanceBehavior<TRequest, TResponse>> _logger;
    private readonly ICurrentUserService _currentUserService;
    private const int SlowRequestThresholdMs = 500;

    public PerformanceBehavior(ILogger<PerformanceBehavior<TRequest, TResponse>> logger, ICurrentUserService currentUserService)
    {
        _logger = logger;
        _currentUserService = currentUserService;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var sw = Stopwatch.StartNew();
        var response = await next();
        sw.Stop();

        if (sw.ElapsedMilliseconds > SlowRequestThresholdMs)
        {
            _logger.LogWarning("Slow request: {RequestName} took {ElapsedMs}ms. User: {UserId}",
                typeof(TRequest).Name, sw.ElapsedMilliseconds, _currentUserService.UserId);
        }

        return response;
    }
}
