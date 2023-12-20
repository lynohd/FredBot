using DSharpPlus.CommandsNext;
using MediatR;

namespace FredBot.Events.CommandsEvents.Messages;

public class OnCommandError(CommandsNextExtension extension, CommandErrorEventArgs args) : INotification
{
    public CommandsNextExtension Extension { get; } = extension;
    public CommandErrorEventArgs Args { get; } = args;
}
