<<<<<<< HEAD:FredBot/Events/ClientEvents/Handlers/ComponentInteractHandler.cs
﻿using FredBot.Events.ClientEvents.Messages;
using FredBot.Services;
using MediatR;

namespace FredBot.Events.ClientEvents.Handlers;

public class ComponentInteractHandler(LeagueCustomsService service) : INotificationHandler<OnComponentInteract>
{
    public async Task Handle(OnComponentInteract notification, CancellationToken cancellationToken)
    {
        await service.OnButtonClick(notification.Args);
=======
﻿using FredBot.Events.Models;
using FredBot.Services;
using MediatR;

namespace FredBot.Events.Handlers;

public class ComponentInteractHandler : INotificationHandler<OnComponentInteract>
{
    LeagueCustomsService _service;

    public ComponentInteractHandler(LeagueCustomsService service)
    {
        _service = service;
    }

    public async Task Handle(OnComponentInteract notification, CancellationToken cancellationToken)
    {
       await _service.OnButtonClick(notification.Args);
>>>>>>> 717168574e1d5d5ec6c80d4ab603280a584ae3be:FredBot/Events/Handlers/ComponentInteractHandler.cs
    }
}
