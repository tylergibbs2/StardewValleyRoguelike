using StardewValley;
using StardewValley.Monsters;

namespace StardewRoguelike.Enchantments
{
    public class GlassCannonEnchantment : BaseEnchantment
    {
        protected override void _OnDealDamage(Monster monster, GameLocation location, Farmer who, ref int amount)
        {
            amount *= 2;
        }
    }
}
