using HarmonyLib;
using StardewValley;
using StardewValley.Tools;
using System;

namespace StardewRoguelike.Patches
{
    [HarmonyPatch(typeof(MeleeWeapon), "doAnimateSpecialMove")]
    internal class MeleeWeaponSpecialPatch
    {
        public static void Postfix(MeleeWeapon __instance)
        {
            //Farmer lastUser = __instance.getLastFarmerToUse();
            //if (lastUser != Game1.player || !Curse.HasCurse(CurseType.GestureOfTheDrowned))
            //    return;

            //switch (__instance.type.Value)
            //{
            //    case 1:
            //        MeleeWeapon.daggerCooldown /= 2;
            //        break;
            //    case 2:
            //        MeleeWeapon.clubCooldown /= 2;
            //        break;
            //    case 3:
            //        MeleeWeapon.defenseCooldown = Math.Max(MeleeWeapon.defenseCooldown / 2, 600);
            //        break;
            //}
        }
    }
}
