using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using DSharpPlus.EventArgs;
using FredBot.Events.ClientEvents.Messages;
using MediatR;

namespace FredBot.Events.ClientEvents.Handlers;

public class GenericDiscordEventHandler(ILogger<GenericDiscordEventHandler> logger) : INotificationHandler<OnGenericDiscordMessage>
{
    public Task Handle(OnGenericDiscordMessage notification, CancellationToken cancellationToken)
    {
        logger.LogInformation("GenericDiscordEventHandler::{Message}", GetPropertyInfo(notification.Args));


        if (notification.Args is GuildCreateEventArgs args)
        {
            //_logger.LogInformation(args.Guild);
        }
        return Task.CompletedTask;

    }


    public string GetPropertyInfo([DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicProperties)] object type)
    {
        var fields = type.GetType().GetProperties(BindingFlags.GetProperty | BindingFlags.Public | BindingFlags.Instance)
            .Where(x => x.Name != "Handled")
            .Select(x => x.Name).ToList();



        return string.Join(", ", fields);
    }
}
