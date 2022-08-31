using HarmonyLib;
using StardewValley;


namespace StardewRoguelike.Patches
{
    [HarmonyPatch(typeof(GameLocation), "makeHoeDirt")]
    internal class DisableHoeDirt
    {
        public static bool Prefix()
        {
            return false;
        }
    }
}
