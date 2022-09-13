using Microsoft.Xna.Framework;
using StardewValley;
using StardewValley.Locations;
using StardewValley.Objects;
using StardewValley.Tools;

namespace StardewRoguelike.Patches
{
    internal class BarrelContentsPatch : Patch
    {
        protected override PatchDescriptor GetPatchDescriptor() => new(typeof(BreakableContainer), "releaseContents");

        public static bool Prefix(BreakableContainer __instance, GameLocation location)
        {
			int x = (int)__instance.TileLocation.X;
			int y = (int)__instance.TileLocation.Y;
			if (location is MineShaft mine)
			{
				if (mine.isContainerPlatform(x, y))
					mine.updateMineLevelData(0, -1);

                var (itemId, quantity) = Roguelike.GetBarrelDrops(mine);
                if (itemId == 1000)
                {
                    Item toDrop = new FishingRod();
                    location.debris.Add(new Debris(toDrop, new Vector2(x * 64 + 32, y * 64 + 32), Game1.player.getStandingPosition()));
                }
                else if (itemId > 0 && quantity > 0)
                    Game1.createMultipleObjectDebris(itemId, x, y, quantity, location);
            }

			return false;
        }
    }
}
