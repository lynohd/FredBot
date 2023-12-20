using DSharpPlus;
using DSharpPlus.EventArgs;
using MediatR;

namespace FredBot.Events.ClientEvents.Messages;
public class OnDiscordGuildAvailable(DiscordClient sender, GuildCreateEventArgs args) : INotification
{
    public DiscordClient Sender { get; } = sender;
    public GuildCreateEventArgs Args { get; } = args;
}