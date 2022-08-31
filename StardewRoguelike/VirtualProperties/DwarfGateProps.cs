using Netcode;
using StardewValley.Locations;
using System.Runtime.CompilerServices;

namespace StardewRoguelike.VirtualProperties
{
    public static class DwarfGateDisabled
    {
        internal class Holder { public readonly NetBool Value = new(); }

        internal static ConditionalWeakTable<DwarfGate, Holder> values = new();

        public static NetBool get_DwarfGateDisabled(this DwarfGate gate)
        {
            var holder = values.GetOrCreateValue(gate);
            return holder.Value;
        }
    }
}
