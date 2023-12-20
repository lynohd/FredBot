# FredBot

How to Inject Services:
```csharp
[Installer(true, 3000)]
public class ExampleInstaller: IServiceInstaller
{
    private readonly ILogger<ExampleInstaller> _logger;
    public ExampleInstaller(ILogger<ExampleInstaller> logger)
    {
        _logger = logger;
    }

    public void Install(IServiceCollection services, IConfiguration config, ref readonly int order)
    {
       _logger.LogInformation("hi {order}", order);
    }
}
```

```csharp
[Installer(true, 3000)]
```
the 3000 defines how high priority in the injection chain this installer has. the higher number the higher priority
