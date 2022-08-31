using Microsoft.Xna.Framework;
using StardewValley.Monsters;
using System;

namespace StardewRoguelike.Bosses
{
    public class QueenBeeMinion : Fly
    {
        private string textureName = "Characters\\Monsters\\Fly_dangerous";

        public QueenBeeMinion () { }

        public QueenBeeMinion(Vector2 position, float difficulty) : base(position, false)
        {
            setTileLocation(position);

            MaxHealth = (int)Math.Round(60 * difficulty);
            Health = MaxHealth;

            DamageToFarmer = (int)Math.Round(15 * difficulty);

            Sprite = new(textureName);
        }

        public override void reloadSprite()
        {
            Sprite = new(textureName);
            HideShadow = true;
        }
    }
}
