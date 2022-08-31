using StardewValley;

namespace StardewRoguelike.Patches
{
    class DisableAchievements : Patch
    {
        protected override PatchDescriptor GetPatchDescriptor() => new(typeof(Game1), "getAchievement");

        public static bool Prefix()
        {
            return false;
        }
    }
}
