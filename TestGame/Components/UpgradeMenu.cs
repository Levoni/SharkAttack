using Base.Events;
using Base.UI;
using Base.Utility;
using Base.Utility.Services;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using TestGame.Events;
using TestGame.Model;
using TestGame.Utility;
using TestGame.Utility.Services;

namespace TestGame.Components
{
   [Serializable]
   public class UpgradeMenu : Control
   {
      public EventBus parentGUIBus { get; set; }
      public enum shopTypeEnum
      {
         weapon,
         ability,
         stat,
         trinket
      }
      bool canPurchase;

      shopTypeEnum currentShopType;
      TestGameSaveFile saveFile;

      Button back;

      Label shopType;
      PictureBox sidebarImage;
      Button dojoShop;
      Button TempleShop;
      Button WeaponShop;
      Button TrinketShop;

      //weapon shop
      ListBox ShopInventoryList;
      Label ShopItemDescription;
      Label ShopItemLevel;
      Label ShopItemCurrentStat;
      Label ShopItemPrice;
      Label CurrentMoney;
      Button Purchase;
      Button shopImage;
      Button ShopItemImage;
      Button ShopItemLevelImage;

      public UpgradeMenu(string name, string value, EngineRectangle bounds, Color color) : base(name, value, bounds, color)
      {
         saveFile = (TestGameSaveFile)SaveService.Save; 
         setImageReferences("none", "none", "none", "none");
         sidebarImage = new PictureBox("picSideBar", new EngineRectangle(0, 0, bounds.Width / 8, bounds.Height), Color.White, "sidebar_background");
         currentShopType = shopTypeEnum.stat;
         dojoShop = new Button("dojoSHop", "Dojo", new EngineRectangle(0, bounds.Height / 8 * 2, bounds.Width / 8, bounds.Height / 8), Color.White);
         dojoShop.onClick = new EHandler<Base.Events.OnClick>(new Action<object, OnClick>(HandleChangeShopClick));
         dojoShop.setImageReferences("shop_button_none", "shop_button_hover", "shop_button_hover", "shop_button_hover");
         TempleShop = new Button("templeShop", "Temple", new EngineRectangle(0, bounds.Height / 8 * 3, bounds.Width / 8, bounds.Height / 8), Color.White);
         TempleShop.onClick = new EHandler<Base.Events.OnClick>(new Action<object, OnClick>(HandleChangeShopClick));
         TempleShop.setImageReferences("shop_button_none", "shop_button_hover", "shop_button_hover", "shop_button_hover");
         WeaponShop = new Button("weaponShop", "Armory", new EngineRectangle(0, bounds.Height / 8 * 4, bounds.Width / 8, bounds.Height / 8), Color.White);
         WeaponShop.onClick = new EHandler<Base.Events.OnClick>(new Action<object, OnClick>(HandleChangeShopClick));
         WeaponShop.setImageReferences("shop_button_none", "shop_button_hover", "shop_button_hover", "shop_button_hover");
         TrinketShop = new Button("trinketShop", "Trinket store", new EngineRectangle(0, bounds.Height / 8 * 5, bounds.Width / 8, bounds.Height / 8), Color.White);
         TrinketShop.minFontScale = 0;
         TrinketShop.onClick = new EHandler<Base.Events.OnClick>(new Action<object, OnClick>(HandleChangeShopClick));
         TrinketShop.setImageReferences("shop_button_none", "shop_button_hover", "shop_button_hover", "shop_button_hover");
         shopType = new Label("shopType", "Dojo", new EngineRectangle(), Color.Black);
         back = new Button("backButton", "Back", new EngineRectangle(0,0, bounds.Width / 8, bounds.Width / 32), Color.White);
         back.minFontScale = 0;
         back.maxFontScale = 1f;
         back.onClick = new EHandler<OnClick>(new Action<object, OnClick>(HandleBackClick));
         CurrentMoney = new Label("currentMoneyLabel",$"Money: {saveFile.money}",new EngineRectangle(bounds.Width - 350, bounds.Height - 125, 300,100), Color.White);
         CurrentMoney.minFontScale = 0;
         CurrentMoney.maxFontScale = .5f;
         LoadBaseUI();
         LoadDojoShop();
         init();
      }

