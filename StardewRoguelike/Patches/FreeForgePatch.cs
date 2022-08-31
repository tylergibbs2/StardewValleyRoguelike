using StardewValley.Menus;

namespace StardewRoguelike.Patches
{
    internal class FreeForgePatch : Patch
    {
        protected override PatchDescriptor GetPatchDescriptor() => new(typeof(ForgeMenu), "GetForgeCost");

        public static bool Prefix(ref int __result)
        {
            __result = 0;
            return false;
        }
    }
}
