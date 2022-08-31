using Microsoft.Xna.Framework;
using StardewValley.Locations;
using System.Collections.Generic;

namespace StardewRoguelike.ChallengeFloors
{
    public class HotSpring : ChallengeBase
    {
        public HotSpring() : base() { }

        public override List<string> MapPaths => new() { "custom-hotspring" };

        public override Vector2? SpawnLocation => new(5, 5);

        public override bool ShouldSpawnLadder(MineShaft mine) => false;
    }
}
