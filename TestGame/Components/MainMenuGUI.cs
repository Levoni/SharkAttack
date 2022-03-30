using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Base.Scenes;
using Base.UI;
using Base.Utility;
using Base.Utility.Services;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TestGame.Utility;
using TestGame.Utility.Services;
using TestGame.UIComponents;
using TestGame.Model;

namespace TestGame.Components
{
   [Serializable]
   class MainMenuGUI : GUI
   {
      string curMenu;
      EngineRectangle viewport;

      //Main Menu
      Button StartGame;
      Button StartGameCheckpoint;
      Label currentCheckpoint;
      Button HighScores;
      Button Options;
      Button achievments;
      Button Shop;
      Button Credits;
      Button Exit;
      Label Title;

      //High Score menu
      ListBox highscoreList;
      Label highScoreLabel;

      //Options Menu
      Base.UI.Textbox backgroundVolumeTextbox;
      Label backgroundVolumeLabel;
      Label spriteSetLabel;
      ListBox spriteSetList;
      //Base.UI.Textbox SFXVolumeTextbox;
      //Label SFXVolumeLabel;
      Label spriteSetDescripiton;
      Label lblControls;
      List<ControlSelector> ControlSelectorList;
      Label lblGoldSkin;
      CheckBox cbxGoldSkin;

      //Credits Menu
      Label ArtTitle;
      Label ProgrammingDesign;
      Label ArtCreditOne;
      Label ArtCreditTwo;
      Label programmingCredit;

      //Achievment Menu
      Label AchievmentTitle;
      ListBox AchievmentListBox;
      Label ShotsFired;
      Label EnemiesKilled;
      Label TotalDamageTaken;
      Label TotalDamageHealed;
      Label MaxWaveWithoutDamage;

      //General
      Button backButton;

      //Upgrade Screen
      UpgradeMenu upgradeMenu;

      public MainMenuGUI()
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

      public override void Update(int dt)
      {
         if (curMenu == "Main Menu")
            updateMainMenu(dt);
         else if (curMenu == "High Scores")
            updateHighScoresMenu(dt);
         else if (curMenu == "Options")
            updateOptionsMenu(dt);
         else if (curMenu == "Credits")
            updateCreditMenu(dt);
         else if (curMenu == "Upgrade")
            UpdateUpgradeMenu(dt);
         else if (curMenu == "Achievments")
            UpdateAchievmentMenu(dt);
      }

      public override void Render(SpriteBatch sb)
      {
         if (curMenu == "Main Menu")
            renderMainMenu(sb);
         else if (curMenu == "High Scores")
            renderHighScoresMenu(sb);
         else if (curMenu == "Options")
            renderOptionsMenu(sb);
         else if (curMenu == "Credits")
            renderCreditMenu(sb);
         else if (curMenu == "Upgrade")
            RenderUpgradeMenu(sb);
         else if (curMenu == "Achievments")
            RenderAchievmentMenu(sb);

      }

      public override void Init(Base.Events.EventBus eb, Scene parentScene)
      {
         LoadMainMenu();
         base.Init(eb, parentScene);
      }

      public void updateMainMenu(int dt)
      {
         StartGame.Update(dt);
         StartGameCheckpoint.Update(dt);
         currentCheckpoint.Update(dt);
         HighScores.Update(dt);
         Options.Update(dt);
         Credits.Update(dt);
         Shop.Update(dt);
         Exit.Update(dt);
         Title.Update(dt);
         achievments.Update(dt);
      }

      public void updateHighScoresMenu(int dt)
      {
         highscoreList.Update(dt);
         highScoreLabel.Update(dt);
         backButton.Update(dt);
      }

      public void updateOptionsMenu(int dt)
      {
         backgroundVolumeTextbox.Update(dt);
         backgroundVolumeLabel.Update(dt);
         spriteSetLabel.Update(dt);
         spriteSetList.Update(dt);
         //SFXVolumeTextbox.Update();
         //SFXVolumeLabel.Update();
         backButton.Update(dt);
         spriteSetDescripiton.Update(dt);
         lblControls.Update(dt);
         foreach (Control c in ControlSelectorList)
         {
            c.Update(dt);
         }
         cbxGoldSkin.Update(dt);
         lblGoldSkin.Update(dt);
      }

