using MediatR;
using FredBot.Events.ClientEvents.Messages;

namespace FredBot.Events.ClientEvents.Handlers;

public class DiscordMessageHandler : INotificationHandler<OnDiscordMessageCreated>
{
    public async Task Handle(OnDiscordMessageCreated notification, CancellationToken cancellationToken)
    {
        var args = notification.Args;
        var sender = notification.Sender;
        var guild = args.Guild;
        var message = args.Message;

        return;

        //if(guild.Id == 846917454533754910)
        //    return;

        //if(message.Content.StartsWith("TimeGuessr"))
        //{
        //    await args.Message.CreateReactionAsync(DiscordEmoji.FromName(sender, ":thumbsup:"));
        //}

        if (guild.Id == 846917454533754910 && args.Channel.Id == 1169708733077671996)
        {
            if (message.Content.StartsWith("TimeGuessr"))
            {
            }

        }

    }
}
