using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Netcorext.Diagnostics.HealthChecks.MemoryCache;

public class MemoryCacheHealthChecker : IHealthCheck
{
    private readonly MemoryCacheHealthCheckerOptions _options;
    private readonly IMemoryCache _memoryCache;

    public MemoryCacheHealthChecker(MemoryCacheHealthCheckerOptions options, IMemoryCache memoryCache)
    {
        _options = options;
        _memoryCache = memoryCache;
    }

    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = new CancellationToken())
    {
        if (_options.CheckKeys == null || _options.CheckKeys.Count == 0)
            return HealthCheckResult.Healthy();

        var healthy = true;
        var result = new Dictionary<string, object>();

        foreach (var check in _options.CheckKeys)
        {
            if (!_memoryCache.TryGetValue<int>(check.Key, out var cacheCount) || cacheCount < check.Value)
                healthy = false;

            result.Add(check.Key, cacheCount);
        }

        return healthy ? HealthCheckResult.Healthy(data: result) : HealthCheckResult.Unhealthy(data: result);
    }
}
