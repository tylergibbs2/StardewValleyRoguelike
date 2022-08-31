using Microsoft.Xna.Framework;
using StardewValley.Locations;

namespace StardewRoguelike.Patches
{
    internal class MineShaftUpdateWhenCurrentLocation : Patch
    {
        protected override PatchDescriptor GetPatchDescriptor() => new(typeof(MineShaft), "UpdateWhenCurrentLocation");

        public static void Postfix(MineShaft __instance, GameTime time)
        {
            ChallengeFloor.DoUpdate(__instance, time);
        }
    }
}