      public void updateCreditMenu(int dt)
      {
         ArtTitle.Update(dt);
         ProgrammingDesign.Update(dt);
         ArtCreditOne.Update(dt);
         ArtCreditTwo.Update(dt);
         programmingCredit.Update(dt);
         backButton.Update(dt);
      }

      public void UpdateUpgradeMenu(int dt)
      {
         upgradeMenu.Update(dt);
      }

      public void UpdateAchievmentMenu(int dt)
      {
         AchievmentTitle.Update(dt);
         AchievmentListBox.Update(dt);
         ShotsFired.Update(dt);
         EnemiesKilled.Update(dt);
         TotalDamageTaken.Update(dt);
         backButton.Update(dt);
         TotalDamageHealed.Update(dt);
         MaxWaveWithoutDamage.Update(dt);
      }

      public void renderMainMenu(SpriteBatch sb)
      {
         StartGame.Render(sb);
         StartGameCheckpoint.Render(sb);
         currentCheckpoint.Render(sb);
         HighScores.Render(sb);
         Options.Render(sb);
         Credits.Render(sb);
         Shop.Render(sb);
         Exit.Render(sb);
         Title.Render(sb);
         achievments.Render(sb);
      }

      public void renderHighScoresMenu(SpriteBatch sb)
      {
         highscoreList.Render(sb);
         highScoreLabel.Render(sb);
         backButton.Render(sb);
      }

      public void renderOptionsMenu(SpriteBatch sb)
      {
         backgroundVolumeTextbox.Render(sb);
         backgroundVolumeLabel.Render(sb);
         spriteSetLabel.Render(sb);
         spriteSetList.Render(sb);
         //SFXVolumeTextbox.Render(sb);
         //SFXVolumeLabel.Render(sb);
         backButton.Render(sb);
         spriteSetDescripiton.Render(sb);
         lblControls.Render(sb);
         foreach (Control c in ControlSelectorList)
         {
            c.Render(sb);
         }
         cbxGoldSkin.Render(sb);
         lblGoldSkin.Render(sb);
      }

      public void renderCreditMenu(SpriteBatch sb)
      {
         ArtTitle.Render(sb);
         ProgrammingDesign.Render(sb);
         ArtCreditOne.Render(sb);
         ArtCreditTwo.Render(sb);
         programmingCredit.Render(sb);
         backButton.Render(sb);
      }

      public void RenderUpgradeMenu(SpriteBatch sb)
      {
         upgradeMenu.Render(sb);
      }

      public void RenderAchievmentMenu(SpriteBatch sb)
      {
         AchievmentTitle.Render(sb);
         AchievmentListBox.Render(sb);
         ShotsFired.Render(sb);
         EnemiesKilled.Render(sb);
         TotalDamageTaken.Render(sb);
         backButton.Render(sb);
         TotalDamageHealed.Render(sb);
         MaxWaveWithoutDamage.Render(sb);
      }

