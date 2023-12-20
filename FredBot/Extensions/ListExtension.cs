using System.Security.Cryptography;

namespace FredBot.Extensions;

public static class ListExtension
{

    private static Random Rng = new();
    public static void Shuffle<T>(this IList<T> list)
    {
        int n = list.Count;

        while(n > 1)
        {
            n--;
            int k = Rng.Next(n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }
}
