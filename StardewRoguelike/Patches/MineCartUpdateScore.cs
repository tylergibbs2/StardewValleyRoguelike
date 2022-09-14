using HarmonyLib;
using StardewValley.Minigames;

namespace StardewRoguelike.Patches
{
    [HarmonyPatch(typeof(MineCart), "restartLevel")]
    internal class MineCartUpdateScore
    {
        public static bool Prefix(MineCart __instance)
        {
            int score = (int)__instance.GetType().GetField("score", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic).GetValue(__instance);
            Minigames.JunimoKartScore += score;

            return true;
        }
    }
}