      public override void Update(int dt)
      {
         base.Update(dt);
         sidebarImage.Update(dt);
         back.Update(dt);
         dojoShop.Update(dt);
         TempleShop.Update(dt);
         WeaponShop.Update(dt);
         TrinketShop.Update(dt);
         ShopInventoryList.Update(dt);
         ShopItemImage.Update(dt);
         ShopItemLevel.Update(dt);
         ShopItemDescription.Update(dt);
         CurrentMoney.Update(dt);
         ShopItemCurrentStat.Update(dt);

         if(canPurchase)
         {
            ShopItemPrice.Update(dt);
            Purchase.Update(dt);
         }
      }

      public override void Render(SpriteBatch sb)
      {
         base.Render(sb);
         sidebarImage.Render(sb);
         back.Render(sb);
         dojoShop.Render(sb);
         TempleShop.Render(sb);
         WeaponShop.Render(sb);
         TrinketShop.Render(sb);
         ShopInventoryList.Render(sb);
         ShopItemImage.Render(sb);
         ShopItemLevel.Render(sb);
         ShopItemDescription.Render(sb);
         Purchase.Render(sb);
         CurrentMoney.Render(sb);
         ShopItemCurrentStat.Render(sb);

         ShopItemPrice.Render(sb);
      }

      private void LoadBaseUI()
      {
         EngineRectangle bounds = ScreenGraphicService.GetViewportBounds();

         currentShopType = shopTypeEnum.weapon;
         saveFile = (TestGameSaveFile)SaveService.Save;
         List<WeaponModel> weapons = ShopService.Weapons;

         shopImage = new Button("shopImage", "", new EngineRectangle(), Color.Black);
         ShopItemImage = new Button("shopItemImage", "", new EngineRectangle(bounds.Width / 8 * 5, bounds.Height / 4, bounds.Height / 8, bounds.Height / 8), Color.Black);
         ShopItemDescription = new Label("ItemDescription", "Item Description", new EngineRectangle(bounds.Width / 8 * 6, bounds.Height / 4, bounds.Width / 8 * 2 - 25, bounds.Height / 4), Color.White);
         ShopItemDescription.minFontScale = 0;
         ShopItemDescription.maxFontScale = .5f;
         ShopItemDescription.isMultiLine = true;
         ShopItemLevel = new Label("itemLevel", "Level: ", new EngineRectangle(bounds.Width / 8 * 5, bounds.Height / 16 * 6, bounds.Width / 4, bounds.Height / 16), Color.White);
         ShopItemLevel.minFontScale = 0;
         ShopItemLevelImage = new Button("shopItemLevelImage", "", new EngineRectangle(), Color.Black);
         ShopItemCurrentStat = new Label("currentStatLabel", "", new EngineRectangle(bounds.Width / 8 * 5, bounds.Height / 16 * 7, bounds.Width / 4, bounds.Height / 16), Color.White);
         ShopItemPrice = new Label("priceLabel", "", new EngineRectangle(bounds.Width / 8 * 5, bounds.Height / 16 * 8, bounds.Width / 4, bounds.Height / 16), Color.White);
         ShopItemPrice.minFontScale = 0;

         dojoShop.setImageReferences("shop_button_none", "shop_button_hover", "shop_button_hover", "shop_button_hover");
         TempleShop.setImageReferences("shop_button_none", "shop_button_hover", "shop_button_hover", "shop_button_hover");
         WeaponShop.setImageReferences("shop_button_none", "shop_button_hover", "shop_button_hover", "shop_button_hover");
         TrinketShop.setImageReferences("shop_button_none", "shop_button_hover", "shop_button_hover", "shop_button_hover");

         Purchase = new Button("purchaseButton", "Buy", new EngineRectangle(bounds.Width / 8 * 5, bounds.Height / 8 * 5, bounds.Width / 4, bounds.Height / 8), Color.White);
         Purchase.onClick = new EHandler<OnClick>(new Action<object, OnClick>(HandlePurchaseClick));
      }

