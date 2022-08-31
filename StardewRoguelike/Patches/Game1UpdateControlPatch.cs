using HarmonyLib;
using StardewRoguelike.UI;
using StardewValley;
using StardewValley.Menus;

namespace StardewRoguelike.Patches
{
    [HarmonyPatch(typeof(Game1), "UpdateControlInput")]
    internal class Game1UpdateControlPatch
    {
        public static void Postfix()
        {
            if (Game1.activeClickableMenu is QuestLog)
                Game1.activeClickableMenu = new PerkDisplayMenu();
        }
    }
}
