# FredBot

## How to Inject Services:
```csharp
[Installer(true, priority: 3000)]
public class ExampleInstaller: IServiceInstaller
{
    private readonly ILogger<ExampleInstaller> _logger;
    public ExampleInstaller(ILogger<ExampleInstaller> logger)
    {
        _logger = logger;
    }

    public void Install(IServiceCollection services, IConfiguration config, ref readonly int priority)
    {
       _logger.LogInformation("hi {priority}", priority);
    }
}
```

```csharp
[Installer(true, priority: 3000)]
```
the "priority: 3000" defines how high priority in the injection chain this installer has. the higher number the higher priority

## Commands
Check DSharpPlus Documentation for commands
but please use the Commands Folder for commands.
https://dsharpplus.github.io/DSharpPlus/articles/commands/intro.html

## Discord Events
For events we use MediatR
for a list of all events you can look in the "Messages" folder. if you want to listen to any of these messages you 
implement the INotificationHandler<YOUR_EVENT> with the event you want to listen to.

example
```csharp
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
```

you can also inject your own services into the handler through the constructor as its all handled by the framework.