      private void LoadDojoShop()
      {
         EngineRectangle bounds = ScreenGraphicService.GetViewportBounds();

         currentShopType = shopTypeEnum.stat;
         saveFile = (TestGameSaveFile)SaveService.Save;
         List<StatModel> stats = ShopService.Stats;

         dojoShop.setImageReferences("shop_button_selected_none", "shop_button_selected_none", "shop_button_selected_none", "shop_button_selected_none");

         //shopImage = new Button("shopImage", "", new EngineRectangle(), Color.Black);

         //ShopItemImage = new Button("shopItemImage", "", new EngineRectangle(), Color.Black);
         //ShopItemDescription = new Label("ItemDescription", "Item Description", new EngineRectangle(bounds.Width / 8 * 5, bounds.Height / 4, bounds.Width / 4, bounds.Height / 8), Color.White);
         //ShopItemDescription.minFontScale = 0;
         //ShopItemDescription.maxFontScale = .5f;
         //ShopItemDescription.isMultiLine = true;
         //ShopItemLevel = new Label("itemLevel", "Level: ", new EngineRectangle(bounds.Width / 8 * 5, bounds.Height / 16 * 6, bounds.Width / 4, bounds.Height / 16), Color.White);
         //ShopItemLevel.minFontScale = 0;
         //ShopItemLevelImage = new Button("shopItemLevelImage", "", new EngineRectangle(), Color.Black);
         //ShopItemCurrentStat = new Label("currentStatLabel", "", new EngineRectangle(bounds.Width / 8 * 5, bounds.Height / 16 * 7, bounds.Width / 4, bounds.Height / 16), Color.White);
         //ShopItemPrice = new Label("priceLabel", "", new EngineRectangle(bounds.Width / 8 * 5, bounds.Height / 16 * 8, bounds.Width / 4, bounds.Height / 16), Color.White);
         //ShopItemPrice.minFontScale = 0;

         //Purchase = new Button("purchaseButton", "Buy", new EngineRectangle(bounds.Width / 8 * 5, bounds.Height / 8 * 5, bounds.Width / 4, bounds.Height / 8), Color.White);
         //Purchase.onClick = new EHandler<OnClick>(new Action<object, OnClick>(HandlePurchaseClick));

         ShopInventoryList = new ListBox("shopInventory", "", new EngineRectangle(bounds.Width / 4, bounds.Height / 4, bounds.Width / 4, bounds.Height / 2), Color.Black);
         ShopInventoryList.minFontScale = 0;
         ShopInventoryList.maxFontScale = 1f;
         foreach (StatModel wm in stats)
         {
            ShopInventoryList.AddItem(new ListBoxItem(wm));
         }
         HandleChangeItemEvent(ShopInventoryList, new OnChange("", "0"));
         ShopInventoryList.onValueChange = new EHandler<OnChange>(new Action<object, OnChange>(HandleChangeItemEvent));
      }

      private void LoadTempleShop()
      {
         EngineRectangle bounds = ScreenGraphicService.GetViewportBounds();

         currentShopType = shopTypeEnum.ability;
         saveFile = (TestGameSaveFile)SaveService.Save;
         List<AbilityModel> abilities = ShopService.Abilities;

         TempleShop.setImageReferences("shop_button_selected_none", "shop_button_selected_none", "shop_button_selected_none", "shop_button_selected_none");


         //shopImage = new Button("shopImage", "", new EngineRectangle(), Color.Black);


         //ShopItemImage = new Button("shopItemImage", "", new EngineRectangle(), Color.Black);
         //ShopItemDescription = new Label("ItemDescription", "Item Description", new EngineRectangle(bounds.Width / 8 * 5, bounds.Height / 4, bounds.Width / 4, bounds.Height / 8), Color.White);
         //ShopItemDescription.minFontScale = 0;
         //ShopItemDescription.maxFontScale = .5f;
         //ShopItemDescription.isMultiLine = true;
         //ShopItemLevel = new Label("itemLevel", "Level: ", new EngineRectangle(bounds.Width / 8 * 5, bounds.Height / 16 * 6, bounds.Width / 4, bounds.Height / 16), Color.White);
         //ShopItemLevel.minFontScale = 0;
         //ShopItemLevelImage = new Button("shopItemLevelImage", "", new EngineRectangle(), Color.Black);
         //ShopItemCurrentStat = new Label("currentStatLabel", "", new EngineRectangle(bounds.Width / 8 * 5, bounds.Height / 16 * 7, bounds.Width / 4, bounds.Height / 16), Color.White);
         //ShopItemPrice = new Label("priceLabel", "", new EngineRectangle(bounds.Width / 8 * 5, bounds.Height / 16 * 8, bounds.Width / 4, bounds.Height / 16), Color.White);
         //ShopItemPrice.minFontScale = 0;


         //Purchase = new Button("purchaseButton", "Buy", new EngineRectangle(bounds.Width / 8 * 5, bounds.Height / 8 * 5, bounds.Width / 4, bounds.Height / 8), Color.White);
         //Purchase.onClick = new EHandler<OnClick>(new Action<object, OnClick>(HandlePurchaseClick));

         ShopInventoryList = new ListBox("shopInventory", "", new EngineRectangle(bounds.Width / 4, bounds.Height / 4, bounds.Width / 4, bounds.Height / 2), Color.Black);
         ShopInventoryList.minFontScale = 0;
         ShopInventoryList.maxFontScale = 1f;
         foreach (AbilityModel am in abilities)
         {
            ShopInventoryList.AddItem(new ListBoxItem(am));
         }
         HandleChangeItemEvent(ShopInventoryList, new OnChange("", "0"));
         ShopInventoryList.onValueChange = new EHandler<OnChange>(new Action<object, OnChange>(HandleChangeItemEvent));
      }

