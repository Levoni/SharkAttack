using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Base.Collision;
using Base.Components;
using Base.Entities;
using Base.Scenes;
using Base.Serialization;
using Base.System;
using Base.UI;
using Base.Utility;
using Base.Utility.Services;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TestGame.Events;
using TestGame.Utility.Services;
using Base.Serialization.Interfaces;

namespace TestGame.Components
{
   [Serializable]
   public class InGameMenuComponent:GUI
   {
      Button btnRestart;
      Button btnHighScore;
      Label nameLabel;
      Textbox nameInput;
      Label unlockTest;

      Button btnSerialize;

      private float endGameScore;
      private int endGameWave;


      public override void Update(int dt)
      {
         updateMenu(dt);
      }

      public override void Render(SpriteBatch sb)
      {
         renderOverlay(sb);
      }

      public override void Init()
      {
         btnRestart.init();
         btnHighScore.init();
         nameLabel.init();
         nameInput.init();
         btnSerialize.init();
      }

      public override void Init(Base.Events.EventBus eb, Scene parentScene)
      {
         var viewport = ScreenGraphicService.GetViewportBounds();
         btnRestart = new Button("btnRestart", "Return to Main Menu", new EngineRectangle(860, 700, 200, 50), Microsoft.Xna.Framework.Color.White);
         btnRestart.isEnabled = false;
         btnRestart.minFontScale = 0;
         btnRestart.maxFontScale = 1f;
         btnRestart.padding = new int[]
         {
            0,10,0,10
         };

         btnHighScore = new Button("btnHighScore", "Save Score", new EngineRectangle(860, 590, 200, 100), Color.White);
         btnHighScore.onClick = new Base.Events.EHandler<Base.Events.OnClick>(new Action<object, Base.Events.OnClick>(HighScoreBtnClick));
         btnHighScore.minFontScale = 0;
         btnHighScore.maxFontScale = 1f;
         btnHighScore.isEnabled = false;
         btnHighScore.padding = new int[]
         {
            0,10,0,10
         };

         btnSerialize = new Button("btnSer", "Serialize", new EngineRectangle(0, 450, 200, 50), Microsoft.Xna.Framework.Color.White);

         var validChars = new HashSet<char>()
         {
            'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n',
            'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z', 'A', 'B', 'C',
            'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R',
            'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z', '0', '9', '8', '7', '6', '5', '4',
            '3', '2', '1', ' ',
         };
         nameInput = new Textbox("txtboxName", "", new EngineRectangle(860, 490, 200, 100), Color.Black, 3, validChars);
         nameInput.isEnabled = false;

         nameLabel = new Label("lblName", "Input 3 Character High Score Name", new EngineRectangle(760, 390, 400, 100), Color.White);
         nameLabel.isEnabled = false;
         nameLabel.minFontScale = 0;
         nameLabel.maxFontScale = 1f;
         nameLabel.textAnchor = Enums.TextAchorLocation.center;

         unlockTest = new Label("lblUnlock", "You have unlocked a new skin in the options menu. A new weapon has also been unlocked in the armory.", new EngineRectangle(viewport.Width * 4, viewport.Height / 4 * 3, viewport.Width / 2, 200), Color.White);
         unlockTest.minFontScale = 0;
         unlockTest.maxFontScale = 1;
         unlockTest.isMultiLine = true;
         unlockTest.isEnabled = false;


         base.Init(eb, parentScene);
         EventBus.Subscribe(new Base.Events.EHandler<GameOverEvent>(new Action<object, GameOverEvent>(HandlePlayerDied)));
         btnRestart.onClick = new Base.Events.EHandler<Base.Events.OnClick>(new Action<object, Base.Events.OnClick>(RestartBtnClick));
         btnSerialize.onClick = new Base.Events.EHandler<Base.Events.OnClick>(new Action<object, Base.Events.OnClick>(SerializeClick));
      }

      public void updateMenu(int dt)
      {
         btnRestart.Update(dt);
         nameLabel.Update(dt);
         nameInput.Update(dt);
         btnHighScore.Update(dt);
         unlockTest.Update(dt);
         //btnSerialize.Update();
      }

      public void renderOverlay(SpriteBatch sb)
      {
         btnRestart.Render(sb);
         nameLabel.Render(sb);
         nameInput.Render(sb);
         btnHighScore.Render(sb);
         unlockTest.Render(sb);
         //btnSerialize.Render(sb);
      }

      public void HandlePlayerDied(object sender, GameOverEvent e)
      {
         PointsService.LoadHighScores();
         if (PointsService.CheckForHighScore(e.GameScore))
         {
            nameInput.isEnabled = true;
            nameLabel.isEnabled = true;
            btnHighScore.isEnabled = true;
            endGameScore = e.GameScore;
            endGameWave = e.Wave;
         }
         else
         {
            endGameScore = -1;
            nameLabel.value = "Game Over";
            nameLabel.isEnabled = true;
         }

         btnRestart.isEnabled = true;

         if(e.Wave == 26)
         {
            //show unlock text
            if (!((TestGame.Utility.TestGameSaveFile)SaveService.Save).BeatGame)
            {
               unlockTest.isEnabled = true;
            }
            nameLabel.value = "Congradulations on beating the game!";
            ((TestGame.Utility.TestGameSaveFile)SaveService.Save).CheckpointReached = 1;
            ((TestGame.Utility.TestGameSaveFile)SaveService.Save).BeatGame = true;
            SaveService.SaveSave();
         }
      }

      public void RestartBtnClick(object sender, Base.Events.OnClick click)
      {
         //restart stuff
         PlayerTransformService.UnassignAllTransforms();
         parentScene.parentWorld.RemoveScene(parentScene);
         parentScene.parentWorld.AddScene(Factories.SceneFactory.SetupMenuScene(new EngineRectangle(0, 0, 1920, 1080)));
      }

      public void HighScoreBtnClick(object sender, Base.Events.OnClick click)
      {
         if (endGameScore != -1 && nameInput.value.Length == 3)
         {
            PointsService.InsertHighScore(endGameScore, nameInput.value, endGameWave);
            PointsService.SaveHighScores();
            btnHighScore.isEnabled = false;
         }
      }

      public void SerializeClick(object sender, Base.Events.OnClick click)
      {
         if (!Base.Utility.Services.DirectoryService.DoesFileExist("test_serialize.scene"))
         {
            BSerializer.serializeObject("test_serialize", "scene", parentScene);
         }
         else
         {
            object scene = BSerializer.deserializeObject("test_serialize", "scene");
            parentScene.parentWorld.AddScene((Scene)scene);
            foreach(BaseComponentManager BCM in ((Scene)scene).componentManagers)
            {
               if (BCM != null)
               {
                  foreach (KeyValuePair<int, BaseComponent> BC in BCM.GetEntityIComponentDictionary())
                  {
                     BC.Value.Init();
                  }
               }
            }
            foreach (EngineSystem s in ((Scene)scene).systems)
            {
               s.Init((Scene)scene);
            }
         }
      }
   }
}
