using System.Diagnostics;
using DSharpPlus.Entities;
using FredBot.Events.ClientEvents.Messages;
using FredBot.Services;
using MediatR;

namespace FredBot.Events.ClientEvents.Handlers;

public class DiscordGuildAvailableHandler : INotificationHandler<OnDiscordGuildAvailable>
{
    public const bool SAFETY_SWITCH = true;
    private readonly ILogger<DiscordGuildAvailableHandler> _logger;
    private readonly IConfiguration _configuration;
    private readonly TimeGuessrService _service;

    public DiscordGuildAvailableHandler(
        ILogger<DiscordGuildAvailableHandler> logger,
        IConfiguration configuration,
        TimeGuessrService service)
    {
        _logger = logger;
        _configuration = configuration;
        _service = service;
    }


    //Guild ID, Channel ID
    Dictionary<ulong, ulong> Guilds = new Dictionary<ulong, ulong>() 
    {
        {761587351469424710, 761587351469424714} /* Test Server*/,
        {846917454533754910, 1169708733077671996 /*Freds*/}
    };

    public async Task Handle(OnDiscordGuildAvailable notification, CancellationToken cancellationToken)
    {
        var args = notification.Args;

        _logger.LogInformation("GuildAvailable {Guild}", args.Guild);

        var started = Stopwatch.GetTimestamp();
        var enabled = _configuration.GetValue<bool>("Discord:Scraping:Enabled");


        if(enabled && SAFETY_SWITCH)
        {
            if(Guilds.ContainsKey(args.Guild.Id))
            {
                DiscordMessageScraper scraper = new();
                var ch = args.Guild.GetChannel(Guilds[args.Guild.Id]);
                var lastMsg = await scraper.GetLastMessage(ch);
                var messages = await Task.Run(async () => await scraper.GetAllMessagesAsync(ch, lastMsg.Id), cancellationToken);

                var timeguessrMessages = messages.Where(x => x.Content.StartsWith("TimeGuessr")).ToList();

                _logger.LogInformation("Found {Amount} TimeGuessr Entries in {Guild}", timeguessrMessages.Count, args.Guild);
            }
        }

        _logger.LogInformation("{Handler} for {Guild} took {Seconds} seconds to complete",
            nameof(DiscordGuildAvailableHandler),
            args.Guild.Name,
            Stopwatch.GetElapsedTime(started).Seconds);
    }
}