      private void LoadArmoryShop()
      {
         EngineRectangle bounds = ScreenGraphicService.GetViewportBounds();

         currentShopType = shopTypeEnum.weapon;
         saveFile = (TestGameSaveFile)SaveService.Save;
         List<WeaponModel> weapons = ShopService.Weapons;

         WeaponShop.setImageReferences("shop_button_selected_none", "shop_button_selected_none", "shop_button_selected_none", "shop_button_selected_none");

         //shopImage = new Button("shopImage", "", new EngineRectangle(), Color.Black);

         //ShopItemImage = new Button("shopItemImage", "", new EngineRectangle(), Color.Black);
         //ShopItemDescription = new Label("ItemDescription", "Item Description", new EngineRectangle(bounds.Width / 8 * 5, bounds.Height / 4, bounds.Width / 4, bounds.Height / 4), Color.White);
         //ShopItemDescription.minFontScale = 0;
         //ShopItemDescription.maxFontScale = .5f;
         //ShopItemDescription.isMultiLine = true;
         //ShopItemLevel = new Label("itemLevel", "Level: ", new EngineRectangle(bounds.Width / 8 * 5, bounds.Height / 16 * 6, bounds.Width / 4, bounds.Height / 16), Color.White);
         //ShopItemLevel.minFontScale = 0;
         //ShopItemLevelImage = new Button("shopItemLevelImage", "", new EngineRectangle(), Color.Black);
         //ShopItemCurrentStat = new Label("currentStatLabel", "", new EngineRectangle(bounds.Width / 8 * 5, bounds.Height / 16 * 7, bounds.Width / 4, bounds.Height / 16), Color.White);
         //ShopItemPrice = new Label("priceLabel", "", new EngineRectangle(bounds.Width / 8 * 5, bounds.Height / 16 * 8, bounds.Width / 4, bounds.Height / 16), Color.White);
         //ShopItemPrice.minFontScale = 0;

         //Purchase = new Button("purchaseButton", "Buy", new EngineRectangle(bounds.Width / 8 * 5, bounds.Height / 8 * 5, bounds.Width / 4, bounds.Height / 8), Color.White);
         //Purchase.onClick = new EHandler<OnClick>(new Action<object, OnClick>(HandlePurchaseClick));

         ShopInventoryList = new ListBox("shopInventory", "", new EngineRectangle(bounds.Width / 4, bounds.Height / 4, bounds.Width / 4, bounds.Height / 2), Color.Black);
         ShopInventoryList.minFontScale = 0;
         ShopInventoryList.maxFontScale = 1f;
         foreach (WeaponModel wm in weapons)
         {
            if (wm.ItemName != "Blast Gun" || saveFile.BeatGame)
            {
               ShopInventoryList.AddItem(new ListBoxItem(wm));
            }
         }
         HandleChangeItemEvent(ShopInventoryList, new OnChange("", "0"));
         ShopInventoryList.onValueChange = new EHandler<OnChange>(new Action<object, OnChange>(HandleChangeItemEvent));
      }

