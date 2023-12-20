using DSharpPlus.Entities;
using System.Collections.Generic;

namespace FredBot.Services;

public class DiscordMessageThing
{
    public List<DiscordMessage> GetPreviousMessages(DiscordChannel channel, int amount)
    {

        List<DiscordMessage> list = new();
        var msg = channel.GetMessagesAsync().ToBlockingEnumerable().ToArray();
        for(int i = 0; i < amount; i++)
        {
            msg = channel.GetMessagesBeforeAsync(msg[msg.Length].Id).ToBlockingEnumerable().ToArray();
            list.AddRange(msg);
        }
        list.AddRange(msg);
        return list;
    }

    public async Task<List<DiscordMessage>> GetPreviousMessagesStartsWith(DiscordChannel channel, int amount, string text)
    {
        var msgs = await GetPreviousMessagesAsync(channel, amount);
        return msgs.Where(x => x.Content.StartsWith(text)).ToList();
    }

    public async Task<List<DiscordMessage>> GetPreviousMessagesAsync(DiscordChannel channel, int amount)
    {
        List<DiscordMessage> list = new();


        DiscordMessage lastMessage = default;
        await foreach(var msg in channel.GetMessagesAsync())
        {
            list.Add(msg);
            lastMessage = msg;
        }

        for(int i = 0; i < amount; i++)
        {
            var next = channel.GetMessagesBeforeAsync(lastMessage!.Id);

            await foreach(var msg in next)
            {
                list.Add(msg);
                lastMessage = msg;
            }
        }


        return list;
    }
}
