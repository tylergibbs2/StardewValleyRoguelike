using Microsoft.Xna.Framework;
using StardewValley;

namespace StardewRoguelike.Extensions
{
    public static class RectangleToUIScale
    {
        public static Rectangle ToUIScale(this Rectangle rect)
        {
            return new Rectangle(
                (int)Utility.ModifyCoordinateForUIScale(rect.X),
                (int)Utility.ModifyCoordinateForUIScale(rect.Y),
                (int)Utility.ModifyCoordinateForUIScale(rect.Width),
                (int)Utility.ModifyCoordinateForUIScale(rect.Height)
            );
        }
    }
}