      private void LoadTrinketShop()
      {
         EngineRectangle bounds = ScreenGraphicService.GetViewportBounds();

         currentShopType = shopTypeEnum.trinket;
         saveFile = (TestGameSaveFile)SaveService.Save;
         List<SecendaryWeaponModel> secendaryWeapons = ShopService.SecedaryWeapons;

         TrinketShop.setImageReferences("shop_button_selected_none", "shop_button_selected_none", "shop_button_selected_none", "shop_button_selected_none");

         //shopImage = new Button("shopImage", "", new EngineRectangle(), Color.Black);

         //ShopItemImage = new Button("shopItemImage", "", new EngineRectangle(), Color.Black);
         //ShopItemDescription = new Label("ItemDescription", "Item Description", new EngineRectangle(bounds.Width / 8 * 5, bounds.Height / 4, bounds.Width / 4, bounds.Height / 4), Color.White);
         //ShopItemDescription.minFontScale = 0;
         //ShopItemDescription.maxFontScale = .5f;
         //ShopItemDescription.isMultiLine = true;
         //ShopItemLevel = new Label("itemLevel", "Level: ", new EngineRectangle(bounds.Width / 8 * 5, bounds.Height / 16 * 6, bounds.Width / 4, bounds.Height / 16), Color.White);
         //ShopItemLevel.minFontScale = 0;
         //ShopItemLevelImage = new Button("shopItemLevelImage", "", new EngineRectangle(), Color.Black);
         //ShopItemCurrentStat = new Label("currentStatLabel", "", new EngineRectangle(bounds.Width / 8 * 5, bounds.Height / 16 * 7, bounds.Width / 4, bounds.Height / 16), Color.White);
         //ShopItemPrice = new Label("priceLabel", "", new EngineRectangle(bounds.Width / 8 * 5, bounds.Height / 16 * 8, bounds.Width / 4, bounds.Height / 16), Color.White);
         //ShopItemPrice.minFontScale = 0;

         //Purchase = new Button("purchaseButton", "Buy", new EngineRectangle(bounds.Width / 8 * 5, bounds.Height / 8 * 5, bounds.Width / 4, bounds.Height / 8), Color.White);
         //Purchase.onClick = new EHandler<OnClick>(new Action<object, OnClick>(HandlePurchaseClick));

         ShopInventoryList = new ListBox("shopInventory", "", new EngineRectangle(bounds.Width / 4, bounds.Height / 4, bounds.Width / 4, bounds.Height / 2), Color.Black);
         ShopInventoryList.minFontScale = 0;
         ShopInventoryList.maxFontScale = 1f;
         foreach (SecendaryWeaponModel swm in secendaryWeapons)
         {
            ShopInventoryList.AddItem(new ListBoxItem(swm));
         }
         HandleChangeItemEvent(ShopInventoryList, new OnChange("", "0"));
         ShopInventoryList.onValueChange = new EHandler<OnChange>(new Action<object, OnChange>(HandleChangeItemEvent));
      }

      #region UIMethods
      private void SetUIForWeaponShopItem(WeaponModel shopItem, int level)
      {
         ShopItemDescription.value = $"Description: {shopItem.ItemDescription}";
         ShopItemCurrentStat.value = $"Stat: {(shopItem.BaseDamage + shopItem.DamageIncrease * Math.Max(0, (level - 1)))} damage";
         ShopItemLevel.value = $"Level: {level.ToString()}";
         ShopItemPrice.value = $"Price: {(shopItem.BasePrice + shopItem.PriceIncrease * level).ToString()}";
         ShopItemImage.setImageReferences(shopItem.ImageReference);

         canPurchase = true;
         if (level >= 5)
         {
            Purchase.value = "Maxed";
            ShopItemPrice.value = "";
            canPurchase = false;
         }
         else if (level > 0)
         {
            Purchase.value = "Upgrade";
         }
         else
         {
            Purchase.value = "Buy";
         }

         if(canPurchase)
         {
            if(shopItem.BasePrice + shopItem.PriceIncrease * level > saveFile.money)
            {
               canPurchase = false;
            }
         }

         CurrentMoney.value = $"Money: {saveFile.money}";
      }

      private void SetUIForDojoShopItem(StatModel shopItem, int level)
      {
         ShopItemDescription.value = $"Description: {shopItem.ItemDescription}";
         ShopItemCurrentStat.value = $"Stat: {(shopItem.BaseValue + shopItem.ValueIncrease * level)}";
         ShopItemPrice.value = $"Price: {(shopItem.BasePrice + shopItem.PriceIncrease * level)}";
         ShopItemLevel.value = $"Level: {level}";
         ShopItemImage.setImageReferences(shopItem.ImageReference);

         canPurchase = true;
         if (level >= 5)
         {
            Purchase.value = "Maxed";
            ShopItemPrice.value = "";
            canPurchase = false;
         }
         else if (level > 0)
         {
            Purchase.value = "Upgrade";
         }
         else
         {
            Purchase.value = "Buy";
         }

         if (canPurchase)
         {
            if (shopItem.BasePrice + shopItem.PriceIncrease * level > saveFile.money)
            {
               canPurchase = false;
            }
         }

         CurrentMoney.value = $"Money: {saveFile.money}";
      }

