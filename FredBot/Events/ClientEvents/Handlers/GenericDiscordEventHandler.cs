using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using DSharpPlus.EventArgs;
using FredBot.Events.ClientEvents.Messages;
using MediatR;

namespace FredBot.Events.ClientEvents.Handlers;

public class GenericDiscordEventHandler : INotificationHandler<OnGenericDiscordMessage>
{

    readonly ILogger<GenericDiscordEventHandler> _logger;

    public GenericDiscordEventHandler(ILogger<GenericDiscordEventHandler> logger)
    {
        _logger = logger;
    }

    public async Task Handle(OnGenericDiscordMessage notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("GenericDiscordEventHandler::{Message}", GetPropertyInfo(notification.Args));


        if (notification.Args is GuildCreateEventArgs args)
        {
            //_logger.LogInformation(args.Guild);
        }
    }


    public string GetPropertyInfo([DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicProperties)] object type)
    {
        var fields = type.GetType().GetProperties(BindingFlags.GetProperty | BindingFlags.Public | BindingFlags.Instance)
            .Where(x => x.Name != "Handled")
            .Select(x => x.Name).ToList();



        return string.Join(", ", fields);
    }
}
