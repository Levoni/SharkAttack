using Base.Events;
using Base.Scenes;
using Base.UI;
using Base.Utility;
using Base.Utility.Services;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestGame.Model;
using TestGame.Utility;
using TestGame.Utility.Services;

namespace TestGame.Components
{
   [Serializable]
   public class LoadOutSelectionUI:GUI
   {
      EngineRectangle viewport;
      Button currentClickedButton;

      Label lblWeaponsTitle;
      Label weaopnLabelOne;
      Label weaponLabelTwo;
      Button weaponImageOne;
      Button weaponImageTwo;
      Label lblAbilityTitle;
      Label abilityLabel;
      Button abilityImage;
      Label lblTrinketTitle;
      Label trinketLabel;
      Button trinketImage;

      ListBox lstBoxItemSelection;
      Button btnSelectedItemImage;
      Button btnSelectItem;
      Label lblSelectedItemName;
      Label lblSelectedItemDescription;

      PictureBox btnHighlightSelection;

      Button btnBack;
      Button btnConfirm;

      public bool FromCheckpoint { get; set; }

      public LoadOutSelectionUI():base()
      {
         if (ScreenGraphicService.checkInitialized())
         {
            this.viewport = ScreenGraphicService.GetViewportBounds();
         }
         else
         {
            this.viewport = new EngineRectangle();
         }
      }

      public override void UnInitialize()
      {
         base.UnInitialize();
      }

      public override void Init()
      {
         base.Init();
      }

      public override void Init(EventBus ebus, Scene parentScene)
      {
         LoadMainScreen();
         base.Init(ebus, parentScene);
      }

      public override void Update(int dt)
      {
         base.Update(dt);
         lblWeaponsTitle.Update(dt);
         weaponImageOne.Update(dt);
         weaponImageTwo.Update(dt);
         lblAbilityTitle.Update(dt);
         abilityImage.Update(dt);
         lblTrinketTitle.Update(dt);
         trinketImage.Update(dt);
         lstBoxItemSelection.Update(dt);
         btnSelectedItemImage.Update(dt);
         lblSelectedItemName.Update(dt);
         lblSelectedItemDescription.Update(dt);
         btnBack.Update(dt);
         btnConfirm.Update(dt);
         btnSelectItem.Update(dt);
         btnHighlightSelection.Update(dt);
      }

      public override void Render(SpriteBatch sb)
      {
         base.Render(sb);
         lblWeaponsTitle.Render(sb);
         weaponImageOne.Render(sb);
         weaponImageTwo.Render(sb);
         lblAbilityTitle.Render(sb);
         abilityImage.Render(sb);
         lblTrinketTitle.Render(sb);
         trinketImage.Render(sb);
         lstBoxItemSelection.Render(sb);
         btnSelectedItemImage.Render(sb);
         lblSelectedItemName.Render(sb);
         lblSelectedItemDescription.Render(sb);
         btnBack.Render(sb);
         btnConfirm.Render(sb);
         btnSelectItem.Render(sb);
         btnHighlightSelection.Render(sb);
      }

