using StardewValley.Menus;

namespace StardewRoguelike.UI
{
    public class LimitedForgeMenu : ForgeMenu
    {
        public int UsesLeft { get; set; } = 3;

        public LimitedForgeMenu() : base() { }
    }
}
