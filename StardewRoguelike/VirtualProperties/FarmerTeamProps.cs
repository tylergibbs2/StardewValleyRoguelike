using Netcode;
using StardewValley;
using System.Runtime.CompilerServices;

namespace StardewRoguelike.VirtualProperties
{
    public static class FarmerTeamHardMode
    {
        internal class Holder { public readonly NetBool Value = new(); }

        internal static ConditionalWeakTable<FarmerTeam, Holder> values = new();

        public static NetBool get_FarmerTeamHardMode(this FarmerTeam team)
        {
            var holder = values.GetOrCreateValue(team);
            return holder.Value;
        }
    }
}
