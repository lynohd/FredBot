using DSharpPlus.CommandsNext;
using FredBot.Extensions;

namespace FredBot.Attributes;

[AttributeUsage(AttributeTargets.Method)]
public class FeatureGateLogMessageAttribute : Attribute
{
    private readonly string _featureEnabledMessage;
    private readonly string _featureDisabledMessage;

    private readonly ResponseMode _respondType;


    private static ILogger<FeatureGateLogMessageAttribute> Logger;

    [Flags]
    public enum ResponseMode
    {
        None = 0,
        Logger = 1,
        DiscordResponse = 2,
        Both = Logger|DiscordResponse
    }


    public FeatureGateLogMessageAttribute(string featureEnabledMessage, string featureDisabledMessage, ResponseMode respondType)
    {
        _featureEnabledMessage = featureEnabledMessage;
        _featureDisabledMessage = featureDisabledMessage;
        _respondType = respondType;
    }

    public async Task Run(CommandContext ctx, bool enabled)
    {
        Logger ??= ctx.Services.GetService<ILogger<FeatureGateLogMessageAttribute>>();

        if(_respondType == ResponseMode.None)
            return;

        if(_respondType == ResponseMode.Both)
        {
            var taskList = new TaskList() | RespondAsync(ctx, enabled) | LogInformation(ctx);

            await taskList;
            return;
        }

        if(_respondType == ResponseMode.DiscordResponse)
        {
            await RespondAsync(ctx, enabled);
            return;
        }

        if(_respondType == ResponseMode.Logger)
        {
            await LogInformation(ctx);
            return;
        }
    }
    public async Task RespondAsync(CommandContext ctx, bool enabled)
    {
        await ctx.RespondAsync(enabled ? _featureEnabledMessage : _featureDisabledMessage);
    }

    public async Task LogInformation (CommandContext ctx)
    {
        Logger.LogInformation("Hi");
    }
}
