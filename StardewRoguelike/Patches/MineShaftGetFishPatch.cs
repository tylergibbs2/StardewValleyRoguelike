using HarmonyLib;
using StardewValley;
using StardewValley.Locations;

namespace StardewRoguelike.Patches
{
    [HarmonyPatch(typeof(MineShaft), "getFish")]
    internal class MineShaftGetFishPatch
    {
        public static bool Prefix(MineShaft __instance, ref Object __result)
        {
            __result = Roguelike.GetFish(__instance);
            return false;
        }
    }
}
