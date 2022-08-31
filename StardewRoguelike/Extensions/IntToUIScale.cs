using StardewValley;

namespace StardewRoguelike.Extensions
{
    public static class IntToUIScale
    {
        public static int ToUIScale(this int i)
        {
            return (int)Utility.ModifyCoordinateForUIScale(i);
        }
    }
}