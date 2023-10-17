
using Common.Host;

var configuration = GetConfiguration();

try
{
    var host = CreateHostBuilder(configuration, args);
    Log.Information("Configuration webHost...",program.AppName);
    Log.Information("applying Migration....",program.AppName);
    host.MigrateDataBase<program>((services) =>
    {
        //var context = services.GetService<CatalogContext>();
        //var env = services.GetService<IWebHostEnvironment>();
        //var logger = services.GetRequiredService<ILogger<CatalogContextSeed>>();
        //new CatalogContextSeed().MigrateAndSeedAsync(context,env,logger).Wait();
    });

    Log.Information("Starting webHost....", program.AppName);

    host.Run();

}
catch (Exception e)
{
    Log.Fatal(e,"Host Terminated");
}


IConfiguration GetConfiguration()
{
    var path = Directory.GetCurrentDirectory();

    var builder = new ConfigurationBuilder()
        .SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
        .AddEnvironmentVariables();

    return builder.Build();
}

IHost CreateHostBuilder(IConfiguration configuration, string[] args) =>

    Host.CreateDefaultBuilder(args)
        .ConfigureWebHostDefaults(webBuilder =>
        {
            webBuilder.UseStartup<Startup>()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseWebRoot("Pics")
                .ConfigureAppConfiguration(x => x.AddConfiguration(configuration))
                .CaptureStartupErrors(false);
        })
        .UseSerilog(SeriLogger.configuration)
        .Build();

public partial class program
{
    public static string? NameSpace = typeof(Startup).Namespace; 
    public static string? AppName = "Catalog.Api";

}
