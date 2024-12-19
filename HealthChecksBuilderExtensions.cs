using Microsoft.Extensions.Diagnostics.HealthChecks;
using Netcorext.Diagnostics.HealthChecks.MemoryCache;

namespace Microsoft.Extensions.DependencyInjection;

public static class HealthChecksBuilderExtensions
{
    private const string NAME = "MemoryCache";

    public static IHealthChecksBuilder AddMemoryCache(this IHealthChecksBuilder builder, Func<IServiceProvider, MemoryCacheHealthCheckerOptions> factory, string name = default, HealthStatus? failureStatus = default, IEnumerable<string> tags = default, TimeSpan? timeout = default)
    {
        return AddMemoryCache<MemoryCacheHealthChecker>(builder, factory, name, failureStatus, tags, timeout);
    }

    public static IHealthChecksBuilder AddMemoryCache<THealthChecker>(this IHealthChecksBuilder builder, Func<IServiceProvider, MemoryCacheHealthCheckerOptions> factory, string name = default, HealthStatus? failureStatus = default, IEnumerable<string> tags = default, TimeSpan? timeout = default)
    {
        if (factory == null) throw new ArgumentNullException(nameof(factory));

        return builder.Add(new HealthCheckRegistration(name ?? NAME + "-" + typeof(THealthChecker).Name,
                                                       provider => (IHealthCheck)Activator.CreateInstance(typeof(THealthChecker), factory.Invoke(provider)),
                                                       failureStatus,
                                                       tags,
                                                       timeout));
    }
}
