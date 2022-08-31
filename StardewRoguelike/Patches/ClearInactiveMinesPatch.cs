using StardewValley.Locations;

namespace StardewRoguelike.Patches
{
    internal class ClearInactiveMinesPatch : Patch
    {
        protected override PatchDescriptor GetPatchDescriptor() => new(typeof(MineShaft), "clearInactiveMines");

        public static bool Prefix()
        {
            return false;
        }
    }
}
