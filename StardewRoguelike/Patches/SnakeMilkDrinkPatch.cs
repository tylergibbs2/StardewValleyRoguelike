using StardewValley;
using System;

namespace StardewRoguelike.Patches
{
    internal class SnakeMilkDrinkPatch : Patch
    {
        protected override PatchDescriptor GetPatchDescriptor() => new(typeof(Farmer), "reduceActiveItemByOne");

        public static bool Prefix(Farmer __instance)
        {
            if (__instance.CurrentItem is not null && __instance.CurrentItem.ParentSheetIndex == 803 && Game1.player.maxHealth < Roguelike.MaxHP)
            {
                Game1.player.maxHealth = Math.Min(Game1.player.maxHealth + 25, Roguelike.MaxHP);
                Game1.player.health += Math.Min(Game1.player.health + 25, Roguelike.MaxHP);
            }

            return true;
        }
    }
}
