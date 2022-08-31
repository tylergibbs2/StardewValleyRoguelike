using HarmonyLib;
using StardewValley.Menus;

namespace StardewRoguelike.Patches
{
    [HarmonyPatch(typeof(LoadGameMenu), "hasDeleteButtons")]
    internal class DisableSaveDeleteButton
    {
        public static bool Prefix(ref bool __result)
        {
            __result = false;
            return false;
        }
    }
}
