using StardewValley;
using StardewValley.Locations;
using StardewValley.Menus;
using System.Collections.Generic;
using xTile.Dimensions;
using xTile.Tiles;

namespace StardewRoguelike.Patches
{
    internal class MineShaftActionPatch : Patch
    {
        protected override PatchDescriptor GetPatchDescriptor() => new(typeof(MineShaft), "checkAction");

		private static readonly List<int> TilesToIgnore = new() { 112, 115, 174, 194, 315, 316, 317 };

		public static bool Prefix(MineShaft __instance, ref bool __result, Location tileLocation, Rectangle viewport, Farmer who)
        {
			Tile tile = __instance.map.GetLayer("Buildings").PickTile(new Location(tileLocation.X * 64, tileLocation.Y * 64), viewport.Size);
			if (tile is not null && who.IsLocalPlayer)
			{
				if (TilesToIgnore.Contains(tile.TileIndex))
				{
					__result = false;
					return false;
				}
				else if (ForgeFloor.IsForgeFloor(__instance) && ((uint)(tile.TileIndex - 123) <= 1u || (uint)(tile.TileIndex - 133) <= 1u || (uint)(tile.TileIndex - 156) <= 1u))
				{
					Game1.activeClickableMenu = new ForgeMenu();
					__result = true;
					return false;
				}
			}

			return true;
		}
    }
}