      private void SetUIForTempleShopItem(AbilityModel shopItem, int level)
      {
         ShopItemDescription.value = $"Description: {shopItem.ItemDescription}";
         ShopItemCurrentStat.value = $"Stat: {(shopItem.BaseCooldown - shopItem.CooldownReduction * Math.Max(0, (level - 1))) / 100} seconds";
         ShopItemPrice.value = $"Price: {(shopItem.BasePrice + shopItem.PriceIncrease * level)}";
         ShopItemLevel.value = $"Level: {level}";
         ShopItemImage.setImageReferences(shopItem.ImageReference);

         canPurchase = true;
         if (level >= 5)
         {
            Purchase.value = "Maxed";
            ShopItemPrice.value = "";
            canPurchase = false;
         }
         else if (level > 0)
         {
            Purchase.value = "Upgrade";
         }
         else
         {
            Purchase.value = "Buy";
         }

         if (canPurchase)
         {
            if (shopItem.BasePrice + shopItem.PriceIncrease * level > saveFile.money)
            {
               canPurchase = false;
            }
         }

         CurrentMoney.value = $"Money: {saveFile.money}";
      }
      //TODO: add another field for secondary weapons to represent most important stat's base value laevel increase value
      private void SetUIForTrinketShopItem(SecendaryWeaponModel shopItem, int level)
      {
         ShopItemDescription.value = $"Description: {shopItem.ItemDescription}";
         ShopItemCurrentStat.value = $"Stat: {shopItem.Weight} weight";
         ShopItemPrice.value = $"Price: {(shopItem.BasePrice + shopItem.PriceIncrease * level)}";
         ShopItemLevel.value = $"Level: {level}";
         ShopItemImage.setImageReferences(shopItem.ImageReference);

         canPurchase = true;
         if (level >= 5)
         {
            Purchase.value = "Maxed";
            ShopItemPrice.value = "";
            canPurchase = false;
         }
         else if (level > 0)
         {
            Purchase.value = "Upgrade";
         }
         else
         {
            Purchase.value = "Buy";
         }

         if (canPurchase)
         {
            if (shopItem.BasePrice + shopItem.PriceIncrease * level > saveFile.money)
            {
               canPurchase = false;
            }
         }

         CurrentMoney.value = $"Money: {saveFile.money}";
      }
      #endregion

      #region clickMethods

      public void HandleChangeShopClick(object sender, OnClick click)
      {
         Button temp = (Button)sender;

         LoadBaseUI();

         if (temp.Name == "dojoSHop")
         {
            LoadDojoShop();
         }
         else if (temp.Name == "templeShop")
         {
            LoadTempleShop();
         }
         else if (temp.Name == "weaponShop")
         {
            LoadArmoryShop();
         }
         else if (temp.Name == "trinketShop")
         {
            LoadTrinketShop();
         }
      }

