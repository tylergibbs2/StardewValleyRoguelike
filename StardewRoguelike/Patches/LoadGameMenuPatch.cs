using HarmonyLib;
using StardewValley;
using StardewValley.Menus;
using System.IO;
using System.Reflection;

namespace StardewRoguelike.Patches
{
    [HarmonyPatch(typeof(LoadGameMenu), MethodType.Constructor)]
    internal class LoadGameMenuPatch
    {
        public static bool Prefix(LoadGameMenu __instance)
        {
            if (__instance is CoopMenu)
                return true;

            string path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), $@"assets/Saves/{Roguelike.SaveFile}/SaveGameInfo");
            SaveGame.Load(path);
            Game1.exitActiveMenu();

            return false;
        }
    }
}
