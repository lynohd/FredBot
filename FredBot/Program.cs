using System.Reflection;
using FredBot.Extensions;
using FredBot.Services;
using Serilog;

var assembly = Assembly.GetExecutingAssembly();



await Host
    .CreateDefaultBuilder()
    .UseSerilog()
    .UseConsoleLifetime()
    .ConfigureHostConfiguration(x =>x.AddUserSecrets(assembly).Build())
    .ConfigureServices((ctx, services) =>
    {
        Log.Logger = new LoggerConfiguration()
            .WriteTo.Console()
            .CreateLogger();

        services.AddMediatR(x => x.RegisterServicesFromAssembly(assembly));
        services.AddLogging(x => x.ClearProviders().AddSerilog());
        services.Install(config: ctx.Configuration, ordered: true,assemblies: assembly);
    })
    .RunConsoleAsync();
await Log.CloseAndFlushAsync();
