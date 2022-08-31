using StardewValley.Monsters;

namespace StardewRoguelike.Patches
{
    internal class NoArmoredBugPatch : Patch
    {
        protected override PatchDescriptor GetPatchDescriptor() => new(typeof(Bug), "takeDamage");

        public static bool Prefix(Bug __instance)
        {
            __instance.isArmoredBug.Value = false;
            return true;
        }
    }
}
