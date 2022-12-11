namespace WorkerMail.Model;

public sealed class MailHostedService : BackgroundService
{
    private readonly IBackgroundTaskQueue _taskQueue;
    private readonly ILogger<MailHostedService> _logger;

    public MailHostedService(
        IBackgroundTaskQueue taskQueue,
        ILogger<MailHostedService> logger) =>
        (_taskQueue, _logger) = (taskQueue, logger);

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation(
            $"{nameof(MailHostedService)} is running.{Environment.NewLine}" +
            $"{Environment.NewLine}");

        return ProcessTaskQueueAsync(stoppingToken);
    }

    private async Task ProcessTaskQueueAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                Func<CancellationToken, ValueTask>? workItem =
                    await _taskQueue.DequeueAsync(stoppingToken);

                await workItem(stoppingToken);
            }
            catch (OperationCanceledException)
            {
                // Prevent throwing if stoppingToken was signaled
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred executing task work item");
            }
        }
    }

    public override async Task StopAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation(
            $"{nameof(MailHostedService)} is stopping.");

        await base.StopAsync(stoppingToken);
    }
}