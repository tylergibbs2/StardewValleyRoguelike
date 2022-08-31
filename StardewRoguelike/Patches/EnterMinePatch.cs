using StardewValley;

namespace StardewRoguelike.Patches
{
    internal class EnterMinePatch : Patch
    {
        protected override PatchDescriptor GetPatchDescriptor() => new(typeof(Game1), "enterMine");

        public static bool Prefix()
        {
            Roguelike.NextFloor();

            Game1.inMine = true;
            // Warp to mine level 1. We handle choosing the map
            // elsewhere.
            Game1.warpFarmer($"UndergroundMine1/{Roguelike.CurrentLevel}", 6, 6, 2);

            return false;
        }
    }
}
