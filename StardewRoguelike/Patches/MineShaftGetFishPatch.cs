using HarmonyLib;
using StardewValley;
using StardewValley.Locations;

namespace StardewRoguelike.Patches
{
    [HarmonyPatch(typeof(MineShaft), "getFish")]
    internal class MineShaftGetFishPatch
    {
        public static bool Prefix(MineShaft __instance, ref Object __result, Farmer who)
        {
            __result = Roguelike.GetFish(__instance, who);
            return false;
        }
    }
}
