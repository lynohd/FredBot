using DSharpPlus;
using DSharpPlus.EventArgs;
using MediatR;

namespace FredBot.Events.ClientEvents.Messages;
public class OnComponentInteract(DiscordClient sender, ComponentInteractionCreateEventArgs args) : INotification
{
    public DiscordClient Sender { get; } = sender;
    public ComponentInteractionCreateEventArgs Args { get; } = args;
}