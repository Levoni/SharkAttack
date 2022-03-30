using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using Base.Scenes;
using Base.Entities;
using Base.Components;
using Base.System;
using Base.Collision;
using Base.Utility;
using Base.Utility.Services;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using TestGame.Utility.Services;
using TestGame.Model;
using System.Collections.Generic;
using Base.Serialization;

namespace TestGame
{
   /// <summary>
   /// This is the main type for your game.
   /// </summary>
   public class Game1 : Game
   {
      GraphicsDeviceManager graphics;
      SpriteBatch spriteBatch;
      Base.World W;

      public Game1()
      {
         graphics = new GraphicsDeviceManager(this);
         //graphics.IsFullScreen = true;
         Content.RootDirectory = "Content";
         graphics.PreferredBackBufferWidth = 1920;
         graphics.PreferredBackBufferHeight = 1080;
         //graphics.ToggleFullScreen();
         ScreenGraphicService.InitializeService(graphics);

      }

      /// <summary>
      /// Allows the game to perform any initialization it needs to before starting to run.
      /// This is where it can query for any required services and load any non-graphic
      /// related content.  Calling base.Initialize will enumerate through any components
      /// and initialize them as well.
      /// </summary>
      protected override void Initialize()
      {
         // TODO: Add your initialization logic here
         IsMouseVisible = true;
         base.Initialize();
      }

      /// <summary>
      /// LoadContent will be called once per game and is the place to load
      /// all of your content.
      /// </summary>
      protected override void LoadContent()
      {
         // Create a new SpriteBatch, which can be used to draw textures.
         spriteBatch = new SpriteBatch(GraphicsDevice);

         // TODO: use this.Content to load your game content here

         float testWidth = 640;
         float testHeight = 640;

         KeyboardService.InitService();
         MouseService.InitService();
         ContentService.InitService(Content);
         DirectoryService.CreateBasicGameDirectories();
         CameraService.InitSystem();
         TestGame.Utility.Services.PointsService.Init();
         CameraService.camera.ViewportWidth = (int)1920;
         CameraService.camera.ViewportHeight = (int)1080;
         //CameraService.camera.ClampBounds = new EngineRectangle(-50000, -50000, 1000000, 1000000);
         //CameraService.camera.MinZoom = .1f;
         //CameraService.camera.MaxZoom = 10;
         //CameraService.camera.Zoom = MathHelper.Max(1920 / testWidth, 1080 / testHeight);
         //CameraService.camera.Position = new Vector2(0, 0);

         ControlService.Init();
         ControlService.AddOrUpdateControl(Enums.ControlType.modifier2, Microsoft.Xna.Framework.Input.Keys.Q);
         ControlService.AddOrUpdateControl(Enums.ControlType.ability1, Microsoft.Xna.Framework.Input.Keys.E);
         ControlService.SaveControls();

         AudioService.Initialize(Content);
         AudioService.SetSoundEffectVolume(0);
         AudioService.AddSong("Fight");
         AudioService.SetBackgroundVolume(0);

         ExitGameService.Initialize(this);

         ConfigService.LoadConfig();
         ShopService.LoadShopInfo();

         if (!DirectoryService.DoesFileExist("save.save"))
         {
            Utility.TestGameSaveFile testGameSave = new Utility.TestGameSaveFile();
            SaveService.Save = testGameSave;
            SaveService.SaveSave();
         }

         SaveService.LoadSave();
         LoadoutService.LoadLoadout();
         AchievmentService.LoadAchievmentsAndStats(); 

         TestGame.Utility.TestGameSaveFile save = (Utility.TestGameSaveFile)SaveService.Save;
         if (save.Stats == null || save.Stats.Count == 0)
         {
            foreach (StatModel o in ShopService.Stats)
            {
               InventoryItemModel inventoryItemModel = new InventoryItemModel
               {
                  Level = 0,
                  ItemName = o.ItemName,
                  ImageReference = o.ImageReference,
                  ItemDescription = o.ItemDescription
               };
               save.Stats.Add(inventoryItemModel);
            }
            foreach(WeaponModel wm in ShopService.Weapons)
            {
               if (wm.ItemName == "Pistol")
               {
                  InventoryItemModel inventoryItemModel = new InventoryItemModel
                  {
                     Level = 1,
                     ItemName = wm.ItemName,
                     ItemDescription = wm.ItemDescription,
                     ImageReference = wm.ImageReference
                  };
                  save.Weapons.Add(inventoryItemModel);
               }
            }
         }

         List<string> testString = new List<string>();
         testString.Add("test");
         testString.Add("two");
         JSerializer.Serialize("arrayTest", "", "",testString);

         JSerializer.Serialize("test", "txt", "", save);

         List<AbilityModel> alist = new List<AbilityModel>();
         alist.Add(new AbilityModel());
         alist.Add(new AbilityModel());

         JSerializer.Serialize("alistTest", "", "", alist);

         var stuff = JSerializer.Deserialize<List<SecendaryWeaponModel>>("SecendaryWeapons", "txt", "./Content/");

         Base.World W = new Base.World();
         Scene s = TestGame.Factories.SceneFactory.SetupMenuScene(new EngineRectangle(graphics.GraphicsDevice.Viewport.Bounds));
         s.sceneID = "Main Menu";
         W.AddScene(s);


         this.W = W;
      }

      /// <summary>
      /// UnloadContent will be called once per game and is the place to unload
      /// game-specific content.
      /// </summary>
      protected override void UnloadContent()
      {
         // TODO: Unload any non ContentManager content here
      }

      /// <summary>
      /// Allows the game to run logic such as updating the world,
      /// checking for collisions, gathering input, and playing audio.
      /// </summary>
      /// <param name="gameTime">Provides a snapshot of timing values.</param>
      protected override void Update(GameTime gameTime)
      {
         if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();
         
         // TODO: Add your update logic here
         W.UpdateScenes(gameTime);

         //Base.Serialization.SerializableScene ss = ;
         //Base.Scenes.SceneService.CreateFileFromScene(singleScene);

         base.Update(gameTime);
      }

      /// <summary>
      /// This is called when the game should draw itself.
      /// </summary>
      /// <param name="gameTime">Provides a snapshot of timing values.</param>
      protected override void Draw(GameTime gameTime)
      {
         GraphicsDevice.Clear(Color.Black);

         // TODO: Add your drawing code here
         W.RenderScenes(spriteBatch);
           var temp = (int)CompareFunction.Always;
         base.Draw(gameTime);
      }
   }
}
