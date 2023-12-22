using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.CommandsNext;
using Microsoft.FeatureManagement.Mvc;
using Microsoft.FeatureManagement;
using FredBot.Extensions;

namespace FredBot.Attributes;

[AttributeUsage(AttributeTargets.Class)]
public class DiscordFeatureFlagsAttribute : CheckBaseAttribute
{

    static IFeatureManager FeatureManager;

    public override Task<bool> ExecuteCheckAsync(CommandContext ctx, bool help) => this.FlagEnabledAsync(ctx);



    public Task SendResponse(CommandContext ctx,bool success)
    {
        var config = ctx.Services.GetRequiredService<IConfiguration>();
        if(ctx.Command.CustomAttributes.Where(x => x.GetType() == typeof(FeatureGateLogMessageAttribute)).FirstOrDefault() is FeatureGateLogMessageAttribute message)
        {
            return message.Run(ctx, success);
        }
        return Task.CompletedTask;
    }

    public async Task<bool> FlagEnabledAsync(CommandContext ctx)
    {

        FeatureManager ??= ctx.Services.GetService<IFeatureManager>();

        if(FeatureManager is null)
        {
            return true;
        }



        if(ctx.Command.CustomAttributes.Where(x => x.GetType() == typeof(FeatureGateAttribute)).FirstOrDefault() is not FeatureGateAttribute attribute)
        {
            return true;
        }


        RequirementType type = attribute.RequirementType;

        if(type == RequirementType.Any)
        {
            foreach(string? feature in attribute.Features)
            {
                if(await FeatureManager.IsEnabledAsync(feature, ctx))
                {
                    await SendResponse(ctx,true);
                    return true;
                }
            }
            await SendResponse(ctx, false);
            return false;
        }
        else if(type == RequirementType.All)
        {
            List<bool> list = [];


            foreach(string feature in attribute.Features)
            {
                bool result = await FeatureManager.IsEnabledAsync(feature, ctx);
                list.Add(result);
            }

            var condition = list.All(x => x == true);
            await SendResponse(ctx, condition);
            return condition;
        }

        //should never go here
        return true;

    }
}
