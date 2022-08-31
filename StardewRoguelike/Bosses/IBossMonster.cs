using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace StardewRoguelike.Bosses
{
    interface IBossMonster
    {
        string DisplayName { get; }

        string MapPath { get; }

        string TextureName { get; }

        Vector2 SpawnLocation { get; }

        List<string> MusicTracks { get; }

        bool InitializeWithHealthbar { get; }

        float Difficulty { get; set; }
    }
}
