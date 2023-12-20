using System.Diagnostics;
using DSharpPlus.Entities;
using FredBot.Events.ClientEvents.Messages;
using FredBot.Services;
using MediatR;

namespace FredBot.Events.ClientEvents.Handlers;

public class DiscordGuildAvailableHandler(ILogger<DiscordGuildAvailableHandler> logger) : INotificationHandler<OnDiscordGuildAvailable>
{
    const bool SCRAPE_PREVIOUS_MESSAGES = false;


    public async Task Handle(OnDiscordGuildAvailable notification, CancellationToken cancellationToken)
    {
        var args = notification.Args;

        logger.LogInformation("GuildAvailable {Guild}", args.Guild);

        var started = Stopwatch.GetTimestamp();

        DiscordMessageThing message = new();

        if (SCRAPE_PREVIOUS_MESSAGES)
        {
            if (args.Guild.Id == 846917454533754910)
            {
                var ch = args.Guild.GetChannel(1169708733077671996);
                var msgs = await message.GetPreviousMessagesStartsWith(ch, 20, "TimeGuessr");
                logger.LogInformation("count {count}", msgs.Count);
            }
        }

        logger.LogInformation("{Handler} for {Guild} took {Seconds} seconds to complete",
            nameof(DiscordGuildAvailableHandler),
            args.Guild.Name,
            Stopwatch.GetElapsedTime(started).Seconds);
    }
}
