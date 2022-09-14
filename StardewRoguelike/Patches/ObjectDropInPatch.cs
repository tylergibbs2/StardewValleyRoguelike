using HarmonyLib;
using StardewValley;
using StardewValley.Objects;
using StardewValley.Tools;
using System;
using System.Collections.Generic;

namespace StardewRoguelike.Patches
{
    [HarmonyPatch(typeof(StardewValley.Object), "performObjectDropInAction")]
    internal class ObjectDropInPatch
    {
        public static bool Prefix(StardewValley.Object __instance, ref bool __result, Item dropInItem, bool probe, Farmer who)
        {
            if (__instance.Name != "Deconstructor")
                return true;

            if (__instance.isTemporarilyInvisible)
                return true;

            if (__instance.heldObject.Value is not null)
            {
                Game1.showRedMessage("Deconstructor is already in use.");
                return false;
            }

            if (dropInItem is not null)
            {
                StardewValley.Object deconstructor_output = __instance.GetDeconstructorOutput(dropInItem);
                if (deconstructor_output != null)
                {
                    __instance.heldObject.Value = new StardewValley.Object(dropInItem.ParentSheetIndex, 1);
                    if (!probe)
                    {
                        __instance.heldObject.Value = deconstructor_output;
                        __instance.MinutesUntilReady = 5;
                        Game1.playSound("furnace");
                        __result = true;
                        return false;
                    }
                    __result = true;
                    return false;
                }
                if (!probe)
                {
                    if (StardewValley.Object.autoLoadChest == null)
                        Game1.showRedMessage(Game1.content.LoadString("Strings\\StringsFromCSFiles:Deconstructor_fail"));

                    return false;
                }
            }

            return false;
        }
    }

    [HarmonyPatch(typeof(StardewValley.Object), "GetDeconstructorOutput")]
    internal class DeconstructorOutputPatch
    {
        public static readonly List<int> InvalidItemIndices = new()
        {
            384,  // Gold Ore
            39,   // Dark Sign
            322   // Wood Fence
        };

        public static bool Prefix(ref StardewValley.Object __result, Item item)
        {
            if (InvalidItemIndices.Contains(item.ParentSheetIndex) || item is Pickaxe)
            {
                __result = null;
                return false;
            }

            __result = new(384, 3);

            return false;
        }
    }
}
