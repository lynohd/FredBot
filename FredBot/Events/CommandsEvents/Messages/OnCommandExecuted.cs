using DSharpPlus.CommandsNext;
using MediatR;

namespace FredBot.Events.CommandsEvents.Messages;

public class OnCommandExecuted(CommandsNextExtension extension, CommandExecutionEventArgs args) : INotification
{
    public CommandsNextExtension Extension { get; } = extension;
    public CommandExecutionEventArgs Args { get; } = args;
}
