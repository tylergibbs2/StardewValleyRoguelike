using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Netcode;
using StardewValley;
using StardewValley.Monsters;
using System;
using static StardewRoguelike.Bosses.BigBug;

namespace StardewRoguelike.Bosses
{
    public class BigBugMinion : Fly
    {
        private MinionState previousState;

        public NetEnum<MinionState> CurrentState = new(MinionState.Normal);

        private Color? CurrentColor = null;

        private int[] debuffList = new int[]
        {
            12, 17, 13,
            18, 14, 19,
            25, 26, 27
        };

        private int originalResilience;

        private int originalDamage;

        public BigBugMinion() : base() { }

        public BigBugMinion(Vector2 position, float difficulty, MinionState state) : base(position, false)
        {
            CurrentState.Value = state;

            MaxHealth = (int)(100 * difficulty);
            Health = MaxHealth;
            DamageToFarmer = (int)(12 * difficulty);

            originalResilience = resilience.Value;
            originalDamage = DamageToFarmer;

            moveTowardPlayerThreshold.Value = 35;
        }

        protected override void initNetFields()
        {
            base.initNetFields();
            NetFields.AddFields(CurrentState);
        }

        public void SetDefaults()
        {
            resilience.Value = originalResilience;
            DamageToFarmer = originalDamage;
        }

        public void ChangeState(MinionState state)
        {
            if (CurrentState.Value == MinionState.Suicidal)
                return;

            SetDefaults();
            if (state == MinionState.Defensive)
                resilience.Value *= 5;
            else if (state == MinionState.Aggressive)
                DamageToFarmer *= 2;

            CurrentState.Value = state;
        }

        public override void updateMovement(GameLocation location, GameTime time)
        {
            base.updateMovement(location, time);
            if (CurrentState.Value == MinionState.Fast)
                base.updateMovement(location, time);
        }

        public override void collisionWithFarmerBehavior()
        {
            base.collisionWithFarmerBehavior();

            if (CurrentState.Value == MinionState.Suicidal)
            {
                currentLocation.playSound("explosion");
                currentLocation.explode(new(position.X / 64, position.Y / 64), 2, Game1.player, true, DamageToFarmer * 2);
                takeDamage(MaxHealth * 3, 0, 0, true, 0.0, Game1.player);
            }
        }

        public override void update(GameTime time, GameLocation location)
        {
            base.update(time, location);

            if (previousState != CurrentState.Value)
                CurrentColor = null;

            previousState = CurrentState.Value;

            if (CurrentState.Value == MinionState.Fast)
                CurrentColor = Color.Green;
            else if (CurrentState.Value == MinionState.Debuffing)
                CurrentColor = Color.Black;
            else if (CurrentState.Value == MinionState.Defensive)
                CurrentColor = Color.Turquoise;
            else if (CurrentState.Value == MinionState.Aggressive)
                CurrentColor = Color.Orange;
            else if (CurrentState.Value == MinionState.Suicidal)
                CurrentColor = Color.Red;

            if (OverlapsFarmerForDamage(Game1.player) && !Game1.player.temporarilyInvincible && CurrentState.Value == MinionState.Debuffing)
                Debuff(Game1.player);
        }

        private int GetRandomDebuff()
        {
            int index = Game1.random.Next(debuffList.Length);
            return debuffList[index];
        }

        public override void drawAboveAllLayers(SpriteBatch b)
        {
            if (Utility.isOnScreen(base.Position, 128))
            {
                b.Draw(this.Sprite.Texture, base.getLocalPosition(Game1.viewport) + new Vector2(32f, this.GetBoundingBox().Height / 2 - 32), this.Sprite.SourceRect, this.hard ? Color.Lime : Color.White, base.rotation, new Vector2(8f, 16f), Math.Max(0.2f, base.scale) * 4f, base.flip ? SpriteEffects.FlipHorizontally : SpriteEffects.None, Math.Max(0f, base.drawOnTop ? 0.991f : ((float)(base.getStandingY() + 8) / 10000f)));
                b.Draw(Game1.shadowTexture, base.getLocalPosition(Game1.viewport) + new Vector2(32f, this.GetBoundingBox().Height / 2), Game1.shadowTexture.Bounds, Color.White, 0f, new Vector2(Game1.shadowTexture.Bounds.Center.X, Game1.shadowTexture.Bounds.Center.Y), 4f, SpriteEffects.None, (float)(base.getStandingY() - 1) / 10000f);
                if (isGlowing)
                    b.Draw(this.Sprite.Texture, base.getLocalPosition(Game1.viewport) + new Vector2(32f, this.GetBoundingBox().Height / 2 - 32), this.Sprite.SourceRect, base.glowingColor * base.glowingTransparency, base.rotation, new Vector2(8f, 16f), Math.Max(0.2f, base.scale) * 4f, base.flip ? SpriteEffects.FlipHorizontally : SpriteEffects.None, Math.Max(0f, base.drawOnTop ? 0.99f : ((float)base.getStandingY() / 10000f + 0.001f)));
                else if (CurrentColor is not null)
                    b.Draw(this.Sprite.Texture, base.getLocalPosition(Game1.viewport) + new Vector2(32f, this.GetBoundingBox().Height / 2 - 32), this.Sprite.SourceRect, CurrentColor.Value * 0.75f, base.rotation, new Vector2(8f, 16f), Math.Max(0.2f, base.scale) * 4f, base.flip ? SpriteEffects.FlipHorizontally : SpriteEffects.None, Math.Max(0f, base.drawOnTop ? 0.99f : ((float)base.getStandingY() / 10000f + 0.001f)));
            }
        }

        public void Debuff(Farmer player)
        {
            if (Game1.random.Next(11) >= player.immunity && !player.hasBuff(28))
            {
                int debuff = GetRandomDebuff();
                if (Game1.player == player)
                {
                    Buff buff = new(debuff)
                    {
                        millisecondsDuration = 5000
                    };
                    Game1.buffsDisplay.addOtherBuff(buff);

                }

                if (debuff == 19)
                    currentLocation.playSound("frozen");
                else
                    currentLocation.playSound("debuffHit");
            }
        }
    }
}
