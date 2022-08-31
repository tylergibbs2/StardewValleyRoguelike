using HarmonyLib;
using StardewValley;
using StardewValley.Menus;

namespace StardewRoguelike.Patches
{
    [HarmonyPatch(typeof(BuffsDisplay), "updatePosition")]
    internal class BuffDisplayUpdatePosition
    {
        public static void Postfix(BuffsDisplay __instance)
        {
            __instance.width = Game1.uiViewport.Width;
            __instance.xPositionOnScreen += 64;
            __instance.syncIcons();
        }
    }
}
