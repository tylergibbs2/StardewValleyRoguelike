using HarmonyLib;
using StardewValley;

namespace StardewRoguelike.Patches
{
    [HarmonyPatch(typeof(Object), "needsToBeDonated")]
    internal class ObjectNeedsDonatedPatch
    {
        public static bool Prefix(ref bool __result)
        {
            __result = false;
            return false;
        }
    }
}
