using System;

namespace Netcorext.Diagnostics.HealthChecks.MemoryCache
{
    public class MemoryCacheHealthCheckerOptions
    {
        public Dictionary<string, int>? CheckKeys { get; set; }
    }
}
