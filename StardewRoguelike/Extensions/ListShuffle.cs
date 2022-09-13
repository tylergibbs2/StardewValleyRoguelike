using StardewValley;
using System;
using System.Collections.Generic;

namespace StardewRoguelike.Extensions
{
    internal static class ListShuffle
    {
        public static void Shuffle<T>(this IList<T> list, Random random = null)
        {
            random ??= Game1.random;

            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = random.Next(n + 1);
                (list[n], list[k]) = (list[k], list[n]);
            }
        }
    }
}
