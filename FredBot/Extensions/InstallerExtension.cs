using System.Reflection;
using System.Runtime.CompilerServices;
using FredBot.Attributes;
using FredBot.Installers;
using Serilog;

namespace FredBot.Extensions;
public class InstallOrder
{
    public int Value { get; set; }

}
public static class InstallerExtension
{
    private static readonly Serilog.ILogger Logger = Log.Logger;

    [Obsolete("Use InstallWithLogger instead")]
    public static void Install(this IServiceCollection services, IConfiguration config, bool ordered = false, params Assembly[] assemblies)
    {
        int order = 0;
        assemblies
            .SelectMany(x => x.DefinedTypes)
            .Where(IsValidInstaller)
            .Where(HasAttribute)
            .Where(IsEnabled)
            .OrderByDescending(x => { return order = GetAttribute<InstallerAttribute>(x).Priority; })
            .Select(Activator.CreateInstance)
            .Cast<IServiceInstaller>()
            .ToList()
            .ForEach(installer =>
            {
                installer?.Install(services, config, ref order);
                Logger?.Information("Running Installer {Service}", installer.GetType());
            });
    }

    public static void InstallServices(this IServiceCollection services, IConfiguration config,params Assembly[] assemblies)
    {
        int order = 0;
        assemblies
            .SelectMany(x => x.DefinedTypes)
            .Where(IsValidInstaller)
            .Where(HasAttribute)
            .Where(IsEnabled)
            .OrderByDescending(x =>
            {
                return order = GetAttribute<InstallerAttribute>(x).Priority;
            })
            .Select(type =>
            {
                return ActivatorUtilities.CreateInstance(services.BuildServiceProvider(), type.AsType());
            })
            .Cast<IServiceInstaller>()
            .ToList()
            .ForEach(installer => installer?.Install(services, config, ref order));
    }

    static bool IsValidInstaller(TypeInfo info) =>
        info.IsAssignableTo(typeof(IServiceInstaller)) == true && info.IsInterface == false;

    static bool HasAttribute(TypeInfo info) =>
        GetAttribute<InstallerAttribute>(info) != null;
    
    static TAttribute? GetAttribute<TAttribute>(TypeInfo info) where TAttribute : Attribute =>
        info.GetCustomAttribute<TAttribute>();
    

    static bool IsEnabled(TypeInfo info) =>
        GetAttribute<InstallerAttribute>(info).Enabled;
}
