# Rate Limiting Guide

## Overview

EAppointment API implements advanced rate limiting to protect against abuse and ensure fair resource usage.

## Rate Limiting Strategies

### 1. Fixed Window Limiter
- **Policy Name**: `fixed`
- **Limit**: 100 requests per minute
- **Queue**: 5 requests
- **Use Case**: General API endpoints

```csharp
[EnableRateLimiting("fixed")]
public class MyController : ApiController
{
    // Controller code
}
```

### 2. Sliding Window Limiter
- **Policy Name**: `sliding`
- **Limit**: 100 requests per minute
- **Segments**: 6 (10-second windows)
- **Queue**: 5 requests
- **Use Case**: More granular control than fixed window

```csharp
[EnableRateLimiting("sliding")]
[HttpGet]
public async Task<IActionResult> GetAll()
{
    // Action code
}
```

### 3. Token Bucket Limiter
- **Policy Name**: `token`
- **Token Limit**: 100
- **Replenishment**: 50 tokens per minute
- **Queue**: 5 requests
- **Use Case**: Burst handling

### 4. Concurrency Limiter
- **Policy Name**: `concurrency`
- **Concurrent Requests**: 50
- **Queue**: 10 requests
- **Use Case**: Resource-intensive operations

### 5. Global Rate Limiter
- **Limit**: 200 requests per minute per user/IP
- **Auto Replenishment**: Enabled
- **Scope**: All endpoints

## Response Codes

### 429 Too Many Requests
When rate limit is exceeded:

```json
{
  "error": "Too many requests. Please try again after 30 seconds."
}
```

### Headers
```
Retry-After: 30
X-RateLimit-Limit: 100
X-RateLimit-Remaining: 0
X-RateLimit-Reset: 1640000000
```

## Configuration

### appsettings.json
```json
{
  "RateLimiting": {
    "EnableGlobalLimiter": true,
    "GlobalLimit": 200,
    "WindowMinutes": 1,
    "FixedWindow": {
      "Limit": 100,
      "WindowMinutes": 1
    }
  }
}
```

## Exemptions

Certain endpoints are exempt from rate limiting:
- `/health`
- `/metrics`
- `/swagger`

## Testing Rate Limits

### Using curl
```bash
# Test fixed window
for i in {1..110}; do
  curl -w "\nStatus: %{http_code}\n" http://localhost:5000/api/doctors/getall
done
```

### Using k6
```javascript
import http from 'k6/http';
import { check } from 'k6';

export default function () {
  const res = http.get('http://localhost:5000/api/doctors/getall');
  check(res, {
    'is status 200 or 429': (r) => [200, 429].includes(r.status),
  });
}
```

## Monitoring

Rate limit metrics are exposed via Prometheus:

- `http_requests_rate_limited_total` - Total rate-limited requests
- `http_requests_rate_limit_queue_length` - Current queue length

## Best Practices

1. **Use Appropriate Strategy**
   - Fixed Window: Simple use cases
   - Sliding Window: Smooth rate distribution
   - Token Bucket: Allow bursts
   - Concurrency: CPU/Memory intensive ops

2. **Set Realistic Limits**
   - Based on load testing results
   - Consider user patterns
   - Monitor and adjust

3. **Handle 429 Gracefully**
   - Implement exponential backoff
   - Show user-friendly messages
   - Log rate limit hits

4. **User-Based Limiting**
   - Authenticated users: Per user
   - Anonymous: Per IP
   - Premium users: Higher limits

## Advanced Configuration

### Custom Rate Limiter
```csharp
builder.Services.AddRateLimiter(options =>
{
    options.AddPolicy("custom", context =>
        RateLimitPartition.GetSlidingWindowLimiter(
            partitionKey: context.User.Identity?.Name ?? context.Request.Headers.Host.ToString(),
            factory: partition => new SlidingWindowRateLimiterOptions
            {
                AutoReplenishment = true,
                PermitLimit = GetUserLimit(partition), // Custom logic
                Window = TimeSpan.FromMinutes(1),
                SegmentsPerWindow = 6
            }));
});
```

### Per-Endpoint Configuration
```csharp
app.MapGet("/api/expensive-operation", async () =>
{
    // Handler code
})
.RequireRateLimiting("token");
```

## Troubleshooting

### Issue: Rate limit too restrictive
**Solution**: Adjust limits based on usage patterns

### Issue: Legitimate traffic blocked
**Solution**: Implement IP whitelisting or API keys

### Issue: Uneven distribution
**Solution**: Switch from Fixed to Sliding Window

## Production Considerations

1. **Distributed Systems**
   - Use Redis for distributed rate limiting
   - Share state across instances

2. **Monitoring**
   - Alert on high rate limit hits
   - Track by endpoint and user

3. **Documentation**
   - Document limits in API docs
   - Include in SLA

4. **Testing**
   - Load test with rate limits
   - Verify graceful degradation

## References

- [ASP.NET Core Rate Limiting](https://learn.microsoft.com/en-us/aspnet/core/performance/rate-limit)
- [RFC 6585 - Additional HTTP Status Codes](https://tools.ietf.org/html/rfc6585#section-4)
