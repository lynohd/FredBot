using System.Reflection;
using System.Runtime.CompilerServices;
using FredBot.Attributes;
using FredBot.Installers;
using Serilog;

namespace FredBot.Extensions;

public static class InstallerExtension
{
    private static Serilog.ILogger Logger = Log.Logger;

    public static void Install(this IServiceCollection services, IConfiguration config, bool ordered = false, params Assembly[] assemblies) =>
        assemblies
            .SelectMany(x => x.DefinedTypes)
            .Where(IsValidInstaller)
            .Where(HasAttribute<InstallerAttribute>)
            .Where(IsEnabled)
            .OrderByDescending(x => GetAttribute<InstallerAttribute>(x).Priority)
            .Select(Activator.CreateInstance)
            .Cast<IServiceInstaller>()
            .ToList()
            .ForEach(installer => {
                installer?.Install(services, config);
                Logger?.Information("Running Installer {Service}", installer.GetType());
            });
    

    static bool IsValidInstaller(TypeInfo info) =>
        info.IsAssignableTo(typeof(IServiceInstaller)) == true && info.IsInterface == false;

    static bool HasAttribute<TAttribute>(TypeInfo info) where TAttribute : Attribute =>
        GetAttribute<TAttribute>(info) != null;
    
    static TAttribute GetAttribute<TAttribute>(TypeInfo info) where TAttribute : Attribute =>
        info.GetCustomAttribute<TAttribute>();
    

    static bool IsEnabled(TypeInfo info) =>
        GetAttribute<InstallerAttribute>(info).Enabled;
}
