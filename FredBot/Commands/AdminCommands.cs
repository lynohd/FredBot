using System.Diagnostics;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using FredBot.Services;

namespace FredBot.Commands;


[ModuleLifespan(ModuleLifespan.Singleton)]
public class AdminCommands : BaseCommandModule
{
    private readonly LeagueCustomsService _customsService;
    private readonly ILogger<AdminCommands> _logger;
    private long StartTime;

    public AdminCommands(ILogger<AdminCommands> logger, LeagueCustomsService customsService)
    {
        _logger = logger;
        _customsService = customsService;
        Console.WriteLine(Guid.NewGuid());
    }

    public override Task BeforeExecutionAsync(CommandContext ctx)
    {
        StartTime = Stopwatch.GetTimestamp();
        return base.BeforeExecutionAsync(ctx);
    }

    public override Task AfterExecutionAsync(CommandContext ctx)
    {
        var time = Stopwatch.GetElapsedTime(StartTime).Milliseconds;
        _logger.LogInformation("Command {Command} took {ms} Milliseconds to execute",ctx.Command.Name, time);
        return base.AfterExecutionAsync(ctx);
    }


    [Command("setup")]
    public async Task SetupCommand(CommandContext ctx)
    {
        await _customsService.Setup(ctx.Guild);
        await ctx.Message.DeleteAsync();
    }



    [Command("merge")]
    public async Task MergeCommand(CommandContext ctx)
    {
        await _customsService.MergeChannels(ctx.Guild);
    }


    [Command("exclude")]
    public async Task ExcludeMember(CommandContext ctx, DiscordMember member)
    {

        await _customsService.ExcludeMember(ctx.Message, member);
    }

    [Command("include")]
    public async Task  IncludeMember(CommandContext ctx, DiscordMember member)
    {
        await _customsService.IncludeMember(ctx.Message, member);
    }

    [Command("split")]
    public async Task SplitTeams(CommandContext ctx)
    {
        await _customsService.SplitTeams(ctx.Guild);
    }

    [Command("randomize")]
    public async Task RandomizeTeams(CommandContext ctx, bool silent = true)
    {
        await _customsService.RandomizeTeams(ctx.Member, ctx.Message, silent);
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
