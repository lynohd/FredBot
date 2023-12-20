namespace FredBot.Installers;

public interface IServiceInstaller
{
    /// <summary>
    /// you can get a logger from the constructor and
    /// If you want to access the order define a integer with any name in the constructor
    /// example: ctor(ILogger<Installer> installer, int order)
    /// </summary>
    /// <param name="services"></param>
    /// <param name="config"></param>
    public void Install(IServiceCollection services, IConfiguration config, ref readonly int priority);
}