      public void LoadMainScreen()
      {
         EngineRectangle[] quarters = new EngineRectangle[4];
         quarters[0] = new EngineRectangle(viewport.X,viewport.Y,viewport.Width / 2,viewport.Height / 2);
         quarters[1] = new EngineRectangle(viewport.Width / 2, viewport.Y, viewport.Width / 2, viewport.Height / 2);
         quarters[2] = new EngineRectangle(viewport.X, viewport.Height / 2, viewport.Width / 2, viewport.Height / 2);
         quarters[3] = new EngineRectangle(viewport.Width / 2, viewport.Height / 2, viewport.Width / 2, viewport.Height / 2);

         //SelectorHighlight
         btnHighlightSelection = new PictureBox("btnHighlight", new EngineRectangle(),Color.White,"none");

         //Weapon section
         lblWeaponsTitle = new Label("weaponsTitle", "Weapons", new EngineRectangle(quarters[0].Width / 4, quarters[0].Height / 8, quarters[0].Width / 2, quarters[0].Height / 4), Color.White);
         lblWeaponsTitle.textAnchor = Enums.TextAchorLocation.center;

         weaponImageOne = new Button("weaponOne", "", new EngineRectangle(quarters[0].Width / 4 - quarters[0].Width / 16, quarters[0].Height / 2, quarters[0].Width / 8, quarters[0].Height/4), Color.White);
         weaponImageOne.onClick = new EHandler<OnClick>(new Action<object, OnClick>(HandleLoadoutClick));
         weaponImageOne.setImageReferences("outline", "outline_hover", "outline_hover", "outline");
         weaponImageTwo = new Button("weaponTwo", "", new EngineRectangle(quarters[0].Width / 4 * 3 - quarters[0].Width / 16, quarters[0].Height / 2, quarters[0].Width / 8, quarters[0].Height / 4), Color.White);
         weaponImageTwo.onClick = new EHandler<OnClick>(new Action<object, OnClick>(HandleLoadoutClick));
         weaponImageTwo.setImageReferences("outline", "outline_hover", "outline_hover", "outline");

         if (LoadoutService.Loadout.FirstWeapon != null)
         {
            weaponImageOne.setImageReferences(LoadoutService.Loadout.FirstWeapon.ImageReference);
            //weaponImageOne.value = LoadoutService.Loadout.FirstWeapon.ItemName;
         }
         if (LoadoutService.Loadout.SecondWeapon != null)
         {
            weaponImageTwo.setImageReferences(LoadoutService.Loadout.SecondWeapon.ImageReference);
            //weaponImageTwo.value = LoadoutService.Loadout.SecondWeapon.ItemName;
         }

         //Ability section
         lblAbilityTitle = new Label("weaponsTitle", "Ability", new EngineRectangle(quarters[1].X + quarters[1].Width / 4, quarters[1].Height / 4, quarters[1].Width / 2, quarters[1].Height / 8), Color.White);
         lblAbilityTitle.textAnchor = Enums.TextAchorLocation.center;
         abilityImage = new Button("ability", "", new EngineRectangle(quarters[1].X + quarters[1].Width / 2 - quarters[1].Width / 16, quarters[1].Height / 2, quarters[1].Width / 8, quarters[1].Height / 4), Color.White);
         abilityImage.onClick = new EHandler<OnClick>(new Action<object, OnClick>(HandleLoadoutClick));
         abilityImage.setImageReferences("outline", "outline_hover", "outline_hover", "outline");

         if (LoadoutService.Loadout.Ability != null)
         {
            abilityImage.setImageReferences(LoadoutService.Loadout.Ability.ImageReference);
            //abilityImage.value = LoadoutService.Loadout.Ability.ItemName;
         }

         //Trinket section
         lblTrinketTitle = new Label("weaponsTitle", "Trinket", new EngineRectangle(quarters[2].Width / 4, quarters[2].Y + quarters[2].Height / 4, quarters[2].Width / 2, quarters[2].Height / 8), Color.White);
         lblTrinketTitle.textAnchor = Enums.TextAchorLocation.center;
         trinketImage = new Button("trinket", "", new EngineRectangle(quarters[2].Width / 2 - quarters[2].Width / 16, quarters[2].Y +  quarters[2].Height / 2, quarters[2].Width / 8, quarters[2].Height / 4), Color.White);
         trinketImage.onClick = new EHandler<OnClick>(new Action<object, OnClick>(HandleLoadoutClick));
         trinketImage.setImageReferences("outline", "outline_hover", "outline_hover", "outline");

         if (LoadoutService.Loadout.Trinket != null)
         {
            trinketImage.setImageReferences(LoadoutService.Loadout.Trinket.ImageReference);
            //trinketImage.value = LoadoutService.Loadout.Trinket.ItemName;
         }

         //Selection section
         lstBoxItemSelection = new ListBox("lstboxSelection","",new EngineRectangle(quarters[3].X + quarters[3].Width / 4,quarters[3].Y + quarters[3].Height / 4, quarters[3].Width / 4, quarters[3].Height / 4 * 3 - 50),Color.Black);
         lstBoxItemSelection.itemBaseHeight = (int)lstBoxItemSelection.bounds.Height / 5;
         lstBoxItemSelection.minFontScale = 0;
         lstBoxItemSelection.maxFontScale = .5f;
         lstBoxItemSelection.onValueChange = new EHandler<OnChange>(new Action<object, OnChange>(HandleItemChange));
         btnSelectItem = new Button("selectedItemImage","Select",new EngineRectangle(quarters[3].X + quarters[3].Width / 4 * 3, quarters[3].Y + quarters[3].Height / 4, quarters[3].Width / 4, quarters[3].Height / 4), Color.White);
         btnSelectItem.onClick = new EHandler<OnClick>(new Action<object, OnClick>(HandleSelectButtonClick));
         btnSelectedItemImage = new Button("btnImage","",new EngineRectangle(quarters[3].X + quarters[3].Width / 4 * 2 + 50, quarters[3].Y + quarters[3].Height / 4, quarters[3].Height / 4, quarters[3].Height / 4),Color.White);
         lblSelectedItemName = new Label("selectedItemName","Name: ", new EngineRectangle(quarters[3].X + quarters[3].Width / 4 * 2 + 50, quarters[3].Y + quarters[3].Height / 4 * 2, quarters[3].Width / 2 - 100, quarters[3].Height / 8), Color.White);
         lblSelectedItemName.minFontScale = 0f;
         lblSelectedItemDescription = new Label("selectedItemDescription","Description: ", new EngineRectangle(quarters[3].X + quarters[3].Width / 4 * 2 + 50, quarters[3].Y + quarters[3].Height / 8 * 5, quarters[3].Width / 2 - 100, quarters[3].Height / 8 * 3 - 50), Color.White);
         lblSelectedItemDescription.minFontScale = 0f;
         lblSelectedItemDescription.isMultiLine = true;

         btnBack = new Button("back","Back",new EngineRectangle(0, 0, viewport.Width / 8, viewport.Width / 32), Color.White);
         btnBack.onClick = new EHandler<OnClick>(new Action<object, OnClick>(handleBackButtonClick));
         btnConfirm = new Button("Confirm","Confirm",new EngineRectangle(viewport.Width / 2 - 100, viewport.Height - 100, 200,50),Color.White);
         btnConfirm.onClick = new EHandler<OnClick>(new Action<object, OnClick>(HandleConfirmButtonClick));
         btnConfirm.minFontScale = 0f;

      }


