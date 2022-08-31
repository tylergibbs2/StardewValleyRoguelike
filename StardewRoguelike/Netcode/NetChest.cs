using Microsoft.Xna.Framework;
using Netcode;
using StardewValley;
using StardewValley.Locations;
using StardewValley.Objects;
using System.Collections.Generic;
using System.Linq;

namespace StardewRoguelike.Netcode
{
    public class NetChest : INetObject<NetFields>
    {
        public NetFields NetFields { get; } = new();

        public NetVector2 TileLocation = new();

        public NetObjectList<Item> Items = new();

        private bool wasSpawned = false;

        public NetChest()
        {
            InitNetFields();
        }

        public NetChest(Vector2 tileLocation, List<Item> items) : this()
        {
            TileLocation.Value = tileLocation;
            foreach (Item item in items)
                Items.Add(item);
        }

        protected void InitNetFields()
        {
            NetFields.AddFields(TileLocation, Items);
        }

        public void Spawn(MineShaft mine)
        {
            if (wasSpawned)
                return;

            Chest chest = new(0, Items.ToList(), TileLocation.Value)
            {
                Tint = Color.White
            };
            mine.overlayObjects[TileLocation.Value] = chest;

            wasSpawned = true;
        }
    }
}
