using StardewRoguelike.UI;
using StardewValley;
using StardewValley.Menus;

namespace StardewRoguelike.Patches
{
    internal class OpenShopPatch : Patch
    {
        protected override PatchDescriptor GetPatchDescriptor() => new(typeof(GameLocation), "openShopMenu");

        public static bool OnPurchase(ISalable item, Farmer who, int numberToBuy)
        {
            if (item is Object boughtItem && boughtItem.ParentSheetIndex == 803)
                Roguelike.MilksBought += numberToBuy;

            return false;
        }

        public static bool Prefix(ref bool __result, string which)
        {
            if (which.Equals("Roguelike"))
            {
                if (Merchant.CurrentShop is null)
                {
                    ShopMenu menu;
                    if (Perks.HasPerk(Perks.PerkType.Indecisive))
                        menu = new RefreshableShopMenu(Merchant.GetMerchantStock(), false, context: "Blacksmith", on_purchase: OnPurchase);
                    else
                        menu = new(Merchant.GetMerchantStock(), context: "Blacksmith", on_purchase: OnPurchase);
                    menu.setUpStoreForContext();
                    Merchant.CurrentShop = menu;
                }
                else if (Merchant.CurrentShop is not RefreshableShopMenu && Perks.HasPerk(Perks.PerkType.Indecisive))
                {
                    if (Merchant.CurrentShop is null)
                        Merchant.CurrentShop = new RefreshableShopMenu(Merchant.GetMerchantStock(), false, context: "Blacksmith", on_purchase: OnPurchase);
                    else
                        Merchant.CurrentShop = new RefreshableShopMenu(Merchant.CurrentShop.itemPriceAndStock, false, context: "Blacksmith", on_purchase: OnPurchase);
                }

                Game1.activeClickableMenu = Merchant.CurrentShop;

                __result = true;
                return false;
            }
            else if (which.Equals("RoguelikeDiscounted"))
            {
                if (Merchant.CurrentShop is null)
                {
                    ShopMenu menu = new(Merchant.GetMerchantStock(0.5f), context: "Blacksmith", on_purchase: OnPurchase);
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
