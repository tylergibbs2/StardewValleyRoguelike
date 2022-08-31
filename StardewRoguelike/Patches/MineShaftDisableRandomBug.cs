using StardewValley.Locations;

namespace StardewRoguelike.Patches
{
    internal class MineShaftDisableRandomBug : Patch
    {
        protected override PatchDescriptor GetPatchDescriptor() => new(typeof(MineShaft), "spawnFlyingMonsterOffScreen");

        public static bool Prefix()
        {
            return false;
        }
    }
}
