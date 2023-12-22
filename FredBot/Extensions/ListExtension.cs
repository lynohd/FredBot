using System;
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

    public static async Task<bool> AnyAsync<T>(this IEnumerable<T> source, Func<T, Task<bool>> predicate)
    {

        if(source == null)
        {
            throw new ArgumentNullException(nameof(source));
        }

        if(predicate == null)
        {
            throw new ArgumentNullException(nameof(predicate));
        }

        foreach(var item in source)
        {
            if(await predicate(item).ConfigureAwait(false))
            {
                return true;
            }
        }

        return false;
    }
    public static async Task<bool> AllAsync<T>(this IEnumerable<T> source, Func<T, Task<bool>> predicate)
    {
        if(source == null)
        {
            throw new ArgumentNullException(nameof(source));
        }

        if(predicate == null)
        {
            throw new ArgumentNullException(nameof(predicate));
        }

        foreach(var item in source)
        {
            if(!await predicate(item).ConfigureAwait(false))
            {
                return false;
            }
        }

        return true;
    }
}