      public void LoadMainMenu()
      {
         curMenu = "Main Menu";

         Title = new Label("lblTitle", "Shark Attack", new EngineRectangle(viewport.Width / 2 - 200, (viewport.Height * .125f) - 50, 400, 100), Color.White);
         Title.minFontScale = 0f;

         StartGame = new Button("btnStart", "Start Game", new EngineRectangle(viewport.Width / 2 - 100, (viewport.Height * .25f) - 25, 200, 50), Color.White);
         StartGame.onClick = new Base.Events.EHandler<Base.Events.OnClick>(StartGameClick);
         StartGame.minFontScale = 0;
         StartGame.padding = new int[]
         {
            0,10,0,10
         };

         if (((TestGameSaveFile)SaveService.Save).CheckpointReached > 1)
         {
            StartGameCheckpoint = new Button("btnContinue", "Continue from checkpoint", new EngineRectangle(viewport.Width / 4 * 3 - 150, (viewport.Height * .25f) - 25, 300, 50), Color.White);
            StartGameCheckpoint.onClick = new Base.Events.EHandler<Base.Events.OnClick>(StartGameFromCheckpointClick);
            StartGameCheckpoint.minFontScale = 0;
            StartGameCheckpoint.padding = new int[]
            {
            0,10,0,10
            };

            currentCheckpoint = new Label("lblCurCheckpoint", "Wave: " + ((TestGameSaveFile)SaveService.Save).CheckpointReached.ToString(), new EngineRectangle(viewport.Width / 4 * 3 - 150, (viewport.Height * .25f) + 50, 300, 50), Color.White);
            currentCheckpoint.textAnchor = Enums.TextAchorLocation.center;

            StartGame.bounds = new EngineRectangle(viewport.Width / 4 - 150, (viewport.Height * .25f) - 25, 300, 50);
         }
         else
         {
            StartGameCheckpoint = new Button("btnContinue", "", new EngineRectangle(), Color.White);
            currentCheckpoint = new Label("lblCurCheckpoint", "", new EngineRectangle(), Color.White);
         }

         HighScores = new Button("btnHighScores", "High Scores", new EngineRectangle(viewport.Width / 2 - 100, (viewport.Height * .37f) - 25, 200, 50), Color.White);
         HighScores.onClick = new Base.Events.EHandler<Base.Events.OnClick>(HighScoreClick);
         HighScores.minFontScale = 0;
         HighScores.padding = new int[]
         {
            0,10,0,10
         };
         Shop = new Button("btnOptions", "Upgrade Shop", new EngineRectangle(viewport.Width / 2 - 100, (viewport.Height * .49f) - 25, 200, 50), Color.White);
         Shop.onClick = new Base.Events.EHandler<Base.Events.OnClick>(UpgradeClick);
         Shop.minFontScale = 0;
         Shop.padding = new int[]
         {
            0,10,0,10
         };
         Options = new Button("btnOptions", "Options", new EngineRectangle(viewport.Width / 2 - 100, (viewport.Height * .61f) - 25, 200, 50), Color.White);
         Options.onClick = new Base.Events.EHandler<Base.Events.OnClick>(OptionClick);
         Options.minFontScale = 0;
         Options.padding = new int[]
         {
            0,10,0,10
         };
         achievments = new Button("btnAchievments", "Achievments", new EngineRectangle(viewport.Width / 2 - 100, (viewport.Height * .73f) - 25, 200, 50), Color.White);
         achievments.onClick = new Base.Events.EHandler<Base.Events.OnClick>(AchievmentsClick); ;
         achievments.minFontScale = 0;
         achievments.padding = new int[]
         {
            0,10,0,10
         }; 
         Credits = new Button("btnCredits", "Credits", new EngineRectangle(50, (viewport.Height) - 100, 200, 50), Color.White);
         Credits.onClick = new Base.Events.EHandler<Base.Events.OnClick>(CreditsClick);
         Credits.minFontScale = 0;
         Exit = new Button("btnExit", "Exit", new EngineRectangle(0, 0, viewport.Width / 8, viewport.Width / 32), Color.White);
         Exit.onClick = new Base.Events.EHandler<Base.Events.OnClick>(ExitClick);
         Exit.minFontScale = 0;
         Exit.maxFontScale = 1f;


      }

      public void LoadHighScoreMenu()
      {
         highscoreList = new ListBox("lstHighScore", "", new EngineRectangle(viewport.Width / 4, viewport.Height / 5, viewport.Width / 2, viewport.Height / 5 * 3), Color.Black);
         highscoreList.minFontScale = 0;
         highscoreList.maxFontScale = 1;
         PointsService.LoadHighScores();
         int i = 0;
         foreach (Score obj in PointsService.GetHighScores())
         {
            i++;
            highscoreList.AddItem(new ListBoxItem($"{i.ToString()}. Name: {obj.name}, Wave: {obj.wave}, Score: {obj.score}"));
         }
         highscoreList.itemBaseHeight = 100;

         highScoreLabel = new Label("lblHighScore", "High Scores", new EngineRectangle(viewport.Width / 4 + 10, viewport.Height / 10, 200, viewport.Height / 10), Color.White);
         highScoreLabel.textAnchor = Enums.TextAchorLocation.centerLeft;
         highScoreLabel.minFontScale = 0;

         backButton = new Button("btnBack", "Back", new EngineRectangle(0, 0, viewport.Width / 8, viewport.Width / 32), Color.White);
         backButton.onClick = new Base.Events.EHandler<Base.Events.OnClick>(new Action<object, Base.Events.OnClick>(backClick));
         backButton.minFontScale = 0;
         backButton.maxFontScale = 1f;

         curMenu = "High Scores";
      }

