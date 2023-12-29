using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using DSharpPlus;
using DSharpPlus.Entities;
using FredBot.Models;

namespace FredBot.Services;
public partial class TimeGuessrService(ILogger<TimeGuessrService> logger)
{

    [GeneratedRegex(@"^TimeGuessr\s+#(?<Day>\d+)\s+(?<Score>\b(?:[0-9]\d{0,4})\b)/(?<Max>\b(?:50000)\b)\b")]
    internal static partial Regex IsValidTimeGuessr();



    public async Task HandleMessagesAsync(DiscordClient sender, DiscordMessage[] messages)
    {
        foreach (var message in messages)
        {
            await HandleMessage(sender, message);
        }
    }
    public async Task HandleMessage(DiscordClient sender, DiscordMessage discordMessage)
    {

        var startTime = Stopwatch.GetTimestamp();

        var match = IsValidTimeGuessr().Match(_SanitizeMessage(discordMessage.Content));


        if(int.TryParse(match.Groups["Day"].Value, out var day) &&
            int.TryParse(match.Groups["Score"].Value, out var score))
        {
            if(score >= 50000)
            {
                score = 50000;

                await discordMessage.CreateReactionAsync(DiscordEmoji.FromName(sender, ":exploding_head:"));
            }

            if(score <= 0)
                score = 0;

            var model = new TimeGuessrScore(day, score, discordMessage.Author.Id, discordMessage.Id);

            logger.LogInformation(model.ToString());

            await discordMessage.CreateReactionAsync(DiscordEmoji.FromName(sender, ":thumbsup:"));
        }


        var elapsed = Stopwatch.GetElapsedTime(startTime);
        string _SanitizeMessage(string text) => text
            .Replace(",", "")
            .Replace("https://timeguessr.com/", "");
    }
}
