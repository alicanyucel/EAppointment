using Hangfire;
using Serilog;

namespace EAppointment.WebApi.Jobs;

public interface IBackgroundJobService
{
    void ScheduleAppointmentReminders();
    void CleanupOldLogs();
    void GenerateReports();
}

public class BackgroundJobService : IBackgroundJobService
{
    public void ScheduleAppointmentReminders()
    {
        // Fire-and-forget job
        BackgroundJob.Enqueue(() => SendAppointmentReminders());

        // Delayed job (send reminder 1 day before)
        BackgroundJob.Schedule(() => SendAppointmentReminders(), TimeSpan.FromDays(1));

        // Recurring job (every day at 9 AM)
        RecurringJob.AddOrUpdate(
            "appointment-reminders",
            () => SendAppointmentReminders(),
            "0 9 * * *"); // Cron expression
    }

    public void CleanupOldLogs()
    {
        // Recurring job (every week on Sunday at 2 AM)
        RecurringJob.AddOrUpdate(
            "cleanup-logs",
            () => DeleteOldLogs(),
            "0 2 * * 0"); // Cron: Every Sunday at 2 AM
    }

    public void GenerateReports()
    {
        // Recurring job (every month on the 1st at midnight)
        RecurringJob.AddOrUpdate(
            "monthly-reports",
            () => GenerateMonthlyReports(),
            "0 0 1 * *"); // Cron: 1st of every month
    }

    public void SendAppointmentReminders()
    {
        Log.Information("Sending appointment reminders...");
        // Implementation: Query appointments for tomorrow and send reminders
        // This would integrate with your notification service
    }

    public void DeleteOldLogs()
    {
        Log.Information("Cleaning up old logs...");
        // Implementation: Delete logs older than 30 days
        var logsDirectory = "logs";
        if (Directory.Exists(logsDirectory))
        {
            var files = Directory.GetFiles(logsDirectory, "*.log")
                .Where(f => File.GetCreationTime(f) < DateTime.Now.AddDays(-30))
                .ToList();

            foreach (var file in files)
            {
                try
                {
                    File.Delete(file);
                    Log.Information("Deleted old log file: {FileName}", file);
                }
                catch (Exception ex)
                {
                    Log.Error(ex, "Failed to delete log file: {FileName}", file);
                }
            }
        }
    }

    public void GenerateMonthlyReports()
    {
        Log.Information("Generating monthly reports...");
        // Implementation: Generate and send monthly statistics reports
    }
}
