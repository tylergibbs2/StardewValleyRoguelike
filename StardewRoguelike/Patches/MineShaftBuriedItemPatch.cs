using HarmonyLib;
using StardewValley.Locations;

namespace StardewRoguelike.Patches
{
    [HarmonyPatch(typeof(MineShaft), "checkForBuriedItem")]
    internal class MineShaftBuriedItemPatch
    {
        public static bool Prefix(ref string __result)
        {
            __result = "";
            return false;
        }
    }
}
