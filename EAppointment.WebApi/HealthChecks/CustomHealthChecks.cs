using Microsoft.Extensions.Diagnostics.HealthChecks;
using StackExchange.Redis;

namespace EAppointment.WebApi.HealthChecks;

public class DatabaseHealthCheck : IHealthCheck
{
    private readonly IServiceProvider _serviceProvider;

    public DatabaseHealthCheck(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task<HealthCheckResult> CheckHealthAsync(
        HealthCheckContext context,
        CancellationToken cancellationToken = default)
    {
        try
        {
            using var scope = _serviceProvider.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<EAppointment.Infrastructure.Context.ApplicationDbContext>();

            var canConnect = await dbContext.Database.CanConnectAsync(cancellationToken);

            if (canConnect)
            {
                var recordCount = await dbContext.Doctors.CountAsync(cancellationToken);
                var data = new Dictionary<string, object>
                {
                    { "database", "connected" },
                    { "doctorCount", recordCount }
                };

                return HealthCheckResult.Healthy("Database connection is healthy", data);
            }

            return HealthCheckResult.Unhealthy("Cannot connect to database");
        }
        catch (Exception ex)
        {
            return HealthCheckResult.Unhealthy("Database health check failed", ex);
        }
    }
}

public class RedisHealthCheck : IHealthCheck
{
    private readonly IConnectionMultiplexer? _redis;
    private readonly ILogger<RedisHealthCheck> _logger;

    public RedisHealthCheck(ILogger<RedisHealthCheck> logger, IConnectionMultiplexer? redis = null)
    {
        _logger = logger;
        _redis = redis;
    }

    public async Task<HealthCheckResult> CheckHealthAsync(
        HealthCheckContext context,
        CancellationToken cancellationToken = default)
    {
        try
        {
            if (_redis == null)
            {
                return HealthCheckResult.Degraded("Redis connection not configured");
            }

            var database = _redis.GetDatabase();
            await database.PingAsync();

            var data = new Dictionary<string, object>
            {
                { "redis", "connected" },
                { "endpoints", string.Join(", ", _redis.GetEndPoints().Select(ep => ep.ToString())) }
            };

            return HealthCheckResult.Healthy("Redis connection is healthy", data);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Redis health check failed");
            return HealthCheckResult.Unhealthy("Redis health check failed", ex);
        }
    }
}

public class ApiHealthCheck : IHealthCheck
{
    private readonly ILogger<ApiHealthCheck> _logger;
    private DateTime _startTime = DateTime.UtcNow;

    public ApiHealthCheck(ILogger<ApiHealthCheck> logger)
    {
        _logger = logger;
    }

    public Task<HealthCheckResult> CheckHealthAsync(
        HealthCheckContext context,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var uptime = DateTime.UtcNow - _startTime;
            var memoryUsed = GC.GetTotalMemory(false) / 1024 / 1024; // MB

            var data = new Dictionary<string, object>
            {
                { "status", "healthy" },
                { "uptime", uptime.ToString(@"dd\.hh\:mm\:ss") },
                { "memoryUsedMB", memoryUsed },
                { "timestamp", DateTime.UtcNow }
            };

            return Task.FromResult(HealthCheckResult.Healthy("API is running", data));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "API health check failed");
            return Task.FromResult(HealthCheckResult.Unhealthy("API is not healthy", ex));
        }
    }
}

public class RabbitMQHealthCheck : IHealthCheck
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<RabbitMQHealthCheck> _logger;

    public RabbitMQHealthCheck(IConfiguration configuration, ILogger<RabbitMQHealthCheck> logger)
    {
        _configuration = configuration;
        _logger = logger;
    }

    public Task<HealthCheckResult> CheckHealthAsync(
        HealthCheckContext context,
        CancellationToken cancellationToken = default)
    {
        try
        {
            // Basic check - in production, implement actual RabbitMQ connection test
            var host = _configuration["RabbitMQ:Host"];

            if (string.IsNullOrEmpty(host))
            {
                return Task.FromResult(HealthCheckResult.Degraded("RabbitMQ not configured"));
            }

            var data = new Dictionary<string, object>
            {
                { "host", host },
                { "configured", true }
            };

            return Task.FromResult(HealthCheckResult.Healthy("RabbitMQ is configured", data));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "RabbitMQ health check failed");
            return Task.FromResult(HealthCheckResult.Unhealthy("RabbitMQ health check failed", ex));
        }
    }
}

public class DiskSpaceHealthCheck : IHealthCheck
{
    private readonly ILogger<DiskSpaceHealthCheck> _logger;
    private const long MinimumFreeMegabytes = 1024; // 1 GB

    public DiskSpaceHealthCheck(ILogger<DiskSpaceHealthCheck> logger)
    {
        _logger = logger;
    }

    public Task<HealthCheckResult> CheckHealthAsync(
        HealthCheckContext context,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var currentDirectory = Directory.GetCurrentDirectory();
            var drive = new DriveInfo(Path.GetPathRoot(currentDirectory) ?? "C:\\");

            var freeSpaceMB = drive.AvailableFreeSpace / 1024 / 1024;
            var totalSpaceMB = drive.TotalSize / 1024 / 1024;
            var usedSpaceMB = totalSpaceMB - freeSpaceMB;
            var percentUsed = (double)usedSpaceMB / totalSpaceMB * 100;

            var data = new Dictionary<string, object>
            {
                { "drive", drive.Name },
                { "freeSpaceMB", freeSpaceMB },
                { "totalSpaceMB", totalSpaceMB },
                { "percentUsed", Math.Round(percentUsed, 2) }
            };

            if (freeSpaceMB < MinimumFreeMegabytes)
            {
                return Task.FromResult(HealthCheckResult.Unhealthy(
                    $"Insufficient disk space: {freeSpaceMB} MB available", null, data));
            }

            if (percentUsed > 90)
            {
                return Task.FromResult(HealthCheckResult.Degraded(
                    $"Disk space is {percentUsed:F2}% full", null, data));
            }

            return Task.FromResult(HealthCheckResult.Healthy("Sufficient disk space available", data));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Disk space health check failed");
            return Task.FromResult(HealthCheckResult.Unhealthy("Disk space health check failed", ex));
        }
    }
}
