using DSharpPlus.CommandsNext;
using DSharpPlus;
using System.Reflection;
using MediatR;
using System.Diagnostics;
using FredBot.Commands.Converters;
using FredBot.Events.ClientEvents.Messages;
using FredBot.Events.CommandsEvents.Messages;

namespace FredBot;

public class DiscordBot : IHostedService
{
    readonly ILogger<DiscordBot> _logger;
    readonly DiscordClient _client;
    readonly IMediator _mediator;
    readonly CommandsNextExtension _commands;

    public DiscordBot(DiscordClient client, ILogger<DiscordBot> logger, IMediator mediator)
    {
        _client = client;
        _commands = _client.GetCommandsNext();
        _commands.RegisterCommands(Assembly.GetExecutingAssembly());
        _commands.RegisterConverter(new CustomArgumentConverter());
        
        _logger = logger;
        _mediator = mediator;
        RegisterEventHandlers();
    }

    private void RegisterEventHandlers()
    {
        _client.ComponentInteractionCreated += (sender, args) => _mediator.Publish(new OnComponentInteract(sender, args));
        _client.GuildAvailable += (sender, args) => _mediator.Publish(new OnDiscordGuildAvailable(sender, args));
        _client.MessageCreated += (sender, args) =>
        {
            //prevent recursion
            if (args.Author == sender.CurrentUser)
                return Task.CompletedTask;

            return _mediator.Publish(new OnDiscordMessageCreated(sender, args));
        };


        _commands.CommandErrored += (ext, args) =>
        {
            _mediator.Publish(new OnCommandError(ext, args));
            _logger.LogInformation("CommandErrored: {User} {Args}", args.Context.User, args.Command);
            return Task.CompletedTask;
        };
        _commands.CommandExecuted += (ext, args) =>
        {
            _mediator.Publish(new OnCommandExecuted(ext, args));
            _logger.LogInformation("{User} Executed Command: {Args}", args.Context.User, args.Command);
            return Task.CompletedTask;
        };
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        var started = Stopwatch.GetTimestamp();
        _logger.LogInformation("Discord bot starting..");
        await _client.ConnectAsync();
        _logger.LogInformation("Took {ms} ms to start.", Stopwatch.GetElapsedTime(started).Milliseconds);
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        var started = Stopwatch.GetTimestamp();
        _logger.LogInformation("Discord bot stopping..");
        await _client.DisconnectAsync();
        _logger.LogInformation("Took {ms} ms to stop.", Stopwatch.GetElapsedTime(started).Milliseconds);
        _client.Dispose();
    }
}
