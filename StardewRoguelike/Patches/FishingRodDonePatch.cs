using HarmonyLib;
using StardewRoguelike.VirtualProperties;
using StardewValley;
using StardewValley.Tools;
using System;
using System.Reflection;

namespace StardewRoguelike.Patches
{
    [HarmonyPatch(typeof(FishingRod), "doDoneFishing")]
    internal class FishingRodDonePatch
    {
        public static void Postfix(FishingRod __instance)
        {
            Farmer lastUser = (Farmer)__instance.GetType().BaseType.GetField("lastUser", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(__instance);
            if (lastUser is null || !lastUser.IsLocalPlayer)
                return;

            __instance.increment_FishingRodUses();

            if (__instance.get_FishingRodUses() >= 3)
            {
                Game1.player.removeItemFromInventory(__instance);
                Game1.playSound("breakingGlass");
            }
        }
    }
}
