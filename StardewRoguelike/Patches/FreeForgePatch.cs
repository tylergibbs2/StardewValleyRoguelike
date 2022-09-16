﻿using HarmonyLib;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewRoguelike.UI;
using StardewValley;
using StardewValley.Menus;
using System;
using System.Collections.Generic;

namespace StardewRoguelike.Patches
{
    internal class FreeForgePatch : Patch
    {
        protected override PatchDescriptor GetPatchDescriptor() => new(new List<string>() { "SpaceCore.Interface.NewForgeMenu", "StardewValley.Menus.ForgeMenu" }, "GetForgeCost");

        public static bool Prefix(ref int __result)
        {
            __result = 0;
            return false;
        }
    }

    internal class FreeForgePatch2 : Patch
    {
        protected override PatchDescriptor GetPatchDescriptor() => new(new List<string>() { "SpaceCore.Interface.NewForgeMenu", "StardewValley.Menus.ForgeMenu" }, "GetForgeCostAtLevel");

        public static bool Prefix(ref int __result)
        {
            __result = 0;
            return false;
        }
    }

    internal class LimitedForgePatch : Patch
    {
        protected override PatchDescriptor GetPatchDescriptor() => new(new List<string>() { "SpaceCore.Interface.NewForgeMenu", "StardewValley.Menus.ForgeMenu" }, "IsValidCraft");

        public static bool Prefix(ForgeMenu __instance, ref bool __result)
        {
            if (__instance is LimitedForgeMenu limitedForgeMenu && limitedForgeMenu.UsesLeft == 0)
            {
                __result = false;
                return false;
            }

            return true;
        }
    }

    internal class LimitedForgePatch2 : Patch
    {
        protected override PatchDescriptor GetPatchDescriptor() => new(new List<string>() { "SpaceCore.Interface.NewForgeMenu", "StardewValley.Menus.ForgeMenu" }, "CraftItem");

        public static void Postfix(ForgeMenu __instance, bool forReal = false)
        {
            if (__instance is LimitedForgeMenu limitedForgeMenu && forReal)
                limitedForgeMenu.UsesLeft--;
        }
    }

    internal class LimitedForgePatch3 : Patch
    {
        protected override PatchDescriptor GetPatchDescriptor() => new(new List<string>() { "SpaceCore.Interface.NewForgeMenu", "StardewValley.Menus.ForgeMenu" }, "draw");

        public static void Postfix(ForgeMenu __instance, SpriteBatch b)
        {
            if (__instance is not LimitedForgeMenu limitedForgeMenu)
                return;

            Color usesColor = limitedForgeMenu.UsesLeft switch
            {
                3 => Color.Green,
                2 => Color.Yellow,
                1 => Color.Red,
                _ => Color.Black
            };

            Utility.drawBoldText(
                b,
                "Uses Left",
                Game1.smallFont,
                new Vector2(limitedForgeMenu.xPositionOnScreen + 292, limitedForgeMenu.yPositionOnScreen + 310),
                Game1.textColor
            );

            Utility.drawBoldText(
                b,
                limitedForgeMenu.UsesLeft.ToString(),
                Game1.smallFont,
                new Vector2(limitedForgeMenu.xPositionOnScreen + 340, limitedForgeMenu.yPositionOnScreen + 338),
                usesColor
            );

            __instance.drawMouse(b);
        }
    }

    [HarmonyPatch(typeof(Utility), "CollectOrDrop", new Type[] { typeof(Item), typeof(int) })]
    internal class UtilityCollectOrDrop
    {
        public static bool Prefix(ref bool __result, Item item, int direction)
        {
            if (item is not null && item.ParentSheetIndex == 848)
            {
                __result = false;
                return false;
            }

            return true;
        }
    }
}
