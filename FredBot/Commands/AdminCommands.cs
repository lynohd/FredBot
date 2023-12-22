using System.Diagnostics;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using FredBot.Attributes;
using FredBot.Services;
using Microsoft.FeatureManagement;
using Microsoft.FeatureManagement.Mvc;

namespace FredBot.Commands;


[ModuleLifespan(ModuleLifespan.Singleton)]
[DiscordFeatureFlags]
public class AdminCommands(ILogger<AdminCommands> logger, LeagueCustomsService customsService) : BaseCommandModule
{
    private long StartTime;

    public override Task BeforeExecutionAsync(CommandContext ctx)
    {
        StartTime = Stopwatch.GetTimestamp();
        return base.BeforeExecutionAsync(ctx);
    }

    public override Task AfterExecutionAsync(CommandContext ctx)
    {
        var time = Stopwatch.GetElapsedTime(StartTime).Milliseconds;
        logger.LogInformation("Command {Command} took {ms} Milliseconds to execute",ctx.Command.Name, time);
        return base.AfterExecutionAsync(ctx);
    }

    [Command("test")]
    [FeatureGate(RequirementType.Any, "TestCommand", "t2")]
    [FeatureGateLogMessage("Its enabled", "its disabled", FeatureGateLogMessageAttribute.ResponseMode.Both)]
    public async Task Test(CommandContext ctx)
    {
        await ctx.RespondAsync("hi");
    }

    [Command("setup")]
    public async Task SetupCommand(CommandContext ctx)
    {
        await customsService.Setup(ctx.Guild);
        await ctx.Message.DeleteAsync();
    }



    [Command("merge")]
    public async Task MergeCommand(CommandContext ctx)
    {
        await customsService.MergeChannels(ctx.Guild);
    }


    [Command("exclude")]
    public async Task ExcludeMember(CommandContext ctx, DiscordMember member)
    {

        await customsService.ExcludeMember(ctx.Message, member);
    }

    [Command("include")]
    public async Task  IncludeMember(CommandContext ctx, DiscordMember member)
    {
        await customsService.IncludeMember(ctx.Message, member);
    }

    [Command("split")]
    public async Task SplitTeams(CommandContext ctx)
    {
        await customsService.SplitTeams(ctx.Guild);
    }

    [Command("randomize")]
    public async Task RandomizeTeams(CommandContext ctx, bool silent = true)
    {
        await customsService.RandomizeTeams(ctx.Member, ctx.Message, silent);
    }

    [Command("moveall")]
    public async Task MoveAll(CommandContext ctx, DiscordChannel? ch = null)
    {
        var guild = ctx.Guild;
        var channels = await guild.GetChannelsAsync();
        var vcs = channels.Where(x => x.Type is ChannelType.Voice);


        foreach(var vc in vcs)
        {
            foreach(var user in vc.Users)
            {
                await user.PlaceInAsync(ch ?? ctx.Member.VoiceState.Channel);
            }
        }
        await ctx.Message.CreateReactionAsync(DiscordEmoji.FromName(ctx.Client, ":thumbsup:"));
    }


}
