namespace DynamicDns;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly DdnsService _ddnsService;

    public Worker(ILogger<Worker> logger, DdnsService ddnsService)
    {
        _logger = logger;
        _ddnsService = ddnsService;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            _logger.LogInformation("Dynamic DNS updater running at: {time}", DateTimeOffset.Now);

            await _ddnsService.RefreshIpAsync().ConfigureAwait(false);

            await Task.Delay(60 * 1000, stoppingToken).ConfigureAwait(false);
        }
    }
}
