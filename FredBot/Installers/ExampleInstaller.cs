using FredBot.Attributes;

namespace FredBot.Installers;

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
       _logger.LogInformation("hi {order}", priority);
    }
}
