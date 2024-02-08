using Microsoft.Extensions.Caching.Memory;
using System.Net;

namespace XPressPayments.Core.Api.Middlewares
{
    public class RateLimitMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IMemoryCache _cache;
        private readonly TimeSpan _timeWindow;
        private readonly int _maxRequests;

        public RateLimitMiddleware(RequestDelegate next, IMemoryCache cache, TimeSpan timeWindow, int maxRequests)
        {
            _next = next;
            _cache = cache;
            _timeWindow = timeWindow;
            _maxRequests = maxRequests;
        }

        public async Task Invoke(HttpContext context)
        {
            var ipAddress = context.Connection.RemoteIpAddress.ToString();
            var cacheKey = $"RateLimit-{ipAddress}";

            var cacheEntryOptions = new MemoryCacheEntryOptions()
                .SetAbsoluteExpiration(_timeWindow);

            var requestCount = _cache.GetOrCreate(cacheKey, entry =>
            {
                entry.SetOptions(cacheEntryOptions);
                return 0;
            });

            if (requestCount >= _maxRequests)
            {
                context.Response.StatusCode = (int)HttpStatusCode.TooManyRequests;
                context.Response.Headers.Add("Retry-After", $"{_timeWindow.TotalSeconds}");
                await context.Response.WriteAsync("Rate limit exceeded.");
                return;
            }

            requestCount++;
            _cache.Set(cacheKey, requestCount);

            await _next(context);
        }
    }
}
