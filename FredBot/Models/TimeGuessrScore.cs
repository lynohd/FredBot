using System.ComponentModel.DataAnnotations;

namespace FredBot.Models;

public record TimeGuessrScore(int Day, int Score, ulong DiscordUser, ulong MessageId);