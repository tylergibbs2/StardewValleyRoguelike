using StardewValley;
using xTile.Dimensions;

namespace StardewRoguelike.Patches
{
    class DrawExtraBackLayer : Patch
    {
        protected override PatchDescriptor GetPatchDescriptor() => new(typeof(GameLocation), "drawBackground");

        public static bool Prefix(GameLocation __instance)
        {
            __instance.Map.GetLayer("BackBackBack")?.Draw(Game1.mapDisplayDevice, Game1.viewport, Location.Origin, wrapAround: false, 4);
            __instance.Map.GetLayer("BackBack")?.Draw(Game1.mapDisplayDevice, Game1.viewport, Location.Origin, wrapAround: false, 4);
            return true;
        }
    }
}