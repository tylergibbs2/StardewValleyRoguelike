using HarmonyLib;
using static StardewValley.Minigames.AbigailGame;

namespace StardewRoguelike.Patches
{
    [HarmonyPatch(typeof(CowboyMonster), "takeDamage")]
    internal class JOTPKTakeDamagePatch
    {
        public static void Postfix(bool __result)
        {
            if (__result)
                Minigames.PrairieKingKills++;
        }
    }
}
