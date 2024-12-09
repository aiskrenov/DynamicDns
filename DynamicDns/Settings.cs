namespace DynamicDns;

public class Settings
{
    public TimeSpan RefreshInterval { get; set; } = TimeSpan.FromMinutes(5);

    public string DnsARecordName { get; set; }

    public string AzureTenantId { get; set; }
    
    public string AzureSubscriptionId { get; set; }
    
    public string AzureResourceGroupName { get; set; }
    
    public string AzureDnsZoneName { get; set; }

    public string AzureClientId { get; set; }

    public string AzureClientSecret { get; set; }
}
