namespace DynamicDns;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = Host.CreateApplicationBuilder(args);
        builder.Services.AddHostedService<Worker>();
        var settings = new Settings();
        builder.Configuration.GetSection("Settings").Bind(settings);
        builder.Services.AddSingleton(settings);
        builder.Services.AddSingleton<IpifyClient>();
        builder.Services.AddSingleton<AzureDnsClient>();
        builder.Services.AddSingleton<DdnsService>();

        var host = builder.Build();
        host.Run();
    }
}