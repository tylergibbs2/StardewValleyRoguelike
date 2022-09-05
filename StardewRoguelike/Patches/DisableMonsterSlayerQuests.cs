using HarmonyLib;
using StardewValley.Locations;

namespace StardewRoguelike.Patches
{
    [HarmonyPatch(typeof(AdventureGuild), "willThisKillCompleteAMonsterSlayerQuest")]
    internal class DisableMonsterSlayerQuests
    {
        public static bool Prefix(ref bool __result)
        {
            __result = false;
            return false;
        }
    }
}
