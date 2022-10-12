using StardewValley.Monsters;
using StardewValley;

namespace StardewRoguelike.Enchantments
{
    public class SlimeKillerEnchantment : BaseWeaponEnchantment
    {
        protected override void _OnDealDamage(Monster monster, GameLocation location, Farmer who, ref int amount)
        {
            if (monster is GreenSlime || monster is BigSlime)
                amount = (int)(amount * 2f);
        }

        public override string GetName()
        {
            return I18n.Enchantments_SlimeKiller();
        }
    }
}
