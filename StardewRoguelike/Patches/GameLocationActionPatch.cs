﻿using StardewModdingAPI;
using StardewValley;
using StardewValley.Locations;
using xTile.Dimensions;
using xTile.Tiles;

namespace StardewRoguelike.Patches
{
	internal class GameLocationActionPatch : Patch
	{
		protected override PatchDescriptor GetPatchDescriptor() => new(typeof(GameLocation), "checkAction");

		public static bool Prefix(GameLocation __instance, ref bool __result, Location tileLocation, Rectangle viewport, Farmer who)
		{
			if (__instance is MineShaft mine)
			{
				bool result = ChallengeFloor.CheckAction(mine, tileLocation, viewport, who);
                if (result)
                {
                    __result = true;
                    return false;
                }
			}
			else if (__instance is Mine)
			{
                Tile tile = __instance.map.GetLayer("Buildings").PickTile(new Location(tileLocation.X * 64, tileLocation.Y * 64), viewport.Size);
                if (tile is not null && who.IsLocalPlayer)
                {
                    if (Context.IsMainPlayer)
                        return true;
                    else if (tile.TileIndex == 173 && Roguelike.GetHighestMineShaftLevel() == 0)
                    {
                        Game1.drawObjectDialogue("The host must enter first.");
                        __result = false;
                        return false;
                    }
                }
            }

			return true;
		}
	}
}
