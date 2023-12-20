using Microsoft.Extensions.Options;

namespace FredBot.Extensions;

public static class DIExtensions
{
    public static T GetOptionsValue<T>(this IServiceProvider serviceProvider) where T : class
    {
        var value = serviceProvider.GetService<IOptions<T>>().Value;
        return value;
    }
    public static T GetRequiredOptionsValue<T>(this IServiceProvider serviceProvider) where T : class
    {
        var value = serviceProvider.GetRequiredService<IOptions<T>>().Value;
        return value;
    }
}
