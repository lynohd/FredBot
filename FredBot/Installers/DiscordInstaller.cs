using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.Interactivity.Extensions;
using FredBot;
using FredBot.Attributes;
using FredBot.Installers;
using FredBot.Services;
using Serilog;

namespace FredBot.Installers;

[Installer(true, 1000)]
public class DiscordInstaller(ILogger<DiscordInstaller> logger) : IServiceInstaller
{

    const string DEFAULT_PREFIX = "!";

    public void Install(IServiceCollection services, IConfiguration config, ref readonly int order)
    {
        services.AddSingleton<LeagueCustomsService>();

        var prefix = config["Discord:Prefix"] ?? DEFAULT_PREFIX;
        logger.LogInformation("Prefix: {Prefix}", prefix);
        var client = new DiscordClient(new DiscordConfiguration()
        {
            LoggerFactory = new LoggerFactory().AddSerilog(),
            MinimumLogLevel = LogLevel.Warning,
            Token = config["Discord:Token"]!,
            Intents = DiscordIntents.All
        });

        var commands = client.UseCommandsNext(new CommandsNextConfiguration()
        {
            PrefixResolver = (msg) => Task.FromResult(msg.GetStringPrefixLength(prefix)),
            Services = services.BuildServiceProvider()
        });
        client.UseInteractivity();
        services.AddSingleton(client);
        services.AddHostedService<DiscordBot>();
    }
}