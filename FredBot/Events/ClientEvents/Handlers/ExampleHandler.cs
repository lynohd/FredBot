using FredBot.Events.ClientEvents.Messages;
using MediatR;

namespace FredBot.Events.ClientEvents.Handlers;

public class ExampleHandler : INotificationHandler<OnDiscordMessageCreated>
{
    private readonly ILogger<ExampleHandler> _logger;

    public ExampleHandler(ILogger<ExampleHandler> logger)
    {
        _logger = logger;
    }
    public async Task Handle(OnDiscordMessageCreated notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("{user} sent message: {message}", notification.Args.Author ,notification.Args.Message.Content);
    }
}
