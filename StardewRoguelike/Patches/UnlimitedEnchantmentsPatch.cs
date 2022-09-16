using HarmonyLib;
using StardewValley.Tools;

namespace StardewRoguelike.Patches
{
    [HarmonyPatch(typeof(MeleeWeapon), "GetMaxForges")]
    internal class UnlimitedEnchantmentsPatch
    {
        public static void Postfix(ref int __result)
        {
            __result = 999;
        }
    }
}
