using HarmonyLib;

namespace StardewRoguelike.Patches
{
    [HarmonyPatch(typeof(StardewValley.Object), "performToolAction")]
    internal class ObjectPerformActionPatch
    {
        public static bool Prefix(StardewValley.Object __instance, ref bool __result)
        {
            // Farm Computer
            if (__instance.ParentSheetIndex == 239)
            {
                __result = false;
                return false;
            }

            return true;
        }
    }
}
