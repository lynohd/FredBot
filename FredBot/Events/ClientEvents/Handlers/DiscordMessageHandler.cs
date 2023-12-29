using MediatR;
using FredBot.Events.ClientEvents.Messages;
using FredBot.Services;

namespace FredBot.Events.ClientEvents.Handlers;

public class DiscordMessageHandler : INotificationHandler<OnDiscordMessageCreated>
{

    readonly TimeGuessrService _service;

    public DiscordMessageHandler(TimeGuessrService service)
    {
        _service = service;
    }

    public async Task Handle(OnDiscordMessageCreated notification, CancellationToken cancellationToken)
    {
        var args = notification.Args;
        var sender = notification.Sender;
        var guild = args.Guild;
        var message = args.Message;


        if(message.ChannelId == 1169708733077671996 || message.ChannelId == 761587351469424714)
        {
            if(message.Content.StartsWith("TimeGuessr"))
            await _service.HandleMessage(sender, message);
        }
    }
}
