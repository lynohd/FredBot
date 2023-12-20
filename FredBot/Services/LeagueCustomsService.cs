#define TESTING
using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using FredBot.Extensions;

namespace FredBot.Services;

public class LeagueCustomsService/*(ILogger<LeagueCustomsService> logger)*/
{
    ulong TEAM1VC = 1095102384688074772;
    ulong TEAM2VC = 1095102436726804600;
#if !TESTING
    ulong TEXT_CHANNEL = 1185724161444806687;
#else
    ulong TEXT_CHANNEL = 761587351469424714;
#endif
    ulong LOBBY_VC = 1095102384688074772;

    List<DiscordMember>? team1;
    List<DiscordMember>? team2;
    List<DiscordMember> _excluded = new();

    private readonly ILogger<LeagueCustomsService> logger;
    public LeagueCustomsService(ILogger<LeagueCustomsService> logger)
    {
        this.logger = logger;
        Console.WriteLine("service: " + Guid.NewGuid());
    }

    public async Task MergeChannels(DiscordGuild guild)
    {
        var channels = await guild.GetChannelsAsync();
        var vcs = channels.Where(x =>  x.Type == ChannelType.Voice && x.Id == TEAM1VC || x.Id == TEAM2VC);

        foreach(var vc in vcs)
        {
            foreach(var member in vc.Users)
            {
                await member.PlaceInAsync(guild.GetChannel(LOBBY_VC));
            }
        }
    }
    public async Task ExcludeMember(DiscordMessage message, DiscordMember member)
    {
        if(member.VoiceState != null)
        {
            _excluded.Add(member);
            await message.RespondAsync($"{member} has been excluded");
        }
        else
        {
            await message.RespondAsync("not in voice channel");
        }
    }
    public async Task IncludeMember(DiscordMessage message, DiscordMember member)
    {
        if(_excluded.Contains(member))
        {
            _excluded.Remove(member);
            await message.RespondAsync($"{member} has been removed from the exclusion list");
        }
    }

    public async Task SplitTeams(DiscordGuild guild)
    {
        if(team1 is null || team2 is null)
            return;
        team1.ForEach(x => x.PlaceInAsync(guild.GetChannel(TEAM1VC)));
        team2.ForEach(x => x.PlaceInAsync(guild.GetChannel(TEAM2VC)));
    }

    public async Task RandomizeTeams(DiscordMember member, DiscordMessage message, bool silent = true)
    {
        var candidates = member.VoiceState.Channel.Users.ExceptBy(_excluded.Select(x => x.Id), x => x.Id).ToList();
        logger.LogInformation("Generating team with candidates {Candidates}", candidates);
        candidates.Shuffle();

        team1 = candidates.Take(candidates.Count / 2).ToList();
        team2 = candidates.Skip(candidates.Count / 2).ToList();


        var embeds = new DiscordEmbedBuilder()
            .AddField("Team 1", string.Join(", ", team1.Select(x => silent ? x.Username : x.Mention)))
            .AddField("Team 2", string.Join(", ", team2.Select(x => silent ? x.Username : x.Mention)))
            .AddField("Excluded Members", _excluded.Count <= 0 ? "None": string.Join(", ", _excluded))
            .Build();

        var builder = new DiscordMessageBuilder()
        .WithContent("Generated Teams")
        .WithEmbed(embeds)
        .AddComponents(Buttons3);

        await message.RespondAsync(builder);
    }

    public async Task GenerateArenaTeam(DiscordMember member, DiscordMessage message, bool silent = true)
    {
        var candidates = member.VoiceState.Channel.Users.ExceptBy(_excluded.Select(x =>x.Id), x => x.Id).ToList();


        if(candidates.Count <= 2) return;


        logger.LogInformation("Generating arena team with candidates {Candidates}", candidates);
        candidates.Shuffle();

        var at1 = candidates[0..2];
        var at2 = candidates[2..4];
        var at3 = candidates[4..6];
        var at4 = candidates[6..8];



        var builder = new DiscordEmbedBuilder();

        if(candidates.Count <= 4)
        {
            builder
                .AddField("Team 1", string.Join(", ", at1.Select(x => silent ? x.Username : x.Mention)))
                .AddField("Team 2", string.Join(", ", at2.Select(x => silent ? x.Username : x.Mention)));
        }
        if(candidates.Count >= 8)
        {
            builder
                .AddField("Team 3", string.Join(", ", at3.Select(x => silent ? x.Username : x.Mention)))
                .AddField("Team 4", string.Join(", ", at4.Select(x => silent ? x.Username : x.Mention)))
                .AddField("Excluded Members", _excluded.Count <= 0 ? "None" : string.Join(", ", _excluded));
        }


        var messageBuilder = new DiscordMessageBuilder()
            .WithContent("Generated Arena Teams")
            .WithEmbed(builder.Build())
            .AddComponents(new DiscordButtonComponent(ButtonStyle.Danger, "btn_delete", "Delete"));

        await message.RespondAsync(builder);
    }


    static readonly DiscordButtonComponent[] Buttons1 =
    {
        new DiscordButtonComponent(ButtonStyle.Success, "btn_randomize", "Make Teams"),
        new DiscordButtonComponent(ButtonStyle.Danger, "btn_merge", "Merge VCs"),
        new DiscordButtonComponent(ButtonStyle.Danger, "btn_massmove", "Mass Move", true),
    };

    static readonly DiscordButtonComponent[] Buttons2 =
    {
        new DiscordButtonComponent(ButtonStyle.Primary, "btn_exclude", "Exclude User"),
        new DiscordButtonComponent(ButtonStyle.Primary, "btn_include", "Include User"),
    };

    static readonly DiscordButtonComponent[] Buttons3 =
    {
        new DiscordButtonComponent(ButtonStyle.Primary, "btn_split", "Split VCs"),
        new DiscordButtonComponent(ButtonStyle.Danger, "btn_delete", "Delete"),
    };

    public async Task Setup(DiscordGuild guild)
    {

        var builder = new DiscordMessageBuilder()
            .WithContent("League Customs")
            .AddComponents(Buttons1)
            .AddComponents(Buttons2)
            .AddComponents(new DiscordButtonComponent(ButtonStyle.Success, "btn_arena", "Make Arena Teams"));
        await guild.GetChannel(TEXT_CHANNEL).SendMessageAsync(builder);
    }


    public async Task OnButtonClick(ComponentInteractionCreateEventArgs args)
    {
        var guild = args.Guild;
        switch(args.Id)
        {
            case "btn_merge":
            await MergeChannels(guild);
            break;

            case "btn_randomize":
            await RandomizeTeams(await guild.GetMemberAsync(args.User.Id), args.Message);
            break;

            case "btn_split":
            await SplitTeams(guild);
            break;

            case "btn_massmove":
            {
                var channels = await guild.GetChannelsAsync();
                var vcs = channels.Where(x => x.Type is ChannelType.Voice);

                var member = await args.Guild.GetMemberAsync(args.User.Id);



                foreach(var vc in vcs)
                {
                    foreach(var user in vc.Users)
                    {
                        await user.PlaceInAsync(member.VoiceState.Channel);
                    }
                }
                break;
            }

            case "btn_delete":
            {
                team1?.Clear();
                team2?.Clear();
                await args.Message.DeleteAsync();
                break;
            }

            case "btn_exclude":
            break;

            case "btn_include":
            break;

            case "btn_arena":
            {
                await GenerateArenaTeam(await guild.GetMemberAsync(args.User.Id), args.Message);
                break;
            }

            default:
            break;
        }
    }

    public async Task SelectExcludeMember()
    {

    }
}