      private void HandleLoadoutClick(object sender, OnClick click)
      {
         Button sendingButton = (Button)sender;
         TestGameSaveFile save = (TestGameSaveFile)SaveService.Save;

         btnHighlightSelection.bounds = sendingButton.bounds;
         btnHighlightSelection.setImageReferences("highlight");

         if(sendingButton.Name == "weaponOne")
         {
            lstBoxItemSelection.Items.Clear();
            foreach(InventoryItemModel wm in save.Weapons)
            {
               lstBoxItemSelection.AddItem(new ListBoxItem(wm));
            }
            if(save.Weapons.Count != 0)
            {
               lstBoxItemSelection.selectedIndex = 0;
               HandleItemChange(lstBoxItemSelection, new OnChange(0,0));
            }
         }
         else if (sendingButton.Name == "weaponTwo")
         {
            lstBoxItemSelection.Items.Clear();
            foreach (InventoryItemModel wm in save.Weapons)
            {
               lstBoxItemSelection.AddItem(new ListBoxItem(wm));
            }
            if (save.Weapons.Count != 0)
            {
               lstBoxItemSelection.selectedIndex = 0;
               HandleItemChange(lstBoxItemSelection, new OnChange(0, 0));
            }
         }
         else if (sendingButton.Name == "ability")
         {
            lstBoxItemSelection.Items.Clear();
            foreach (InventoryItemModel a in save.Abilities)
            {
               lstBoxItemSelection.AddItem(new ListBoxItem(a));
            }
            if (save.Abilities.Count != 0)
            {
               lstBoxItemSelection.selectedIndex = 0;
               HandleItemChange(lstBoxItemSelection, new OnChange(0, 0));
            }
         }
         else if (sendingButton.Name == "trinket")
         {
            lstBoxItemSelection.Items.Clear();
            foreach (InventoryItemModel sw in save.SecendaryWeapons)
            {
               lstBoxItemSelection.AddItem(new ListBoxItem(sw));
            }
            if (save.SecendaryWeapons.Count != 0)
            {
               lstBoxItemSelection.selectedIndex = 0;
               HandleItemChange(lstBoxItemSelection, new OnChange(0, 0));
            }
         }

         currentClickedButton = sendingButton;
      }

