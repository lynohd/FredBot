using DSharpPlus;
using DSharpPlus.EventArgs;
using MediatR;

namespace FredBot.Events.ClientEvents.Messages;

public class OnDiscordMessageCreated(DiscordClient sender, MessageCreateEventArgs args) : INotification
{
    public DiscordClient Sender { get; } = sender;
    public MessageCreateEventArgs Args { get; } = args;
}
