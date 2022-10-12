using StardewValley;
using System;
using System.Collections.Generic;

namespace StardewRoguelike.Patches
{
    internal class AddStockPatch : Patch
    {
		protected override PatchDescriptor GetPatchDescriptor() => new(typeof(Utility), "AddStock");

		public static bool Prefix(Dictionary<ISalable, int[]> stock, Item obj, int buyPrice = -1, int limitedQuantity = -1)
		{
			int price = buyPrice;
			if (buyPrice == -1)
			{
				price = obj.salePrice();
			}
			int stack = int.MaxValue;
			if (obj is SObject && (obj as SObject).IsRecipe)
			{
				stack = 1;
			}
			else if (limitedQuantity != -1)
			{
				stack = limitedQuantity;
			}
			stock.Add(obj, new int[2] { price, stack });

			return false;
		}
	}
}
