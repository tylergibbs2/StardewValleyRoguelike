using Microsoft.Xna.Framework;
using Netcode;
using StardewModdingAPI;
using StardewRoguelike.Extensions;
using StardewRoguelike.UI;
using StardewValley;
using StardewValley.Locations;
using StardewValley.Menus;
using StardewValley.Monsters;
using StardewValley.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace StardewRoguelike
{
    class CommandHandler
    {
        /// <summary>
        /// Valid commands that can be used in chat.
        /// Key is the command name (e.g "/stats"), value is the
        /// action that occurs upon command usage.
        /// </summary>
        private static readonly Dictionary<string, Action<string[]>> commands = new()
        {
            { "stats", ShowStats },
            { "character", Character },
            { "reset", Reset },
            { "stuck", Stuck }
        };

        /// <summary>
        /// Handles command parsing from a raw string.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns>false if the game should stop parsing this string, true otherwise.</returns>
        public static bool Handle(string command)
        {
            command = command.Trim();
            string[] args = command.Split(' ');

            if (commands.ContainsKey(args[0]))
            {
                commands[args[0]](args.Skip(1).ToArray());
                return false;
            }

            return true;
        }

        /// <summary>
        /// Command handler for showing the player's current statistics.
        /// Opens the stats menu.
        /// </summary>
        /// <param name="args">The command arguments.</param>
        public static void ShowStats(string[] args)
        {
            if (Game1.activeClickableMenu is not null)
            {
                Game1.chatBox.addErrorMessage("Close existing menu before showing stats.");
                return;
            }

            StatsMenu.Show();
        }

        /// <summary>
        /// Command handler for editing the player's character.
        /// Opens the character customization menu.
        /// </summary>
        /// <param name="args">The command arguments.</param>
        public static void Character(string[] args)
        {
            if (Game1.activeClickableMenu is not null)
            {
                Game1.chatBox.addErrorMessage("Close existing menu before customizing your character.");
                return;
            }

            Game1.activeClickableMenu = new CharacterCustomization(CharacterCustomization.Source.NewFarmhand);
        }

        /// <summary>
        /// Resets the current run.
        /// </summary>
        /// <param name="args">The command arguments.</param>
        public static void Reset(string[] args)
        {
            if (!Context.IsMainPlayer)
            {
                Game1.chatBox.addErrorMessage("Only the host can reset the run.");
                return;
            }

            ModEntry.MultiplayerHelper.SendMessage(
                "GameOver",
                "GameOver"
            );
            Roguelike.GameOver();
        }

        /// <summary>
        /// Helps you when you're stuck.
        /// </summary>
        /// <param name="args">The command arguments.</param>
        public static void Stuck(string[] args)
        {
            if (!Context.IsMainPlayer)
            {
                Game1.chatBox.addErrorMessage("Only the host can use this command.");
                return;
            }

            if (Game1.player.currentLocation is not MineShaft mine)
            {
                Game1.chatBox.addErrorMessage("You cannot use this command here.");
                return;
            }

            int monstersOffMap = 0;
            foreach (Character character in mine.characters)
            {
                if (character is Monster monster)
                {
                    Vector2 tileLoc = monster.getTileLocation();
                    Point tilePoint = new((int)tileLoc.X, (int)tileLoc.Y);
                    int backTile = mine.getTileIndexAt(tilePoint, "Back");
                    int backbackTile = mine.getTileIndexAt(tilePoint, "BackBack");
                    int backbackbackTile = mine.getTileIndexAt(tilePoint, "BackBackBack");
                    if (backTile == -1 && backbackTile == -1 && backbackbackTile == -1)
                    {
                        monstersOffMap++;

                        Vector2 newPos;
                        do
                        {
                            newPos = mine.getRandomTile();
                        } while (!mine.isTileOnClearAndSolidGround(newPos));
                        monster.setTileLocation(newPos);
                    }

                    monster.moveTowardPlayerThreshold.Value = 50;
                    monster.focusedOnFarmers = true;
                }
            }

            if (monstersOffMap == 0)
            {
                bool ladderHasSpawned = (bool)mine.GetType().GetField("ladderHasSpawned", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(mine);

                if (!ladderHasSpawned && mine.EnemyCount == 0 && (mine.IsNormalFloor() || BossFloor.IsBossFloor(mine)))
                {
                    Vector2 tileBeneathLadder = (Vector2)mine.GetType().GetProperty("tileBeneathLadder", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(mine);
                    NetVector2Dictionary<bool, NetBool> createLadderEvent = (NetVector2Dictionary<bool, NetBool>)mine.GetType().GetField("createLadderAtEvent", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(mine);
                    createLadderEvent[tileBeneathLadder] = true;
                    Game1.chatBox.addInfoMessage("Spawned ladder.");
                }
                else if (!ladderHasSpawned && mine.EnemyCount == 0 && !mine.IsNormalFloor() && !BossFloor.IsBossFloor(mine))
                    Game1.chatBox.addErrorMessage("You cannot use this command here.");
                else
                    Game1.chatBox.addInfoMessage("All monsters have been detected to be on the map.");
                return;
            }
            else
                Game1.chatBox.addInfoMessage($"Found {monstersOffMap} off the map, they have been repositioned.");
        }
    }
}