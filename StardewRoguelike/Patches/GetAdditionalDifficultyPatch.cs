using HarmonyLib;
using StardewValley.Locations;

namespace StardewRoguelike.Patches
{
    [HarmonyPatch(typeof(MineShaft), "GetAdditionalDifficulty")]
    internal class GetAdditionalDifficultyPatch
    {
        public static bool Prefix(MineShaft __instance, ref int __result)
        {
            int level = Roguelike.GetLevelFromMineshaft(__instance);
            __result = level % 48 >= Roguelike.DangerousThreshold ? 1 : 0;
            return false;
        }
    }
}
