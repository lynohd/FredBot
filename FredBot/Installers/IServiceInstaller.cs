namespace FredBot.Installers;

public interface IServiceInstaller
{
    public void Install(IServiceCollection services, IConfiguration config);
}
