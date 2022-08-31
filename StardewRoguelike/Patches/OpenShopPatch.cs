using StardewRoguelike.UI;
using StardewValley;
using StardewValley.Menus;
using System;

namespace StardewRoguelike.Patches
{
    internal class OpenShopPatch : Patch
    {
        protected override PatchDescriptor GetPatchDescriptor() => new(typeof(GameLocation), "openShopMenu");

        public static bool Prefix(ref bool __result, string which)
        {
            if (which.Equals("Roguelike"))
            {
                if (Merchant.CurrentShop is null)
                {
                    ShopMenu menu;
                    if (Perks.HasPerk(Perks.PerkType.Indecisive))
                        menu = new RefreshableShopMenu(Merchant.GetMerchantStock(), false, context: "Blacksmith");
                    else
                        menu = new(Merchant.GetMerchantStock(), context: "Blacksmith");
                    menu.setUpStoreForContext();
                    Merchant.CurrentShop = menu;
                }
                else if (Merchant.CurrentShop is not RefreshableShopMenu && Perks.HasPerk(Perks.PerkType.Indecisive))
                    Merchant.CurrentShop = new RefreshableShopMenu(Merchant.CurrentShop.itemPriceAndStock, false, context: "Blacksmith");

                Game1.activeClickableMenu = Merchant.CurrentShop;

                __result = true;
                return false;
            }
            else if (which.Equals("RoguelikeDiscounted"))
            {
                if (Merchant.CurrentShop is null)
                {
                    ShopMenu menu = new(Merchant.GetMerchantStock(0.5f), context: "Blacksmith");
                    menu.setUpStoreForContext();
                    Merchant.CurrentShop = menu;
                }
                Game1.activeClickableMenu = Merchant.CurrentShop;

                __result = true;
                return false;
            }
            else if (which.Equals("Perks"))
            {
                Perks.CurrentMenu ??= new PerkMenu();
                Perks.CurrentMenu.isActive = true;
                Perks.CurrentMenu.informationUp = true;
                Game1.activeClickableMenu = Perks.CurrentMenu;

                __result = true;
                return false;
            }

            return true;
        }
    }
}