      public void HandleChangeItemEvent(object sender, OnChange change)
      {
         //TODO: add current numbers for everything
         ListBox listBox = (ListBox)sender;
         int ItemLevel = 0;
         float ItemValue = 0;

         if (currentShopType == shopTypeEnum.stat)
         {
            StatModel newSelectedStat = (StatModel)listBox.GetSelectedItem().Value;
            int index = -1;
            for (int i = 0; i < saveFile.Stats.Count; i++)
            {
               if (newSelectedStat.ItemName == saveFile.Stats[i].ItemName)
               {
                  index = i;
               }
            }

            if (index != -1)
            {
               ShopItemLevel.value = saveFile.Stats[index].Level.ToString();
               ShopItemDescription.value = saveFile.Stats[index].ItemDescription;
               ItemLevel = saveFile.Stats[index].Level;
            }
            else
            {
               ShopItemLevel.value = "0";
               ShopItemDescription.value = newSelectedStat.ItemDescription;
               ItemLevel = 0;
            }
            SetUIForDojoShopItem(newSelectedStat, ItemLevel);
         }
         else if (currentShopType == shopTypeEnum.ability)
         {
            AbilityModel newSelectedAbility = (AbilityModel)listBox.GetSelectedItem().Value;
            int index = -1;
            for (int i = 0; i < saveFile.Abilities.Count; i++)
            {
               if (newSelectedAbility.ItemName == saveFile.Abilities[i].ItemName)
               {
                  index = i;
               }
            }

            if (index != -1)
            {
               ShopItemLevel.value = saveFile.Abilities[index].Level.ToString();
               ShopItemDescription.value = saveFile.Abilities[index].ItemDescription;
               ItemLevel = saveFile.Abilities[index].Level;
            }
            else
            {
               ShopItemLevel.value = "0";
               ShopItemDescription.value = newSelectedAbility.ItemDescription;
               ItemLevel = 0;
            }
            SetUIForTempleShopItem(newSelectedAbility, ItemLevel);
         }
         else if (currentShopType == shopTypeEnum.weapon)
         {
            WeaponModel newSelectedWeapons = (WeaponModel)listBox.GetSelectedItem().Value;
            int index = -1;
            for (int i = 0; i < saveFile.Weapons.Count; i++)
            {
               if (newSelectedWeapons.ItemName == saveFile.Weapons[i].ItemName)
               {
                  index = i;
               }
            }
            if (index != -1)
            {
               ShopItemLevel.value = saveFile.Weapons[index].Level.ToString();
               ShopItemDescription.value = saveFile.Weapons[index].ItemDescription;
               ItemLevel = saveFile.Weapons[index].Level;
            }
            else
            {
               ShopItemLevel.value = "0";
               ShopItemDescription.value = newSelectedWeapons.ItemDescription;
               ItemLevel = 0;
            }
            SetUIForWeaponShopItem(newSelectedWeapons, ItemLevel);
         }
         else if (currentShopType == shopTypeEnum.trinket)
         {
            SecendaryWeaponModel newSelectedSecondaryWeapons = (SecendaryWeaponModel)listBox.GetSelectedItem().Value;
            int index = -1;
            for (int i = 0; i < saveFile.SecendaryWeapons.Count; i++)
            {
               if (newSelectedSecondaryWeapons.ItemName == saveFile.SecendaryWeapons[i].ItemName)
               {
                  index = i;
               }
            }
            if (index != -1)
            {
               ShopItemLevel.value = saveFile.SecendaryWeapons[index].Level.ToString();
               ShopItemDescription.value = saveFile.SecendaryWeapons[index].ItemDescription;
               ItemLevel = saveFile.SecendaryWeapons[index].Level;
            }
            else
            {
               ShopItemLevel.value = "0";
               ShopItemDescription.value = newSelectedSecondaryWeapons.ItemDescription;
               ItemLevel = 0;
            }
            SetUIForTrinketShopItem(newSelectedSecondaryWeapons, ItemLevel);
         }
      }

