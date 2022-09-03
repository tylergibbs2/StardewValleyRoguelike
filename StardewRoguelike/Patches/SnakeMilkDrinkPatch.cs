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
                int toAdd = 25;
                if (Curse.HasCurse(CurseType.GlassCannon))
                    toAdd = 12;

                Game1.player.maxHealth = Math.Min(Game1.player.maxHealth + toAdd, Roguelike.MaxHP);
                Game1.player.health = Math.Min(Game1.player.health + toAdd, Game1.player.maxHealth);
            }

            return true;
        }
    }
}
