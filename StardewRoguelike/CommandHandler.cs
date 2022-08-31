using StardewModdingAPI;
using StardewRoguelike.UI;
using StardewValley;
using StardewValley.Menus;
using System;
using System.Collections.Generic;
using System.Linq;

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
            { "reset", Reset }
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
    }
}