using Microsoft.Xna.Framework;
using StardewValley.Locations;
using StardewRoguelike.VirtualProperties;

namespace StardewRoguelike.Patches
{
    internal class MineShaftEntrancePosition : Patch
    {
        protected override PatchDescriptor GetPatchDescriptor() => new(typeof(MineShaft), "mineEntrancePosition");

        public static bool Prefix(MineShaft __instance, ref Vector2 __result)
        {
            Vector2 customDest = __instance.get_MineShaftCustomDestination().Value;
            if (customDest != Vector2.Zero)
            {
                __result = customDest;
                return false;
            }

            return true;
        }
    }
}
