using Microsoft.Xna.Framework;
using StardewValley;

namespace StardewRoguelike.Extensions
{
    public static class Vector2ToUIScale
    {
        public static Vector2 ToUIScale(this Vector2 v2)
        {
            return new Vector2(
                (int)Utility.ModifyCoordinateForUIScale(v2.X),
                (int)Utility.ModifyCoordinateForUIScale(v2.Y)
            );
        }
    }
}