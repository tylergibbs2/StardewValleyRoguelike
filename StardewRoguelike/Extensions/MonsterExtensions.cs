using StardewValley.Monsters;
using System;

namespace StardewRoguelike.Extensions
{
    internal static class MonsterExtensions
    {
        public static float AdjustRangeForHealth(this Monster monster, float min, float max)
        {
            float range = max - min;
            float hpPercent = (float)monster.Health / monster.MaxHealth;
            float toSubtract = hpPercent * range;
            return Math.Min(Math.Max(min, max - toSubtract), max);
        }

        public static int AdjustRangeForHealth(this Monster monster, int min, int max)
        {
            int range = max - min;
            float hpPercent = (float)monster.Health / monster.MaxHealth;
            int toSubtract = (int)Math.Round(hpPercent * range, 0);
            return Math.Min(Math.Max(min, max - toSubtract), max);
        }
    }
}
