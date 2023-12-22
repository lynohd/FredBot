using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.CommandsNext;
using Microsoft.FeatureManagement.Mvc;
using Microsoft.FeatureManagement;
using FredBot.Extensions;

namespace FredBot.Attributes;

[AttributeUsage(AttributeTargets.Class)]
public class DiscordFeatureFlagsAttribute : CheckBaseAttribute
{

    static IFeatureManager? FeatureManager;
    static IConfiguration? Configuration;

    public override Task<bool> ExecuteCheckAsync(CommandContext ctx, bool help) => this.FlagEnabledAsync(ctx);



    public async Task<bool> FlagEnabledAsync(CommandContext ctx)
    {
        FeatureManager ??= ctx.Services.GetService<IFeatureManager>();
        Configuration ??= ctx.Services.GetRequiredService<IConfiguration>();

        if(FeatureManager is null || !(ctx.Command.CustomAttributes.FirstOrDefault(x => x.GetType() == typeof(FeatureGateAttribute)) is FeatureGateAttribute attribute))
        {
            return true;
        }

        RequirementType type = attribute.RequirementType;

        async Task<bool> CheckFeatureAsync(string feature) => await FeatureManager!.IsEnabledAsync(feature, ctx);

        var result = type switch
        {
            RequirementType.Any => await attribute.Features.AnyAsync(CheckFeatureAsync),
            RequirementType.All => await attribute.Features.AllAsync(CheckFeatureAsync),
            _ => throw new Exception("something went wrong here..")
        };

        var message = ctx.Command.CustomAttributes
            .FirstOrDefault(x => x.GetType() == typeof(FeatureGateLogMessageAttribute)) as FeatureGateLogMessageAttribute;

        if(message is not null)
        await message?.Run(ctx, result);

        return result;
    }
}
