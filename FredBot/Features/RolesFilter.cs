using DSharpPlus.CommandsNext;
using DSharpPlus.Entities;
using Microsoft.FeatureManagement;

namespace FredBot.Features;

[FilterAlias(nameof(RolesFilter))]
public class RolesFilter : IContextualFeatureFilter<CommandContext>
{
    public Task<bool> EvaluateAsync(FeatureFilterEvaluationContext context, CommandContext command)
    {
        var settings = context.Parameters.Get<RolesFilterSettings>();


        var userRoles = command.Member.Roles.Select(x => x.Name).ToList();
        var allowedRoles = settings.Roles;


        var allowed = userRoles.Intersect(allowedRoles).ToList();

        var result = allowed.Count > 0;
        return Task.FromResult(result);
    }
}

public class RolesFilterSettings
{
    public string[] Roles { get; set; }
}

