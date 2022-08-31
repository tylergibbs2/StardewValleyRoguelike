using StardewValley.Locations;

namespace StardewRoguelike.Patches
{
	internal class NoChestsPatch : Patch
	{
		protected override PatchDescriptor GetPatchDescriptor() => new(typeof(MineShaft), "addLevelChests");

		public static bool Prefix()
		{
			return false;
		}
	}
}
