using FredBot.Attributes;

namespace FredBot.Installers;

[Installer(true, priority: 3000)]
public class ExampleInstaller(ILogger<ExampleInstaller> logger) : IServiceInstaller
{
    public void Install(IServiceCollection services, IConfiguration config, ref readonly int priority)
    {
       logger.LogInformation("hi {order}", priority);
    }
}
