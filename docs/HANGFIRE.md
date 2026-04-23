# Hangfire Background Jobs Guide

## Overview

EAppointment API uses Hangfire for background job processing, scheduled tasks, and recurring jobs.

## Features

- **Fire-and-Forget** - Execute jobs once in the background
- **Delayed** - Schedule jobs to execute after a delay
- **Recurring** - Schedule jobs to execute on a regular basis (cron)
- **Continuations** - Chain jobs together
- **Dashboard** - Web-based monitoring UI

## Dashboard Access

Access the Hangfire Dashboard at:
```
http://localhost:5000/hangfire
```

### Dashboard Features
- View all jobs (succeeded, failed, processing)
- Retry failed jobs
- Delete jobs
- View job details and exceptions
- Monitor server statistics
- Real-time updates

## Job Types

### 1. Fire-and-Forget Jobs

Execute immediately in the background:

```csharp
BackgroundJob.Enqueue(() => Console.WriteLine("Fire-and-forget!"));
```

Example in EAppointment:
```csharp
BackgroundJob.Enqueue(() => SendAppointmentReminders());
```

### 2. Delayed Jobs

Execute after a specified delay:

```csharp
BackgroundJob.Schedule(
    () => SendAppointmentReminders(),
    TimeSpan.FromDays(1)
);
```

### 3. Recurring Jobs

Execute on a schedule using cron expressions:

```csharp
RecurringJob.AddOrUpdate(
    "appointment-reminders",
    () => SendAppointmentReminders(),
    "0 9 * * *" // Every day at 9 AM
);
```

### 4. Continuation Jobs

Execute after another job completes:

```csharp
var jobId = BackgroundJob.Enqueue(() => ProcessAppointment());
BackgroundJob.ContinueJobWith(jobId, () => SendNotification());
```

## Current Jobs

### 1. Appointment Reminders
- **Schedule**: Daily at 9:00 AM
- **Cron**: `0 9 * * *`
- **Description**: Sends reminder emails for upcoming appointments

### 2. Log Cleanup
- **Schedule**: Weekly on Sunday at 2:00 AM
- **Cron**: `0 2 * * 0`
- **Description**: Deletes log files older than 30 days

### 3. Monthly Reports
- **Schedule**: 1st of every month at midnight
- **Cron**: `0 0 1 * *`
- **Description**: Generates and sends monthly statistics reports

## Cron Expression Examples

```
*    *    *    *    *
┬    ┬    ┬    ┬    ┬
│    │    │    │    │
│    │    │    │    └─── Day of week (0 - 7) (Sunday=0 or 7)
│    │    │    └──────── Month (1 - 12)
│    │    └───────────── Day of month (1 - 31)
│    └────────────────── Hour (0 - 23)
└─────────────────────── Minute (0 - 59)
```

### Common Patterns

| Expression | Description |
|------------|-------------|
| `* * * * *` | Every minute |
| `0 * * * *` | Every hour |
| `0 0 * * *` | Every day at midnight |
| `0 9 * * *` | Every day at 9 AM |
| `0 0 * * 0` | Every Sunday at midnight |
| `0 0 1 * *` | 1st of every month at midnight |
| `*/15 * * * *` | Every 15 minutes |
| `0 9-17 * * *` | Every hour from 9 AM to 5 PM |

## Configuration

### appsettings.json

```json
{
  "Hangfire": {
    "DashboardTitle": "EAppointment Background Jobs",
    "ServerName": "EAppointment-API-Server",
    "WorkerCount": 5
  }
}
```

### Program.cs Configuration

```csharp
builder.Services.AddHangfire(config => config
    .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
    .UseSimpleAssemblyNameTypeSerializer()
    .UseRecommendedSerializerSettings()
    .UseSqlServerStorage(connectionString)
    .UseRedisStorage(redisConnection));

builder.Services.AddHangfireServer();
```

## Creating Custom Jobs

### 1. Create Job Service

```csharp
public interface IMyJobService
{
    void ProcessData();
}

public class MyJobService : IMyJobService
{
    private readonly ILogger<MyJobService> _logger;

    public MyJobService(ILogger<MyJobService> logger)
    {
        _logger = logger;
    }

    public void ProcessData()
    {
        _logger.LogInformation("Processing data...");
        // Your job logic here
    }
}
```

### 2. Register Service

```csharp
builder.Services.AddScoped<IMyJobService, MyJobService>();
```

### 3. Schedule Job

```csharp
RecurringJob.AddOrUpdate<IMyJobService>(
    "process-data",
    service => service.ProcessData(),
    "0 */6 * * *" // Every 6 hours
);
```

## Job Parameters

### Passing Parameters

```csharp
BackgroundJob.Enqueue(() => ProcessAppointment(appointmentId));
```

### Complex Parameters

```csharp
public class AppointmentData
{
    public Guid AppointmentId { get; set; }
    public string PatientEmail { get; set; }
}

BackgroundJob.Enqueue(() => ProcessAppointment(data));
```

