using Azure.ResourceManager.Dns;
using Azure.ResourceManager;
using Azure.Identity;
using Azure.ResourceManager.Dns.Models;
using System.Net;

namespace DynamicDns;

public class AzureDnsClient
{
    private readonly Settings _settings;
    private readonly ILogger<AzureDnsClient> _logger;
    private readonly DnsZoneResource _dnsZone;

    public AzureDnsClient(Settings settings, ILogger<AzureDnsClient> logger)
    {
        _settings = settings;
        _logger = logger;

        var credentials = new ClientSecretCredential(_settings.AzureTenantId, _settings.AzureClientId, _settings.AzureClientSecret);
        var armClient = new ArmClient(credentials);
        var dnsZoneIdentifier = DnsZoneResource.CreateResourceIdentifier(
            _settings.AzureSubscriptionId,
            _settings.AzureResourceGroupName,
            _settings.AzureDnsZoneName);

        _dnsZone = armClient.GetDnsZoneResource(dnsZoneIdentifier);
    }

    public async Task UpdateDnsARecordIpAsync(string aRecordName, string ip)
    {
        try
        {
            var aRecordSetResource = await _dnsZone.GetDnsARecordAsync(aRecordName).ConfigureAwait(false);

            if (aRecordSetResource == null)
            {
                _logger.LogInformation("A record set with name {aRecordName} was not found. Aborting...", aRecordName);
                return;
            }

            var aRecordSetData = aRecordSetResource.Value.Data;

            if (aRecordSetData.DnsARecords.Count == 0 || aRecordSetData.DnsARecords[0].IPv4Address.ToString() != ip)
            {
                aRecordSetData.DnsARecords.Clear();
                aRecordSetData.DnsARecords.Add(new DnsARecordInfo { IPv4Address = IPAddress.Parse(ip) });

                var updatedRecordSet = await aRecordSetResource.Value.UpdateAsync(aRecordSetData).ConfigureAwait(false);

                _logger.LogInformation("A record set with name {aRecordName} was updated with IP {ip}", aRecordName, ip);
            }
            else
            {
                _logger.LogInformation("A record set with name {aRecordName} is already set to the desired IP address ({ip}). No update needed.", aRecordName, ip);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while updating the A record set with name {aRecordName} to IP {ip}", aRecordName, ip);
        }
    }
}