      private void HandleItemChange(object sender, OnChange change)
      {
         ListBoxItem item = (ListBoxItem)((ListBox)sender).GetSelectedItem();
         if(item != null)
         {
            lblSelectedItemName.value = $"Name: {((InventoryItemModel)item.Value).ItemName}";
            lblSelectedItemDescription.value = $"Description: {((InventoryItemModel)item.Value).ItemDescription}";
            
            btnSelectedItemImage.setImageReferences(((InventoryItemModel)item.Value).ImageReference);
         }
         
      }

      private void HandleSelectButtonClick(object sender, OnClick click)
      {
         ListBoxItem item = (ListBoxItem)(lstBoxItemSelection).GetSelectedItem();
         if (item != null)
         {
            InventoryItemModel inventoryItem = (InventoryItemModel)item.Value;
            int index = -1;
            for(int i = 0; i < ShopService.Weapons.Count; i++)
            {
               if(inventoryItem.ItemName == ShopService.Weapons[i].ItemName)
               {
                  if (currentClickedButton.Name == "weaponOne")
                  {
                     LoadoutService.Loadout.FirstWeapon = ShopService.Weapons[i];
                     currentClickedButton.setImageReferences(ShopService.Weapons[i].ImageReference);
                     currentClickedButton.value = ""; //ShopService.Weapons[i].ItemName;
                  }
                  else
                  {
                     LoadoutService.Loadout.SecondWeapon = ShopService.Weapons[i];
                     currentClickedButton.setImageReferences(ShopService.Weapons[i].ImageReference);
                     currentClickedButton.value = ""; // ShopService.Weapons[i].ItemName;
                  }
               }
            }
            for (int i = 0; i < ShopService.Abilities.Count; i++)
            {
               if (inventoryItem.ItemName == ShopService.Abilities[i].ItemName)
               {
                  LoadoutService.Loadout.Ability = ShopService.Abilities[i];
                  currentClickedButton.setImageReferences(ShopService.Abilities[i].ImageReference);
                  currentClickedButton.value = ""; // ShopService.Abilities[i].ItemName;
               }
            }
            for (int i = 0; i < ShopService.SecedaryWeapons.Count; i++)
            {
               if (inventoryItem.ItemName == ShopService.SecedaryWeapons[i].ItemName)
               {
                  LoadoutService.Loadout.Trinket = ShopService.SecedaryWeapons[i];
                  currentClickedButton.setImageReferences(ShopService.SecedaryWeapons[i].ImageReference);
                  currentClickedButton.value = ""; // ShopService.SecedaryWeapons[i].ItemName;
               }
            }
         }
      }

      private void HandleConfirmButtonClick(object sender, OnClick click)
      {
         //TODO: Reset Loadout to the items since they might be outdated if file is changed
         if (LoadoutService.Loadout.FirstWeapon != null || LoadoutService.Loadout.FirstWeapon != null)
         {
            LoadoutService.SaveLoadout();
            parentScene.parentWorld.RemoveScene(parentScene);
            parentScene.parentWorld.AddScene(Factories.SceneFactory.SetupMainScene(FromCheckpoint));
         }
         else
         {
            btnConfirm.value = "Weapon Required";
         } 
      }

      private void handleBackButtonClick(object sender, OnClick click)
      {
         LoadoutService.SaveLoadout();
         parentScene.parentWorld.RemoveScene(parentScene);
         parentScene.parentWorld.AddScene(Factories.SceneFactory.SetupMenuScene(viewport));
      }
   }
}
