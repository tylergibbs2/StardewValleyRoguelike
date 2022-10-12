using HarmonyLib;

namespace StardewRoguelike.Patches
{
    [HarmonyPatch(typeof(SObject), "performToolAction")]
    internal class ObjectPerformActionPatch
    {
        public static bool Prefix(SObject __instance, ref bool __result)
        {
            // Farm Computer, Deconstructor, Garden Pot, Sprinkler
            if (__instance.ParentSheetIndex == 239 || __instance.ParentSheetIndex == 265 || __instance.ParentSheetIndex == 62 || __instance.ParentSheetIndex == 599)
            {
                __result = false;
                return false;
            }

            return true;
        }
    }
}
