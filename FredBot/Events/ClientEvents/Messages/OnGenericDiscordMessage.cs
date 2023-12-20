using DSharpPlus;
using DSharpPlus.EventArgs;
using MediatR;

namespace FredBot.Events.ClientEvents.Messages;

public class OnGenericDiscordMessage(DiscordClient sender, DiscordEventArgs args) : INotification
{
    public DiscordClient Sender { get; set; } = sender;
    public DiscordEventArgs Args { get; set; } = args;
}
