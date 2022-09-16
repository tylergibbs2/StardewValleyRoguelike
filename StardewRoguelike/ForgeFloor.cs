using StardewRoguelike.UI;
using StardewRoguelike.VirtualProperties;
using StardewValley.Locations;
using System.Collections.Generic;

namespace StardewRoguelike
{
    public class ForgeFloor
    {
        public static readonly string ForgeFloorMapPath = "custom-forge";

        public static readonly int ForgeFloorMinimumLevel = 25;

        public static readonly int ForgeFloorMinimumInterval = 10;

        public static readonly double ForgeFloorChance = 0.15;

        public static LimitedForgeMenu CurrentForge = null;

        public static bool ShouldDoForgeFloor(int level)
        {
            if (DebugCommands.ForcedForgeFloor)
                return true;

            int lastForgeFloor = 0;
            int highestFloor = Roguelike.GetHighestMineShaftLevel();
            foreach (MineShaft mine in MineShaft.activeMines)
            {
                int floor = Roguelike.GetLevelFromMineshaft(mine);
                if (IsForgeFloor(mine) && floor > lastForgeFloor)
                    lastForgeFloor = floor;
            }

            if (lastForgeFloor == 0)
                return level > ForgeFloorMinimumLevel;

            return Roguelike.FloorRng.NextDouble() <= ForgeFloorChance && level > highestFloor && (level - lastForgeFloor) >= ForgeFloorMinimumInterval;
        }

        public static bool IsForgeFloor(MineShaft mine)
        {
            return mine.get_MineShaftForgeFloor().Value;
        }

        public static List<string> GetMusicTracks()
        {
            return new() { "Volcano_Ambient" };
        }
    }
}
