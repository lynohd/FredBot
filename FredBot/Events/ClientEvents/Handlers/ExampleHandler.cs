using FredBot.Events.ClientEvents.Messages;
using MediatR;

namespace FredBot.Events.ClientEvents.Handlers;

public class ExampleHandler(ILogger<ExampleHandler> logger) : INotificationHandler<OnDiscordMessageCreated>
{
    public Task Handle(OnDiscordMessageCreated notification, CancellationToken cancellationToken)
    {
        logger.LogInformation("{user} sent message: {message}", notification.Args.Author ,notification.Args.Message.Content);
        return Task.CompletedTask;
    }
}
