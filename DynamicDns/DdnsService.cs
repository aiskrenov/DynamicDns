namespace DynamicDns;

public class DdnsService
{
    private readonly IpifyClient _ipifyClient;
    private readonly AzureDnsClient _azureDnsClient;
    private readonly Settings _settings;
    private readonly ILogger<DdnsService> _logger;

    public DdnsService(
        IpifyClient ipifyClient,
        AzureDnsClient azureDnsClient,
        Settings settings,
        ILogger<DdnsService> logger)
    {
        _ipifyClient = ipifyClient;
        _azureDnsClient = azureDnsClient;
        _settings = settings;
        _logger = logger;
    }

    public async Task RefreshIpAsync()
    {
        var ip = await _ipifyClient.GetPublicIpAsync().ConfigureAwait(false);

        if (string.IsNullOrWhiteSpace(ip))
        {
            _logger.LogError("IP address is null or empty. Aborting...");
            return;
        }

        await _azureDnsClient.UpdateDnsARecordIpAsync(_settings.DnsARecordName, ip).ConfigureAwait(false);
    }
}