      public void LoadOptionMenu()
      {
         //SFXVolumeTextbox = new Textbox("txtboxVolume", "0", new EngineRectangle(viewport.Width / 2 - 100, viewport.Height / 3 - 50, 200, 100), Color.Black, 3, new HashSet<char> { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' });
         //SFXVolumeTextbox.value = ((int)(AudioService.GetSFXVolume() * 100)).ToString();
         //SFXVolumeTextbox.isEditable = true;
         //SFXVolumeTextbox.onValueChange = new Base.Events.EHandler<Base.Events.OnChange>(new Action<object, Base.Events.OnChange>(SFXVolumeTextUpdate));

         //SFXVolumeLabel = new Label("lblSFXVolume", "SFX Volume: 0 - 100", new EngineRectangle(viewport.Width / 2 - 100, viewport.Height / 3 - 150, 200, 100), Color.White);

         spriteSetLabel = new Label("lblSpriteSet", "Sprite Set", new EngineRectangle(viewport.Width / 2 + 50, viewport.Height / 3 - 150, viewport.Width / 4 - 100, 100), Color.White);
         spriteSetLabel.minFontScale = 0;


         spriteSetList = new ListBox("lstboxSpriteSet", "", new EngineRectangle(viewport.Width / 2 + 50, viewport.Height / 3 - 50, viewport.Width / 4 - 100, 100), Color.Black);
         spriteSetList.minFontScale = 0;
         spriteSetList.maxFontScale = 1;
         spriteSetList.AddItem(new ListBoxItem("Donuts and Sharks"));
         spriteSetList.AddItem(new ListBoxItem("Geometry rumble"));
         if (ConfigService.config.SpriteSet == "Donuts and Sharks")
            spriteSetList.selectedIndex = 0;
         else
            spriteSetList.selectedIndex = 1;
         spriteSetList.itemBaseHeight = 25;
         spriteSetList.onValueChange = new Base.Events.EHandler<Base.Events.OnChange>(SpriteSetChange);

         string spriteDescription;
         if (ConfigService.config.SpriteSet == "Donuts and Sharks")
            spriteDescription = "One lonely donut must take on the ocean by itself. How long can its sprinkles last?";
         else
            spriteDescription = "No shape is safe. Defend yourself!";
         spriteSetDescripiton = new Label("lblSpriteSetDescription", spriteDescription, new EngineRectangle(viewport.Width / 4 * 3 + 50, viewport.Height / 3 - 50, viewport.Width / 4 - 100, 200), Color.White);
         spriteSetDescripiton.isMultiLine = true;
         spriteSetDescripiton.maxFontScale = .5f;

         backgroundVolumeTextbox = new Textbox("txtboxVolume", "0", new EngineRectangle(viewport.Width / 2 + 50, (viewport.Height / 3) * 2 - 50, viewport.Width / 4 - 100, 100), Color.Black, 3, new HashSet<char> { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' });
         backgroundVolumeTextbox.value = ((int)(AudioService.GetBackgroundVolume() * 100)).ToString();
         backgroundVolumeTextbox.minFontScale = 0;
         backgroundVolumeTextbox.isEditable = true;
         backgroundVolumeTextbox.onValueChange = new Base.Events.EHandler<Base.Events.OnChange>(new Action<object, Base.Events.OnChange>(BackgroundVolumeTextUpdate));

         backgroundVolumeLabel = new Label("lblBackgroundVolume", "Background Volume: 0 - 100", new EngineRectangle(viewport.Width / 2 + 50, (viewport.Height / 3) * 2 - 150, viewport.Width / 2 - 100, 100), Color.White);
         backgroundVolumeLabel.minFontScale = 0;

         backButton = new Button("btnBack", "Back", new EngineRectangle(0, 0, viewport.Width / 8, viewport.Width / 32), Color.White);
         backButton.onClick = new Base.Events.EHandler<Base.Events.OnClick>(new Action<object, Base.Events.OnClick>(backClick));
         backButton.minFontScale = 0;
         backButton.maxFontScale = 1f;

         lblControls = new Label("lblControl", "Controls:", new EngineRectangle(50, (viewport.Height - 100) / 10 * 1, viewport.Width / 2 - 100, (viewport.Height - 100) / 10), Color.White);

         ControlSelectorList = new List<ControlSelector>();
         ControlSelectorList.Add(new ControlSelector("", "", new EngineRectangle(50, (viewport.Height - 100) / 10 * 2, viewport.Width / 2 - 100, (viewport.Height - 100) / 10), Color.White, Enums.ControlType.attack1, "Shoot", parentScene));
         ControlSelectorList.Add(new ControlSelector("", "", new EngineRectangle(50, (viewport.Height - 100) / 10 * 3, viewport.Width / 2 - 100, (viewport.Height - 100) / 10), Color.White, Enums.ControlType.attack2, "Use Trinket", parentScene));
         ControlSelectorList.Add(new ControlSelector("", "", new EngineRectangle(50, (viewport.Height - 100) / 10 * 4, viewport.Width / 2 - 100, (viewport.Height - 100) / 10), Color.White, Enums.ControlType.modifier2, "Change Weapon", parentScene));
         ControlSelectorList.Add(new ControlSelector("", "", new EngineRectangle(50, (viewport.Height - 100) / 10 * 5, viewport.Width / 2 - 100, (viewport.Height - 100) / 10), Color.White, Enums.ControlType.ability1, "Use Ability", parentScene));
         ControlSelectorList.Add(new ControlSelector("", "", new EngineRectangle(50, (viewport.Height - 100) / 10 * 6, viewport.Width / 2 - 100, (viewport.Height - 100) / 10), Color.White, Enums.ControlType.up, "Move Up", parentScene));
         ControlSelectorList.Add(new ControlSelector("", "", new EngineRectangle(50, (viewport.Height - 100) / 10 * 7, viewport.Width / 2 - 100, (viewport.Height - 100) / 10), Color.White, Enums.ControlType.down, "Move Down", parentScene));
         ControlSelectorList.Add(new ControlSelector("", "", new EngineRectangle(50, (viewport.Height - 100) / 10 * 8, viewport.Width / 2 - 100, (viewport.Height - 100) / 10), Color.White, Enums.ControlType.left, "Move Left", parentScene));
         ControlSelectorList.Add(new ControlSelector("", "", new EngineRectangle(50, (viewport.Height - 100) / 10 * 9, viewport.Width / 2 - 100, (viewport.Height - 100) / 10), Color.White, Enums.ControlType.right, "Move Right", parentScene));
         ControlSelectorList.Add(new ControlSelector("", "", new EngineRectangle(50, (viewport.Height - 100) / 10 * 10, viewport.Width / 2 - 100, (viewport.Height - 100) / 10), Color.White, Enums.ControlType.modifier1, "Trigger Bomb", parentScene));

         if (((TestGameSaveFile)SaveService.Save).BeatGame)
         {
            lblGoldSkin = new Label("lblGoldSkin", "Gold Skin:", new EngineRectangle(viewport.Width / 2 + 50, (viewport.Height / 3) * 3 - 150, viewport.Width / 8, 100), Color.White);
            lblGoldSkin.minFontScale = 0f;
            lblGoldSkin.maxFontScale = 1f;
            lblGoldSkin.textAnchor = Enums.TextAchorLocation.centerLeft;
            cbxGoldSkin = new CheckBox("cbxSkin", ConfigService.config.GoldenSkin, new EngineRectangle(viewport.Width / 8 * 5 + 50, (viewport.Height / 3) * 3 - 150, 100, 100));
            cbxGoldSkin.onClick = new Base.Events.EHandler<Base.Events.OnClick>(new Action<object, Base.Events.OnClick>(checkboxClick));
         }
         else
         {
            lblGoldSkin = new Label("lblGoldSkin", "", new EngineRectangle(), Color.White);
            cbxGoldSkin = new CheckBox("cbxSkin", ConfigService.config.GoldenSkin, new EngineRectangle());
         }

         curMenu = "Options";
      }

      public void LoadCreditMenu()
      {
         ArtTitle = new Label("lblArtTitle", "Art", new EngineRectangle(viewport.Width / 2 - 100, viewport.Height / 4, 200, 50), Color.White);
         ArtTitle.minFontScale = ArtTitle.maxFontScale = 1;
         ArtTitle.textAnchor = Enums.TextAchorLocation.center;

         ArtCreditOne = new Label("lblArtCreditOne", "Sashia Swenson (Donuts and Sharks)", new EngineRectangle(viewport.Width / 2 - 100, viewport.Height / 4 + 50, 200, 50), Color.White);
         ArtCreditOne.minFontScale = ArtCreditOne.maxFontScale = .5f;
         ArtCreditOne.textAnchor = Enums.TextAchorLocation.center;

         ArtCreditTwo = new Label("lblArtCreditTwo", "Levon Swenson (Geometry rumble)", new EngineRectangle(viewport.Width / 2 - 100, viewport.Height / 4 + 100, 200, 50), Color.White);
         ArtCreditTwo.minFontScale = ArtCreditTwo.maxFontScale = .5f;
         ArtCreditTwo.textAnchor = Enums.TextAchorLocation.center;

         ProgrammingDesign = new Label("lblProgramingTitle", "Programming", new EngineRectangle(viewport.Width / 2 - 100, viewport.Height / 2, 200, 50), Color.White);
         ProgrammingDesign.minFontScale = ProgrammingDesign.maxFontScale = 1f;
         ProgrammingDesign.textAnchor = Enums.TextAchorLocation.center;

         programmingCredit = new Label("lblProgramingCredit", "Levon Swenson", new EngineRectangle(viewport.Width / 2 - 100, viewport.Height / 2 + 50, 200, 100), Color.White);
         programmingCredit.minFontScale = programmingCredit.maxFontScale = .5f;
         programmingCredit.textAnchor = Enums.TextAchorLocation.center;

         backButton = new Button("btnBack", "Back", new EngineRectangle(0, 0, viewport.Width / 8, viewport.Width / 32), Color.White);
         backButton.onClick = new Base.Events.EHandler<Base.Events.OnClick>(new Action<object, Base.Events.OnClick>(backClick));
         backButton.minFontScale = 0;
         backButton.maxFontScale = 1f;

         curMenu = "Credits";
      }

      public void LoadUpgradeMenu()
      {
         curMenu = "Upgrade";
         upgradeMenu = new UpgradeMenu("upgradeScreen", "", new EngineRectangle(viewport.toRectangle()), Color.Black);
         upgradeMenu.onDoubleClick = new Base.Events.EHandler<Base.Events.Event>(new Action<object, Base.Events.Event>(backClick));
         upgradeMenu.parentGUIBus = EventBus;
      }

      public void LoadAchievmentMenu()
      {
         curMenu = "Achievments";
         AchievmentTitle = new Label("lblAchievmentTitle", "Achievments And Stats", new EngineRectangle(viewport.Width / 4, viewport.Height / 10, viewport.Width / 2, viewport.Height / 10), Color.White);
         AchievmentTitle.textAnchor = Enums.TextAchorLocation.center;
         AchievmentListBox = new ListBox("lstboxAchievement", "", new EngineRectangle(viewport.Width / 8, viewport.Height / 4, viewport.Width / 4, viewport.Height / 2), Color.White);
         foreach(KeyValuePair<string, AchievmentModel> model in AchievmentService.Achievments)
         {
            var item = new AchievementUI(model.Value,new EngineRectangle(viewport.Width / 8, viewport.Height / 4, viewport.Width / 4, viewport.Height / 2));
            AchievmentListBox.AddItem(item);
         }
         AchievmentListBox.setImageReferences("none", "none", "none", "none");
         AchievmentListBox.bar.setImageReferences("gold_bar", "gold_bar", "gold_bar", "gold_bar");
         AchievmentListBox.isFocused = true;
         AchievmentListBox.isEditing = true;

         ShotsFired = new Label("lblShotsFired", "Shots Fired: " + AchievmentService.Stats["Shots"].StatValue, new EngineRectangle( viewport.Width / 8 * 5, viewport.Height/ 4, viewport.Width / 4,viewport.Height / 8), Color.White);
         ShotsFired.minFontScale = 0f;
         ShotsFired.maxFontScale = 1f;
         EnemiesKilled = new Label("lblEnemiesKilled", "Enemies Killed: " + AchievmentService.Stats["Kills"].StatValue, new EngineRectangle(viewport.Width / 8 * 5, viewport.Height / 8 * 3, viewport.Width / 4, viewport.Height / 8), Color.White);
         EnemiesKilled.minFontScale = 0f;
         EnemiesKilled.maxFontScale = 1f;
         TotalDamageTaken = new Label("lblDamageTaken", "Damage Taken: " + AchievmentService.Stats["Damage Taken"].StatValue, new EngineRectangle(viewport.Width / 8 * 5, viewport.Height / 8 * 4, viewport.Width / 4, viewport.Height / 8), Color.White);
         TotalDamageTaken.minFontScale = 0f;
         TotalDamageTaken.maxFontScale = 1f;
         TotalDamageHealed = new Label("lblTotalDamageHealed", "Damage Healed: " + AchievmentService.Stats["Heath Healed"].StatValue, new EngineRectangle(viewport.Width / 8 * 5, viewport.Height / 8 * 5, viewport.Width / 4, viewport.Height / 8), Color.White);
         TotalDamageHealed.minFontScale = 0f;
         TotalDamageHealed.maxFontScale = 1f;
         MaxWaveWithoutDamage = new Label("lblMaxWaveWithoutDamage", "Highest Wave Without Damage: " + AchievmentService.Stats["Max Wave Taking No Damage"].StatValue, new EngineRectangle(viewport.Width / 8 * 5, viewport.Height / 8 * 6, viewport.Width / 4, viewport.Height / 8), Color.White);
         MaxWaveWithoutDamage.minFontScale = 0f;
         MaxWaveWithoutDamage.maxFontScale = 1f;

         backButton = new Button("btnBack", "Back", new EngineRectangle(0, 0, viewport.Width / 8, viewport.Width / 32), Color.White);
         backButton.onClick = new Base.Events.EHandler<Base.Events.OnClick>(new Action<object, Base.Events.OnClick>(backClick));
         backButton.minFontScale = 0;
         backButton.maxFontScale = 1f;
      }

      private void BackgroundVolumeTextUpdate(object sender, Base.Events.OnChange change)
      {
         string newValue = change.newValue.ToString();
         newValue = newValue.TrimEnd('_');
         if (!string.IsNullOrEmpty(newValue))
         {
            float volume = float.Parse(newValue);
            if (volume > 100)
            {
               volume = 100;
               var textbox = (Textbox)sender;
               textbox.value = volume.ToString() + '_';
            }
            volume = volume / 100;
            AudioService.SetBackgroundVolume(volume);
         }
      }

      private void SFXVolumeTextUpdate(object sender, Base.Events.OnChange change)
      {
         string newValue = change.newValue.ToString();
         newValue = newValue.TrimEnd('_');
         if (!string.IsNullOrEmpty(newValue))
         {
            float volume = float.Parse(newValue);
            if (volume > 100)
            {
               volume = 100;
               var textbox = (Textbox)sender;
               textbox.value = volume.ToString() + '_';
            }
            volume = volume / 100;
            AudioService.SetSoundEffectVolume(volume);
         }
      }

      private void backClick(object sender, Base.Events.OnClick click)
      {
         LoadMainMenu();
      }

      private void backClick(object sender, Base.Events.Event click)
      {
         LoadMainMenu();
      }

      private void StartGameClick(object sender, Base.Events.OnClick click)
      {
         parentScene.parentWorld.RemoveScene(parentScene);
         parentScene.parentWorld.AddScene(Factories.SceneFactory.SetupLoadoutMenuScene(false));
      }

      private void StartGameFromCheckpointClick(object sender, Base.Events.OnClick click)
      {
         parentScene.parentWorld.RemoveScene(parentScene);
         parentScene.parentWorld.AddScene(Factories.SceneFactory.SetupLoadoutMenuScene(true));
      }

      private void HighScoreClick(object sender, Base.Events.OnClick click)
      {
         LoadHighScoreMenu();
      }

      private void UpgradeClick(object sender, Base.Events.OnClick click)
      {
         LoadUpgradeMenu();
      }

      private void OptionClick(object sender, Base.Events.OnClick click)
      {
         LoadOptionMenu();
      }

      private void CreditsClick(object sender, Base.Events.OnClick click)
      {
         LoadCreditMenu();
      }

      private void AchievmentsClick(object sender, Base.Events.OnClick click)
      {
         LoadAchievmentMenu();
      }

      private void ExitClick(object sender, Base.Events.OnClick click)
      {
         ExitGameService.ExitGame();
      }

      private void checkboxClick(object sender, Base.Events.OnClick click)
      {
         var box = (CheckBox)sender;
         ConfigService.config.GoldenSkin = box.IsChecked;
         ConfigService.SaveConfig();
      }

      private void SpriteSetChange(object sender, Base.Events.OnChange change)
      {
         int index = (int)change.newValue;

         ConfigService.config.SpriteSet = spriteSetList.Items[index].Value.ToString();
         ConfigService.SaveConfig();

         string spriteDescription;
         if (ConfigService.config.SpriteSet == "Donuts and Sharks")
            spriteDescription = "One lonely donut must take on the ocean by itself. How long can its sprinkles last?";
         else
            spriteDescription = "No shape is safe. Defend yourself!";
         spriteSetDescripiton.value = spriteDescription;
      }
   }
}
