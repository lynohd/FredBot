using FredBot.Events.ClientEvents.Messages;
using FredBot.Services;
using MediatR;

namespace FredBot.Events.ClientEvents.Handlers;

public class ComponentInteractHandler(LeagueCustomsService service) : INotificationHandler<OnComponentInteract>
{
    public async Task Handle(OnComponentInteract notification, CancellationToken cancellationToken)
    {
        await service.OnButtonClick(notification.Args);
    }
}
