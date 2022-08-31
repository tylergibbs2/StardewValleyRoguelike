using HarmonyLib;
using StardewValley;
using StardewRoguelike.VirtualProperties;

namespace StardewRoguelike.Patches
{
    [HarmonyPatch(typeof(GameLocation), "updateEvenIfFarmerIsntHere")]
    internal class GameLocationUpdatePatch
    {
        public static void Postfix(GameLocation __instance)
        {
            __instance.get_DebuffPlayerEvent().Poll();
        }
    }
}
