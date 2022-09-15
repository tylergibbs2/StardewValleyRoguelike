using Microsoft.Xna.Framework;
using StardewModdingAPI;
using StardewRoguelike.Bosses;
using StardewRoguelike.Extensions;
using StardewValley;
using StardewValley.Locations;
using StardewValley.Monsters;
using System.Collections.Generic;

namespace StardewRoguelike.ChallengeFloors
{
    internal class HiddenBoss : ChallengeBase
    {
        public override List<string> MapPaths => new() { "custom-lavalurk" };

        public override List<string> GetMusicTracks(MineShaft mine) => new() { "VolcanoMines1", "VolcanoMines2" };

        public override Vector2? GetSpawnLocation(MineShaft mine) => new(20, 41);

        private bool bossSpawned = false;

        private bool spawnedReward = false;

        public override void Initialize(MineShaft mine)
        {
            mine.CreateDwarfGate(1, new(21, 30), new(21, 33));

            BossFloor.SpawnBoss(mine, typeof(HiddenLurker));
            bossSpawned = true;
        }

        public override void PlayerEntered(MineShaft mine)
        {
            base.PlayerEntered(mine);
            mine.forceViewportPlayerFollow = false;
            mine.waterTiles = null;
        }

        public void SpawnReward(MineShaft mine)
        {
            mine.playSound("discoverMineral");
            mine.SpawnLocalChest(new(17, 35), new Object(74, 1));
            mine.projectiles.Clear();
        }

        public override void Update(MineShaft mine, GameTime time)
        {
            if (!bossSpawned || spawnedReward || !Context.IsMainPlayer)
                return;

            foreach (Character character in mine.characters)
            {
                if (character is Monster)
                    return;
            }

            SpawnReward(mine);
            spawnedReward = true;
        }
    }
}
