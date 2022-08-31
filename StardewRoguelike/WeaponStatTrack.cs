using StardewValley;
using StardewValley.Monsters;

namespace StardewRoguelike
{
    public class WeaponStatTrack : BaseEnchantment
    {
        protected override void _OnDealDamage(Monster monster, GameLocation location, Farmer who, ref int amount)
        {
            ModEntry.Stats.DamageDealt += amount / 2;
        }

        protected override void _OnMonsterSlay(Monster monster, GameLocation location, Farmer who)
        {
            ModEntry.Stats.MonstersKilled++;
        }
    }
}
