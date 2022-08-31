﻿using StardewValley.Locations;
using System;
using System.Reflection;
using StardewRoguelike.VirtualProperties;
using StardewValley;
using StardewRoguelike.Extensions;

namespace StardewRoguelike.Patches
{
    internal class GetMinePatch : Patch
    {
        protected override PatchDescriptor GetPatchDescriptor() => new(typeof(MineShaft), "GetMine");

        public static bool Prefix(ref MineShaft __result, string name)
        {
            // requestedFloor/requestedLevel
            string format = name["UndergroundMine".Length..];
            string floorString = format.Split("/")[0];
            string levelString = format.Split("/")[1];

            int requestedFloor = Convert.ToInt32(floorString);
            int requestedLevel = Convert.ToInt32(levelString);

            foreach (MineShaft mine in MineShaft.activeMines)
            {
                if (MineShaftLevel.get_MineShaftLevel(mine).Value == requestedLevel)
                {
                    __result = mine;
                    return false;
                }
            }

            MineShaft newMine = new(0);
            newMine.get_MineShaftLevel().Value = requestedLevel;
            newMine.get_MineShaftEntryTime().Value = ((DateTimeOffset)DateTime.UtcNow).ToUnixTimeSeconds();

            if (Merchant.IsMerchantFloor(requestedLevel))
                Merchant.SpawnMarlon(newMine);
            else if (BossFloor.IsBossFloor(requestedLevel))
                BossFloor.SpawnBoss(newMine);
            else if (ForgeFloor.ShouldDoForgeFloor(requestedLevel))
                newMine.get_MineShaftForgeFloor().Value = true;
            else if (ChestFloor.ShouldDoChestFloor(requestedLevel))
            {
                newMine.get_MineShaftChestFloor().Value = true;
                ChestFloor.SpawnChest(newMine);
            }
            else if (ChallengeFloor.ShouldDoChallengeFloor(requestedLevel))
            {
                newMine.get_MineShaftIsChallengeFloor().Value = true;
                newMine.get_MineShaftChallengeFloor().Value = ChallengeFloor.GetRandomChallenge(requestedLevel);
            }

            double darkChance;
            if (DebugCommands.ForcedDarkChance > 0f)
                darkChance = DebugCommands.ForcedDarkChance;
            else
                darkChance = 0.2;

            if (Roguelike.HardMode && Game1.random.NextDouble() < darkChance && (newMine.IsNormalFloor() || Merchant.IsMerchantFloor(newMine)))
                newMine.set_MineShaftIsDarkArea(true);

            int depth = Roguelike.GetFloorDepth(newMine, requestedFloor);

            newMine.mineLevel = depth;
            newMine.name.Value = name;

            MineShaft.activeMines.Add(newMine);

            newMine.GetType().GetMethod("generateContents", BindingFlags.Instance | BindingFlags.NonPublic).Invoke(newMine, null);

			__result = newMine;

            Game1.netWorldState.Value.MinesDifficulty = 0;
            Game1.netWorldState.Value.SkullCavesDifficulty = 0;

            if (newMine.get_MineShaftIsChallengeFloor().Value)
                newMine.get_MineShaftChallengeFloor().Value.Initialize(newMine);

            return false;
        }
    }
}