      public void HandlePurchaseClick(object sender, OnClick click)
      {
         if (currentShopType == shopTypeEnum.stat)
         {
            StatModel selectedItem = (StatModel)ShopInventoryList.GetSelectedItem().Value;

            //Find item is save file
            int index = -1;
            for (int i = 0; i < saveFile.Stats.Count; i++)
            {
               if (selectedItem.ItemName == saveFile.Stats[i].ItemName)
                  index = i;
            }

            int newLevel = 0;
            //update save file infomation if necessary
            if (index != -1)
            {
               int originalLevel = saveFile.Stats[index].Level;
               float cost = selectedItem.BasePrice + selectedItem.PriceIncrease * originalLevel;
               if (saveFile.money >= cost)
               {
                  saveFile.money -= (int)cost;
                  saveFile.Stats[index].Level++;
                  SetUIForDojoShopItem(selectedItem, saveFile.Stats[index].Level);
               }
               parentGUIBus.Publish(this, new ItemPurchesedEvent(saveFile.Stats[index], currentShopType));
            }
            else
            {
               if (saveFile.money >= selectedItem.BasePrice)
               {
                  saveFile.money -= (int)selectedItem.BasePrice;
                  newLevel = 1;
                  var newItem = new InventoryItemModel
                  {
                     ItemName = selectedItem.ItemName,
                     ImageReference = selectedItem.ImageReference,
                     ItemDescription = selectedItem.ItemDescription,
                     Level = 1
                  };
                  saveFile.Stats.Add(newItem);
                  SetUIForDojoShopItem(selectedItem, 1);
                  parentGUIBus.Publish(this, new ItemPurchesedEvent(newItem, currentShopType));
               }
            }

         }
         else if (currentShopType == shopTypeEnum.ability)
         {
            AbilityModel selectedItem = (AbilityModel)ShopInventoryList.GetSelectedItem().Value;

            //Find item is save file
            int index = -1;
            for (int i = 0; i < saveFile.Abilities.Count; i++)
            {
               if (selectedItem.ItemName == saveFile.Abilities[i].ItemName)
                  index = i;
            }

            int newLevel = 0;
            //update save file infomation if necessary
            if (index != -1)
            {
               int originalLevel = saveFile.Abilities[index].Level;
               float cost = selectedItem.BasePrice + selectedItem.PriceIncrease * originalLevel;
               if (saveFile.money >= cost)
               {
                  saveFile.money -= (int)cost;
                  saveFile.Abilities[index].Level++;
                  SetUIForTempleShopItem(selectedItem, saveFile.Abilities[index].Level);
                  parentGUIBus.Publish(this, new ItemPurchesedEvent(saveFile.Abilities[index], currentShopType));
               }
            }
            else
            {
               if (saveFile.money >= selectedItem.BasePrice)
               {
                  saveFile.money -= (int)selectedItem.BasePrice;
                  newLevel = 1;
                  var newItem = new InventoryItemModel
                  {
                     ItemName = selectedItem.ItemName,
                     ImageReference = selectedItem.ImageReference,
                     ItemDescription = selectedItem.ItemDescription,
                     Level = 1
                  };
                  saveFile.Abilities.Add(newItem);
                  SetUIForTempleShopItem(selectedItem, 1);
                  parentGUIBus.Publish(this, new ItemPurchesedEvent(newItem, currentShopType));
               }
            }
         }
         else if (currentShopType == shopTypeEnum.weapon)
         {
            WeaponModel selectedItem = (WeaponModel)ShopInventoryList.GetSelectedItem().Value;

            //Find item is save file
            int index = -1;
            for (int i = 0; i < saveFile.Weapons.Count; i++)
            {
               if (selectedItem.ItemName == saveFile.Weapons[i].ItemName)
                  index = i;
            }

            int newLevel = 0;
            //update save file infomation if necessary
            if (index != -1)
            {
               int originalLevel = saveFile.Weapons[index].Level;
               float cost = selectedItem.BasePrice + selectedItem.PriceIncrease * originalLevel;
               if (saveFile.money >= cost)
               {
                  saveFile.money -= (int)cost;
                  saveFile.Weapons[index].Level++;
                  SetUIForWeaponShopItem(selectedItem, saveFile.Weapons[index].Level);
                  parentGUIBus.Publish(this, new ItemPurchesedEvent(saveFile.Weapons[index], currentShopType));
               }
            }
            else
            {
               if (saveFile.money >= selectedItem.BasePrice)
               {
                  saveFile.money -= (int)selectedItem.BasePrice;
                  newLevel = 1;
                  var newItem = new InventoryItemModel
                  {
                     ItemName = selectedItem.ItemName,
                     ImageReference = selectedItem.ImageReference,
                     ItemDescription = selectedItem.ItemDescription,
                     Level = 1
                  };
                  saveFile.Weapons.Add(newItem);
                  SetUIForWeaponShopItem(selectedItem, 1);
                  parentGUIBus.Publish(this, new ItemPurchesedEvent(newItem, currentShopType));
               }
            }
         }
         else if (currentShopType == shopTypeEnum.trinket)
         {
            SecendaryWeaponModel selectedItem = (SecendaryWeaponModel)ShopInventoryList.GetSelectedItem().Value;

            //Find item is save file
            int index = -1;
            for (int i = 0; i < saveFile.SecendaryWeapons.Count; i++)
            {
               if (selectedItem.ItemName == saveFile.SecendaryWeapons[i].ItemName)
                  index = i;
            }

            int newLevel = 0;
            //update save file infomation if necessary
            if (index != -1)
            {
               int originalLevel = saveFile.SecendaryWeapons[index].Level;
               float cost = selectedItem.BasePrice + selectedItem.PriceIncrease * originalLevel;
               if (saveFile.money >= cost)
               {
                  saveFile.money -= (int)cost;
                  saveFile.SecendaryWeapons[index].Level++;
                  SetUIForTrinketShopItem(selectedItem, saveFile.SecendaryWeapons[index].Level);
                  parentGUIBus.Publish(this, new ItemPurchesedEvent(saveFile.SecendaryWeapons[index], currentShopType));
               }
            }
            else
            {
               if (saveFile.money >= selectedItem.BasePrice)
               {
                  saveFile.money -= (int)selectedItem.BasePrice;
                  newLevel = 1;
                  var newItem = new InventoryItemModel
                  {
                     ItemName = selectedItem.ItemName,
                     ImageReference = selectedItem.ImageReference,
                     ItemDescription = selectedItem.ItemDescription,
                     Level = 1
                  };
                  saveFile.SecendaryWeapons.Add(newItem);
                  SetUIForTrinketShopItem(selectedItem, 1);
                  parentGUIBus.Publish(this, new ItemPurchesedEvent(newItem, currentShopType));
               }
            }
         }

         SaveService.SaveSave();
      }

      public void HandleBackClick(object sender, OnClick click)
      {
         DoubleClick();
      }
      #endregion
   }
}
