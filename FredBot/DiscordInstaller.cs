using System.Reflection;
using System.Windows.Input;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.Interactivity.Extensions;
using DSharpPlus.SlashCommands;
using FredBot.Attributes;
using FredBot.Commands.Converters;
using FredBot.Extensions;
using FredBot.Installers;
using FredBot.Services;
using MediatR;
using Serilog;

[Installer(true, 1000)]
public class DiscordInstaller : IServiceInstaller
{

    const string DEFAULT_PREFIX = "!";
    private static Serilog.ILogger Logger = Log.Logger;
    public void Install(IServiceCollection services, IConfiguration config)
    {
        services.AddSingleton<LeagueCustomsService>();
        var prefix = config["Prefix"] ?? DEFAULT_PREFIX;
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