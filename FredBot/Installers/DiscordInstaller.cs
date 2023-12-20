using System.Reflection;
using System.Windows.Input;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.Interactivity.Extensions;
using DSharpPlus.SlashCommands;
using FredBot;
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
    private readonly ILogger<DiscordInstaller> _logger;
    public DiscordInstaller(ILogger<DiscordInstaller> logger)
    {
        _logger = logger;
    }

    public void Install(IServiceCollection services, IConfiguration config, ref readonly int order)
    {
        services.AddSingleton<LeagueCustomsService>();

        var prefix = config["Discord:Prefix"] ?? DEFAULT_PREFIX;
        _logger.LogInformation("Prefix: {Prefix}", prefix);
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