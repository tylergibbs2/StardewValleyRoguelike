using Microsoft.Xna.Framework;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewRoguelike.Enchantments;
using StardewRoguelike.VirtualProperties;
using StardewValley;
using StardewValley.Locations;
using StardewValley.Monsters;
using StardewValley.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace StardewRoguelike
{
    /// <summary>
    /// All types of curses. The enum value is the unique
    /// buff ID to be used in-game.
    /// </summary>
    #pragma warning disable format
    internal enum CurseType
    {
        GlassCannon           = 89000,
        DamageOverTime        = 89001,
        PlayerKnockback       = 89002,
        BrittleCrown          = 89003,
        MoreEnemiesLessHealth = 89004,
        //GestureOfTheDrowned   = 89005,
        BombsAroundPlayer     = 89006,
        DoubleSpeedEveryone   = 89007,
        HealOverTime          = 89008,
        BuffsMorePotent       = 89009,
        BootsBetterImmunity   = 89010,
        BootsBetterDefense    = 89011,
        MoreCritsLessDamage   = 89012,
        CheaperMerchant       = 89013
    }
    #pragma warning restore format

    internal class Curse : Buff
    {
        /// <summary>
        /// Key is the type of curse. Value is a tuple of two strings.
        /// The first string is the display name, the second string is the description.
        /// </summary>
        private static readonly Dictionary<CurseType, (string, string)> CurseData = new()
        {
            { CurseType.GlassCannon, ("Glass Cannon", "You deal double damage but have half health.") },
            { CurseType.DamageOverTime, ("Damage Over Time", "You take damage over time and you take 1.5x more.") },
            { CurseType.PlayerKnockback, ("More Knockback", "Your knockback is doubled but you also take knockback.") },
            { CurseType.BrittleCrown, ("Brittle Crown", "You gain money on hit but also lose money when taking damage.") },
            { CurseType.MoreEnemiesLessHealth, ("More Enemies", "More enemies spawn but they have less health.") },
            //{ CurseType.GestureOfTheDrowned, ("Gesture of the Drowned", "Your weapon specials automatically activate but have a lower cooldown.") },
            { CurseType.BombsAroundPlayer, ("Player Bombing", "Cherry bombs occasionally spawn around you.") },
            { CurseType.DoubleSpeedEveryone, ("Double Speed", "You move twice as fast but so do enemies.") },
            { CurseType.HealOverTime, ("Heal Over Time", "You heal over time and heal 1.5x more.") },
            { CurseType.BuffsMorePotent, ("Potent Buffs", "Food buffs are more potent but have limited duration.") },
            { CurseType.BootsBetterImmunity, ("Boot Immunity", "All boots have +10 immunity and -10 defense.") },
            { CurseType.BootsBetterDefense, ("Boot Defense", "All boots have +10 defense and -10 immunity.") },
            { CurseType.MoreCritsLessDamage, ("More Crits", "Higher chance for a crit but crits do less damage.") },
            { CurseType.CheaperMerchant, ("Cheaper Merchant", "The merchant sells items for cheaper but has less items.") },
        };

        /// <summary>
        /// The description of the Curse instance.
        /// </summary>
        public readonly string Description;

        /// <summary>
        /// The buff icon to use when displaying curses.
        /// </summary>
        public static readonly int IconId = 18;

        /// <summary>
        /// The duration (in milliseconds) of buffs that are made
        /// potent via the <see cref="CurseType.BuffsMorePotent"/> curse.
        /// </summary>
        public static readonly int PotentBuffDuration = 1000 * 60 * 5;

        /// <summary>
        /// The amount of game ticks to wait before spawning bombs when
        /// the player has the <see cref="CurseType.BombsAroundPlayer"/> curse.
        /// </summary>
        private static int TicksToSpawnBombs = 60 * 10;

        /// <summary>
        /// The amount of damage over time to deal when the player has the
        /// <see cref="CurseType.DamageOverTime"/> curse.
        /// </summary>
        public static int DOTDamageToTick = 0;

        /// <summary>
        /// The amount of heal over time to perform when the player has the
        /// <see cref="CurseType.HealOverTime"/> curse.
        /// </summary>
        public static int HOTHealToTick = 0;

        public Curse(CurseType curseType) : base(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, minutesDuration: 1, "Roguelike", "Roguelike")
        {
            Description = $"{CurseData[curseType].Item1}\n{CurseData[curseType].Item2}";

            if (curseType == CurseType.DoubleSpeedEveryone)
                buffAttributes[9] = 1;

            which = (int)curseType;

            // Give the curse the maximum duration.
            millisecondsDuration = int.MaxValue;
        }

        /// <summary>
        /// Spawns three cherry bombs within 3 tiles of the local player.
        /// </summary>
        public static void SpawnBombsAroundPlayer()
        {
            if (Game1.player.currentLocation is not MineShaft)
                return;

            MineShaft mine = Game1.player.currentLocation as MineShaft;

            StardewValley.Object bomb = new(286, 1);
            int bombsToSpawn = 3;
            int spawnPadding = 3;
            while (bombsToSpawn > 0)
            {
                Vector2 playerPosition = Game1.player.getTileLocation();
                Vector2 tileToSpawn = new(
                    playerPosition.X + Game1.random.Next(-spawnPadding, spawnPadding),
                    playerPosition.Y + Game1.random.Next(-spawnPadding, spawnPadding)
                );
                mine.isTileClearForMineObjects(tileToSpawn);
                tileToSpawn *= 64f;

                bomb.placementAction(mine, (int)tileToSpawn.X, (int)tileToSpawn.Y, Game1.player);
                bombsToSpawn--;
            }
        }

        /// <summary>
        /// Adjusts a passed monster based on the curses
        /// that are active in the game.
        /// </summary>
        /// <param name="monster">The monster to adjust.</param>
        public static void AdjustMonster(ref Monster monster)
        {
            if (AnyFarmerHasCurse(CurseType.MoreEnemiesLessHealth))
            {
                monster.MaxHealth = (int)Math.Round(monster.MaxHealth * .75f);
                monster.Health = monster.MaxHealth;
            }
            if (AnyFarmerHasCurse(CurseType.DoubleSpeedEveryone))
                monster.Speed *= 2;
        }

        public static void UpdateTicked(object sender, UpdateTickedEventArgs e)
        {
            if (!Context.IsWorldReady)
                return;

            bool isMonsterArea = Game1.player.currentLocation is MineShaft mine && (bool)mine.GetType().GetProperty("isMonsterArea", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(mine);
            if (isMonsterArea && HasCurse(CurseType.BombsAroundPlayer) && Game1.shouldTimePass())
            {
                if (TicksToSpawnBombs > 0)
                {
                    TicksToSpawnBombs--;

                    if (TicksToSpawnBombs == 0)
                    {
                        SpawnBombsAroundPlayer();
                        TicksToSpawnBombs = Game1.random.Next(60 * 9, 60 * 12);
                    }
                }
            }

            //if (HasCurse(CurseType.GestureOfTheDrowned))
            //{
            //    if (Game1.player.CurrentTool != null && Game1.player.CurrentTool is MeleeWeapon weapon && Game1.player.CanMove && !Game1.player.canOnlyWalk && !Game1.eventUp && !Game1.player.onBridge.Value)
            //        weapon.animateSpecialMove(Game1.player);
            //}

            if (!e.IsOneSecond)
                return;

            if (HasCurse(CurseType.DamageOverTime) && DOTDamageToTick > 0)
            {
                int toRemove = Math.Min(3, DOTDamageToTick);
                Game1.player.health -= toRemove;
                DOTDamageToTick -= toRemove;
            }

            if (HasCurse(CurseType.HealOverTime) && HOTHealToTick > 0)
            {
                int toAdd = Math.Min(3, HOTHealToTick);
                if (Game1.player.health + toAdd >= Game1.player.maxHealth)
                {
                    Game1.player.health = Game1.player.maxHealth;
                    HOTHealToTick = 0;
                }
                else
                {
                    Game1.player.health += toAdd;
                    HOTHealToTick -= toAdd;
                }
            }
        }

        // Only works for Game1.player
        public static bool HasCurse(CurseType curseType)
        {
            return Game1.buffsDisplay.otherBuffs.Any(b => b.which == (int)curseType);
        }

        public static bool HasAnyCurse()
        {
            return Game1.player.get_FarmerCurses().Count > 0;
        }

        public static bool AnyFarmerHasCurse(CurseType curseType)
        {
            foreach (Farmer farmer in Game1.getOnlineFarmers())
            {
                if (farmer.get_FarmerCurses().Contains((int)curseType))
                    return true;
            }

            return false;
        }

        public static bool AddRandomCurse()
        {
            List<CurseType> curseTypes = Enum.GetValues<CurseType>().ToList();
            curseTypes = curseTypes.Where(c => !HasCurse(c)).ToList();

            if (curseTypes.Count == 0)
                return false;

            CurseType curseType = curseTypes[Game1.random.Next(curseTypes.Count)];
            return AddCurse(curseType);
        }

        public static void RemoveRandomCurse()
        {
            int totalCurses = Game1.player.get_FarmerCurses().Count;
            if (totalCurses == 0)
                return;

            CurseType randomCurse = (CurseType)Game1.player.get_FarmerCurses().ElementAt(Game1.random.Next(totalCurses));
            RemoveCurse(randomCurse);
        }

        public static bool AddCurse(CurseType curseType)
        {
            if (HasCurse(curseType))
                return true;

            switch (curseType)
            {
                case CurseType.BootsBetterDefense:
                case CurseType.BootsBetterImmunity:
                    if (Game1.player.boots.Value != null)
                    {
                        Game1.player.boots.Value.reloadData();
                        Game1.player.boots.Value.onUnequip();
                    }
                    break;
                default:
                    break;
            }

            Curse curse = new(curseType);
            Game1.player.get_FarmerCurses().Add((int)curseType);
            bool result = Game1.buffsDisplay.addOtherBuff(curse);

            switch (curseType)
            {
                case CurseType.GlassCannon:
                    Game1.player.maxHealth /= 2;
                    Game1.player.health = Math.Min(Game1.player.maxHealth, Game1.player.health);
                    Game1.player.enchantments.Add(new GlassCannonEnchantment());
                    break;
                case CurseType.DamageOverTime:
                    DOTDamageToTick = 0;
                    break;
                case CurseType.HealOverTime:
                    HOTHealToTick = 0;
                    break;
                case CurseType.BootsBetterDefense:
                case CurseType.BootsBetterImmunity:
                    if (Game1.player.boots.Value != null)
                    {
                        Game1.player.boots.Value.reloadData();
                        Game1.player.boots.Value.onEquip();
                    }
                    break;
                case CurseType.BuffsMorePotent:
                    if (Game1.buffsDisplay.food is not null)
                    {
                        Game1.buffsDisplay.food.millisecondsDuration = PotentBuffDuration;
                        for (int i = 0; i < Game1.buffsDisplay.food.buffAttributes.Length; i++)
                        {
                            if (Game1.buffsDisplay.food.buffAttributes[i] > 0)
                                Game1.buffsDisplay.food.buffAttributes[i] += 1;
                        }
                    }
                    if (Game1.buffsDisplay.drink is not null)
                    {
                        Game1.buffsDisplay.drink.millisecondsDuration = PotentBuffDuration;
                        for (int i = 0; i < Game1.buffsDisplay.drink.buffAttributes.Length; i++)
                        {
                            if (Game1.buffsDisplay.drink.buffAttributes[i] > 0)
                                Game1.buffsDisplay.drink.buffAttributes[i] += 1;
                        }
                    }
                    Game1.buffsDisplay.syncIcons();
                    break;
                default:
                    break;
            }

            return result;
        }

        public static bool RemoveCurse(CurseType curseType)
        {
            if (!HasCurse(curseType))
                return true;

            switch (curseType)
            {
                case CurseType.GlassCannon:
                    Game1.player.maxHealth = Math.Min(Game1.player.maxHealth * 2, Roguelike.MaxHP);
                    GlassCannonEnchantment toRemove = null;
                    foreach (BaseEnchantment enchantment in Game1.player.enchantments)
                    {
                        if (enchantment is GlassCannonEnchantment)
                        {
                            toRemove = (GlassCannonEnchantment)enchantment;
                            break;
                        }
                    }
                    if (toRemove is not null)
                        Game1.player.enchantments.Remove(toRemove);
                    break;
                case CurseType.DamageOverTime:
                    DOTDamageToTick = 0;
                    break;
                case CurseType.HealOverTime:
                    HOTHealToTick = 0;
                    break;
                case CurseType.BootsBetterDefense:
                case CurseType.BootsBetterImmunity:
                    if (Game1.player.boots.Value != null)
                    {
                        Game1.player.boots.Value.onUnequip();
                    }
                    break;
                default:
                    break;
            }

            Game1.player.get_FarmerCurses().Remove((int)curseType);
            bool result = Game1.buffsDisplay.removeOtherBuff((int)curseType);

            switch (curseType)
            {
                case CurseType.BootsBetterDefense:
                case CurseType.BootsBetterImmunity:
                    if (Game1.player.boots.Value != null)
                    {
                        Game1.player.boots.Value.reloadData();
                        Game1.player.boots.Value.onEquip();
                    }
                    break;
                default:
                    break;
            }

            return result;
        }

        public static void RemoveAllCurses()
        {
            foreach (CurseType curseType in Enum.GetValues(typeof(CurseType)))
                RemoveCurse(curseType);
        }
    }
}
