using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Converters;
using DSharpPlus.Entities;
using Serilog;

namespace FredBot.Commands.Converters;

public class CustomArgumentConverter : IArgumentConverter<bool>
{
    private readonly Serilog.ILogger _logger = Log.Logger;
    public Task<Optional<bool>> ConvertAsync(string value, CommandContext ctx)
    {
        _logger.Information("{Command} with Arguments[{Arguments}] is being handled by {className} ",ctx.Command, ctx.RawArgumentString, nameof(CustomArgumentConverter));
        if(bool.TryParse(value, out var @bool))
        {
            return Task.FromResult(Optional.FromValue(@bool));
        }

        switch(value.ToLower())
        {
            
            case "true" or "y" or "yes" or "1":
            _logger.Information("ArgumentConverter::{className} ran with result {result}", nameof(CustomArgumentConverter), true);
            return Task.FromResult(Optional.FromValue(true));

            case "false" or "n" or "no" or "0":
            _logger.Information("ArgumentConverter::{className} ran with result {result}", nameof(CustomArgumentConverter), false);
            return Task.FromResult(Optional.FromValue(false));

            default: return Task.FromResult(Optional.FromNoValue<bool>());
        }
    }
}
