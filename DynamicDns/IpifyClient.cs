namespace DynamicDns;

public class IpifyClient
{
    private const string _url = "https://api.ipify.org";

    private readonly HttpClient _httpClient;
    private readonly ILogger<IpifyClient> _logger;

    public IpifyClient(ILogger<IpifyClient> logger)
    {
        _httpClient = new HttpClient();
        _logger = logger;
    }

    public async Task<string> GetPublicIpAsync()
    {
        try
        {
            var response = await _httpClient.GetAsync(_url).ConfigureAwait(false);
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

            _logger.LogInformation("Public IP address fetched: {result}", result);

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while fetching the public IP address from {url}", _url);
            return null;
        }
    }
}
