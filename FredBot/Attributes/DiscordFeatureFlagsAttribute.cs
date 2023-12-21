using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.CommandsNext;
using Microsoft.FeatureManagement;
using Microsoft.FeatureManagement.Mvc;

namespace FredBot.Attributes;

[AttributeUsage(AttributeTargets.Class)]
public class DiscordFeatureFlagsAttribute : CheckBaseAttribute
{
    public override Task<bool> ExecuteCheckAsync(CommandContext ctx, bool help) => this.FlagEnabledAsync(ctx);

    public async Task<bool> FlagEnabledAsync(CommandContext ctx)
    {
        IFeatureManager? _manager = ctx.Services.GetService<IFeatureManager>();

        if(_manager is null)
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
                if(await _manager.IsEnabledAsync(feature))
                {
                    return true;
                }
            }
            return false;
        }
        else if(type == RequirementType.All)
        {
            List<bool> list = [];


            foreach(string feature in attribute.Features)
            {
                bool result = await _manager.IsEnabledAsync(feature);
                list.Add(result);
            }

            return list.All(x => x == true);
        }

        return true;

    }
}