## Error Handling

### Automatic Retries

Hangfire automatically retries failed jobs:

```csharp
[AutomaticRetry(Attempts = 3)]
public void RiskyJob()
{
    // Job logic
}
```

### Custom Retry Logic

```csharp
[AutomaticRetry(Attempts = 5, DelaysInSeconds = new[] { 60, 300, 900 })]
public void ImportantJob()
{
    // Job logic
}
```

## Monitoring

### Dashboard Metrics
- **Succeeded Jobs**: Total successful executions
- **Failed Jobs**: Jobs that failed after all retries
- **Processing**: Currently executing jobs
- **Scheduled**: Jobs waiting to execute
- **Servers**: Active Hangfire servers
- **Recurring Jobs**: Scheduled recurring jobs

### Health Checks

Monitor Hangfire health:

```csharp
builder.Services.AddHealthChecks()
    .AddHangfire(options =>
    {
        options.MinimumAvailableServers = 1;
    });
```

## Best Practices

### 1. Use Interfaces

```csharp
// ✅ Good
RecurringJob.AddOrUpdate<IEmailService>(
    x => x.SendDailyReport(),
    "0 9 * * *"
);

// ❌ Bad
RecurringJob.AddOrUpdate(
    () => new EmailService().SendDailyReport(),
    "0 9 * * *"
);
```

### 2. Keep Jobs Idempotent

Jobs should be safe to execute multiple times:

```csharp
public void ProcessOrder(int orderId)
{
    var order = _db.Orders.Find(orderId);
    
    if (order.IsProcessed)
        return; // Already processed
        
    // Process order
    order.IsProcessed = true;
    _db.SaveChanges();
}
```

### 3. Use Cancellation Tokens

```csharp
public async Task LongRunningJob(CancellationToken cancellationToken)
{
    while (!cancellationToken.IsCancellationRequested)
    {
        // Do work
        await Task.Delay(1000, cancellationToken);
    }
}
```

### 4. Log Everything

```csharp
public void MyJob()
{
    _logger.LogInformation("Job started");
    
    try
    {
        // Job logic
        _logger.LogInformation("Job completed successfully");
    }
    catch (Exception ex)
    {
        _logger.LogError(ex, "Job failed");
        throw;
    }
}
```

## Performance Tuning

### Worker Count

Adjust based on workload:

```csharp
builder.Services.AddHangfireServer(options =>
{
    options.WorkerCount = Environment.ProcessorCount * 5;
});
```

### Queue Priorities

```csharp
[Queue("critical")]
public void CriticalJob() { }

[Queue("default")]
public void NormalJob() { }

[Queue("low")]
public void LowPriorityJob() { }
```

Configure servers to process queues:

```csharp
builder.Services.AddHangfireServer(options =>
{
    options.Queues = new[] { "critical", "default", "low" };
});
```

## Storage

### SQL Server (Default)

```csharp
.UseSqlServerStorage(connectionString, new SqlServerStorageOptions
{
    CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
    SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
    QueuePollInterval = TimeSpan.Zero,
    UseRecommendedIsolationLevel = true,
    DisableGlobalLocks = true
});
```

### Redis (Alternative)

```csharp
.UseRedisStorage(redisConnection, new RedisStorageOptions
{
    Prefix = "hangfire:",
    InvisibilityTimeout = TimeSpan.FromMinutes(30)
});
```

## Security

### Dashboard Authorization

```csharp
app.UseHangfireDashboard("/hangfire", new DashboardOptions
{
    Authorization = new[] { new HangfireAuthorizationFilter() }
});
```

### Custom Authorization

```csharp
public class HangfireAuthorizationFilter : IDashboardAuthorizationFilter
{
    public bool Authorize(DashboardContext context)
    {
        var httpContext = context.GetHttpContext();
        return httpContext.User.IsInRole("Admin");
    }
}
```

## Docker & Kubernetes

### Docker Compose

Hangfire is automatically configured in `docker-compose.yml`:

```yaml
eappointment-api:
  environment:
    - Hangfire__WorkerCount=5
```

### Kubernetes

Multiple replicas will share job processing:

```yaml
spec:
  replicas: 3  # All instances process jobs from shared queue
```

## Troubleshooting

### Jobs Not Executing

1. Check Hangfire server is running:
   - Dashboard → Servers tab
2. Verify connection string
3. Check worker count > 0
4. Review job queue

### Performance Issues

1. Increase worker count
2. Use queue priorities
3. Optimize job logic
4. Consider batch processing

### Dashboard Not Accessible

1. Verify route: `/hangfire`
2. Check authorization filter
3. Ensure `UseHangfireDashboard` is called

## References

- [Hangfire Documentation](https://docs.hangfire.io/)
- [Cron Expression Generator](https://crontab.guru/)
- [Best Practices](https://docs.hangfire.io/en/latest/best-practices.html)
