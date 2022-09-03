using HarmonyLib;
using StardewValley;
using System;

namespace StardewRoguelike.Patches
{
    [HarmonyPatch(typeof(Buff), MethodType.Constructor, new Type[] { typeof(int) })]
    internal class ShorterNauseousPatch
    {
        public static void Postfix(Buff __instance, int which)
        {
            if (which == 25 && __instance.millisecondsDuration >= 10 * 1000)
                __instance.millisecondsDuration = 10 * 1000;
        }
    }
}
