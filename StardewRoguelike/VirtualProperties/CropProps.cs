using StardewValley;
using System.Runtime.CompilerServices;

namespace StardewRoguelike.VirtualProperties
{
    public static class CropProps
    {
        internal class Holder { public int Value = 0; }

        internal static ConditionalWeakTable<Crop, Holder> values = new();

        public static void set_CropMerchantsPlantedAgo(this Crop crop, int value)
        {
            var holder = values.GetOrCreateValue(crop);
            holder.Value = value;
        }

        public static int get_CropMerchantsPlantedAgo(this Crop crop)
        {
            return values.GetOrCreateValue(crop).Value;
        }
    }
}
