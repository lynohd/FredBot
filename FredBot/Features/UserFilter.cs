using DSharpPlus.CommandsNext;
using DSharpPlus.Entities;
using Microsoft.FeatureManagement;

namespace FredBot.Features;
[FilterAlias(nameof(UserFilter))]
public class UserFilter : IContextualFeatureFilter<CommandContext>
{
    public Task<bool> EvaluateAsync(FeatureFilterEvaluationContext context, CommandContext command)
    {
        var settings = context.Parameters.Get<UserFilterSettings>();
        return Task.FromResult(settings.DiscordIds.Contains(command.User.Id));
    }
}

public class UserFilterSettings
{
    public ulong[] DiscordIds { get; set; }
}

