using DSharpPlus.Entities;
using System.Collections.Generic;
namespace FredBot.Services;

public class DiscordMessageScraper
{


    public async Task<DiscordMessage> GetLastMessage(DiscordChannel channel) => await channel.GetMessagesAsync(1).FirstAsync();

    public async Task<int> GetTotalMessagesInChannel(DiscordChannel channel, ulong startFromId)
    {
        int amount = 0;
        var allMessages = new List<DiscordMessage>();
        ulong beforeId = startFromId;

        do
        {
            var messages = await channel.GetMessagesBeforeAsync(beforeId, 100).ToListAsync();
            if(messages.Count == 0)
                break;
            amount += messages.Count;
            beforeId = messages.Last().Id;
        } while(true);

        return amount;

    }

    public async Task<IReadOnlyList<DiscordMessage>> GetAllMessagesAsync(DiscordChannel channel, ulong startFromId)
    {
        var allMessages = new List<DiscordMessage>();
        ulong beforeId = startFromId;

        do
        {
            var messages = await channel.GetMessagesBeforeAsync(beforeId, 100).ToListAsync();
            if(messages.Count == 0)
                break;

            allMessages.AddRange(messages);
            beforeId = messages.Last().Id;
        } while(true);

        return allMessages;
    }

}
