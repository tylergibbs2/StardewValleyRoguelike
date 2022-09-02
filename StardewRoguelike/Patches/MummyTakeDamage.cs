using HarmonyLib;
using StardewValley.Monsters;

namespace StardewRoguelike.Patches
{
    [HarmonyPatch(typeof(Mummy), "takeDamage")]
    internal class MummyTakeDamage
    {
        public static bool Prefix(ref bool isBomb)
        {
            isBomb = true;
            return true;
        }
    }
}
