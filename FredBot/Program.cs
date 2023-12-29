using System.Reflection;
using FredBot.Extensions;
using FredBot.Features;
using FredBot.Models;
using FredBot.Services;
using Microsoft.FeatureManagement;
using Serilog;

var assembly = Assembly.GetExecutingAssembly();



await Host
    .CreateDefaultBuilder()
    .UseSerilog()
    .UseConsoleLifetime()
    .ConfigureHostConfiguration(x => x.AddUserSecrets(assembly).AddJsonFile("appsettings.json", false, true).Build())
    .ConfigureServices((ctx, services) =>
    {
        Log.Logger = new LoggerConfiguration()
            .WriteTo.Console()
            .CreateLogger();

        services.AddFeatureManagement(ctx.Configuration.GetSection("Features"));

        services.AddMediatR(x =>
        {
            x.RegisterServicesFromAssembly(assembly);
            x.NotificationPublisher = new MediatR.NotificationPublishers.TaskWhenAllPublisher();
        });

        services.AddLogging(x => x.ClearProviders().AddSerilog().AddJsonConsole());

        //services.AddGraphQLServer().AddQueryType<Player>();


        services.AddSingleton<TimeGuessrService>();
        services.InstallServices(ctx.Configuration, assembly);
    })
    .RunConsoleAsync();
await Log.CloseAndFlushAsync();
