using Base.Collision;
using Base.Components;
using Base.Entities;
using Base.Scenes;
using Base.System;
using Base.Utility;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Base.Utility.Services;
using TestGame.Components;
using TestGame.Utility.Services;
using TestGame.Model;
using TestGame.Events;
using TestGame.System;

namespace TestGame.Factories
{
   public static class SceneFactory
   {
      public static Scene SetupMainScene(bool startAtcheckpoint)
      {
         CameraService.camera.Position = new Vector2(960, 540);
         //CameraService.camera.Position = new Vector2(320, 320);

         var MaxHealthInfo = ShopService.Stats.Find(x => x.ItemName == "Max Health");
         var speedInfo = ShopService.Stats.Find(x => x.ItemName == "Speed");
         var saveFile = (TestGame.Utility.TestGameSaveFile)SaveService.Save;

         var CharacterMaxHealth = MaxHealthInfo.BaseValue + MaxHealthInfo.ValueIncrease * saveFile.Stats.Find(x => x.ItemName == "Max Health").Level;
         var CharacterSpeed = speedInfo.BaseValue + speedInfo.ValueIncrease * saveFile.Stats.Find(x => x.ItemName == "Speed").Level;

         Scene s = new Scene();
         s.sceneID = "Main Game";
         AudioService.PlaySong("Fight");

         //Player Entity
         Entity pcEntity = s.CreateEntity();
         Transform pcTransform = new Transform(900, 900, 1, 1, 0);
         PlayerController pcController = new PlayerController(CharacterSpeed, ControlService.controls);

         //Determin texture
         string spriteTextureName = "defaultTexture";
         if (ConfigService.config.SpriteSet == "Donuts and Sharks")
         {
            if (ConfigService.config.GoldenSkin)
            {
               spriteTextureName = "donut_gold";
            }
            else
            {
               spriteTextureName = "donut";
            }
         }
         else
         {
            spriteTextureName = "splash";
         }
         Sprite pcSprite = new Sprite(spriteTextureName);
         pcSprite.imageWidth = 20; //50T
         pcSprite.imageHeight = 20; //50 TODO
         pcSprite.zOrder = 1;
         RigidBody2D pcRigid = new RigidBody2D(1, false, false, 50);
         BoxCollisionBound2D pcBC = new BoxCollisionBound2D(0, 0, 1, 1, (int)Utility.CombatIdType.player);
         ColliderTwoD pcCollider = new ColliderTwoD(new global::System.Collections.Generic.List<ICollisionBound2D> { pcBC });
         Components.HealthComponent HC = new Components.HealthComponent(CharacterMaxHealth);
         Components.CombatComponent combatComponent = new Components.CombatComponent(10, Utility.CombatIdType.player);
         TestGame.Components.ShootingComponent SC = new Components.ShootingComponent(new List<WeaponModel> { LoadoutService.Loadout.FirstWeapon, LoadoutService.Loadout.SecondWeapon});
         HealthBarComponent HBC = new HealthBarComponent(1);
         TrinketComponent trinketComponent = new TrinketComponent(new List<SecendaryWeaponModel>() { LoadoutService.Loadout.Trinket});
         AbilityComponent abilityComponent = new AbilityComponent(new List<AbilityModel>() { LoadoutService.Loadout.Ability });


         PlayerTransformService.AssignPlayerTransform(pcTransform);


         s.AddComponent(pcEntity, pcTransform);
         s.AddComponent(pcEntity, pcSprite);
         s.AddComponent(pcEntity, pcController);
         s.AddComponent(pcEntity, pcRigid);
         s.AddComponent(pcEntity, pcCollider);
         s.AddComponent(pcEntity, HC);
         s.AddComponent(pcEntity, combatComponent);
         s.AddComponent(pcEntity, SC);
         s.AddComponent(pcEntity, HBC);
         s.AddComponent(pcEntity, trinketComponent);
         s.AddComponent(pcEntity, abilityComponent);


         //GUI overlay Entity
         Entity guiEntity = s.CreateEntity();
         TestGame.Components.overlayUI overlay = new Components.overlayUI();
         overlay.Init(s.bus, s);

         s.AddComponent(guiEntity, overlay);


         Entity guiInGameMenuEntity = s.CreateEntity();
         TestGame.Components.InGameMenuComponent inGameMenuGUI = new Components.InGameMenuComponent();
         inGameMenuGUI.Init(s.bus, s);

         s.AddComponent(guiInGameMenuEntity, inGameMenuGUI);

         if (ConfigService.config.SpriteSet == "Donuts and Sharks")
         {
            //Background
            Entity background = s.CreateEntity();
            Sprite backgroundSprite = new Sprite("background2");
            backgroundSprite.imageHeight = 1080;
            backgroundSprite.imageWidth = 1920;
            backgroundSprite.zOrder = 0;
            Transform backgroundTransform = new Transform(0, 0, 1, 1, 0);

            s.AddComponent(background, backgroundSprite);
            s.AddComponent(background, backgroundTransform);
         }

         //Collision MaskCollection
         const int pickupLayer = 10;
         MaskCollection layerMask = new MaskCollection();
         Mask playerMask = new Mask(uint.MaxValue);
         playerMask.ClearBit((int)Utility.CombatIdType.player);
         playerMask.ClearBit((int)Utility.CombatIdType.shot);
         playerMask.ClearBit((int)Utility.CombatIdType.bomb);
         Mask enemyMask = new Mask(uint.MaxValue);
         //enemyMask.ClearBit((int)Utility.CombatIdType.enemy);
         enemyMask.ClearBit((int)Utility.CombatIdType.spawnEnemy);
         enemyMask.ClearBit((int)Utility.CombatIdType.charging);
         Mask shotMask = new Mask(uint.MaxValue);
         shotMask.ClearBit((int)Utility.CombatIdType.player);
         shotMask.ClearBit((int)Utility.CombatIdType.shot);
         shotMask.ClearBit(pickupLayer);
         Mask bombMask = new Mask(uint.MaxValue);
         bombMask.ClearBit((int)Utility.CombatIdType.player);
         bombMask.ClearBit((int)Utility.CombatIdType.shot);
         bombMask.ClearBit(pickupLayer);
         bombMask.ClearBit((int)Utility.CombatIdType.boss);
         Mask SpawnerMask = new Mask(uint.MaxValue);
         SpawnerMask.ClearBit((int)Utility.CombatIdType.enemy);
         SpawnerMask.ClearBit((int)Utility.CombatIdType.spawnEnemy);
         SpawnerMask.ClearBit((int)Utility.CombatIdType.charging);
         SpawnerMask.ClearBit((int)Utility.CombatIdType.boss);
         Mask pickupMask = new Mask(0);
         pickupMask.SetBit((int)Utility.CombatIdType.player);
         Mask chargerMask = new Mask(0);
         chargerMask.SetBit((int)Utility.CombatIdType.player);
         chargerMask.SetBit((int)Utility.CombatIdType.shot);
         chargerMask.SetBit((int)Utility.CombatIdType.bomb);
         Mask BossMask = new Mask(0);
         BossMask.SetBit((int)Utility.CombatIdType.player);
         BossMask.SetBit((int)Utility.CombatIdType.shot);

         layerMask.SetMaskValue((int)Utility.CombatIdType.player, playerMask);
         layerMask.SetMaskValue((int)Utility.CombatIdType.enemy, enemyMask);
         layerMask.SetMaskValue((int)Utility.CombatIdType.shot, shotMask);
         layerMask.SetMaskValue((int)Utility.CombatIdType.spawnEnemy, SpawnerMask);
         layerMask.SetMaskValue(pickupLayer, pickupMask);
         layerMask.SetMaskValue((int)Utility.CombatIdType.charging, chargerMask);
         layerMask.SetMaskValue((int)Utility.CombatIdType.boss, BossMask);

         //Creating all the systems
         InputSystem IS = new InputSystem();
         PlayerControllerSystem controlerSystem = new PlayerControllerSystem();
         SimplePlayerController spcs = new SimplePlayerController();
         CollisionSystemTwoD CS2D = new CollisionSystemTwoD(s, 1920, 1080, layerMask);
         PhisycsSystem PS = new PhisycsSystem();
         TwoDRenderSystem TDRS = new TwoDRenderSystem();
         TileTwoDRenderSystem ttdrs = new TileTwoDRenderSystem();
         //CameraSystem CS = new CameraSystem();
         //CS.isClampedToBounds = true;
         BoundrySystem BS = new BoundrySystem();
         System.SpawnSystem SS = new System.SpawnSystem();
         SS.xMax = 1920;
         SS.yMax = 1080;
         SS.spawnInterval = 500;
         SS.remainingInterval = SS.spawnInterval;
         SS.Level = 1;
         System.AISystem AI = new System.AISystem();
         System.CombatSystem combatSystem = new System.CombatSystem();
         System.ShootingSystem shootingSystem = new System.ShootingSystem();
         System.BulletSystem bulletSystem = new System.BulletSystem();
         System.LifetimeSystem LC = new System.LifetimeSystem();
         System.PointsSystems pointsSystem = new System.PointsSystems();
         System.LeechShotSystem leechShotSystem = new System.LeechShotSystem();
         leechShotSystem.player = pcEntity;
         System.MergeSystem mergeSeperateSystem = new System.MergeSystem();
         GUISystem GS = new GUISystem();
         System.BombSystem bombSystem = new System.BombSystem();
         System.HealthBarSystem HBS = new System.HealthBarSystem();
         System.PickupSystem pickupSystem = new System.PickupSystem();
         pickupSystem.xMax = 1920;
         pickupSystem.yMax = 1080;
         System.TileWallSystem TWS = new System.TileWallSystem();
         System.TrinketSystem trinketSystem = new System.TrinketSystem();
         System.AbilitySystem abilitySystem = new System.AbilitySystem();
         System.LifeFountainSystem lifeFountainSystem = new System.LifeFountainSystem();
         System.DecoySystem decoySystem = new System.DecoySystem();
         System.HammerBossAISystem hammerBossAISystem = new System.HammerBossAISystem();
         System.SquidBossAiSystem squidBossAISystem = new System.SquidBossAiSystem();
         squidBossAISystem.worldBounds = new EngineRectangle(0, 0, 1920, 1080);
         Base.System.AnimationSystem animationSystem = new AnimationSystem();

         AchievmentStatSystem AS = new AchievmentStatSystem();
         

         //Adding all the systems
         s.AddSystem(IS);
         s.AddSystem(controlerSystem);
         s.AddSystem(spcs);
         s.AddSystem(AI);
         s.AddSystem(hammerBossAISystem);
         s.AddSystem(squidBossAISystem);
         s.AddSystem(PS);
         s.AddSystem(CS2D);
         //s.AddSystem(CS);
         s.AddSystem(BS);
         s.AddSystem(SS);
         s.AddSystem(combatSystem);
         s.AddSystem(shootingSystem);
         s.AddSystem(trinketSystem);
         s.AddSystem(abilitySystem);
         s.AddSystem(bulletSystem);
         s.AddSystem(leechShotSystem);
         s.AddSystem(lifeFountainSystem);
         s.AddSystem(decoySystem);
         s.AddSystem(LC);
         s.AddSystem(pointsSystem);
         s.AddSystem(mergeSeperateSystem);
         s.AddSystem(bombSystem);
         s.AddSystem(HBS);
         s.AddSystem(pickupSystem);
         s.AddSystem(TWS);
         s.AddSystem(ttdrs);
         s.AddSystem(TDRS);
         s.AddSystem(animationSystem);
         s.AddSystem(GS);
         s.AddSystem(AS);

         if (startAtcheckpoint)
         {
            for (int i = 1; i < saveFile.CheckpointReached; i++)
            {
               if (i == saveFile.CheckpointReached - 1)
               {
                  SS.SetupNextWave(true);
               }
               else
               {
                  SS.SetupNextWave(false);
               }
            }
            AS.hasTakenDamage = true;
         }

         //TODO: update count for weapon trinket and ability usage
         //Setup Overlay UI
         if (LoadoutService.Loadout.FirstWeapon != null)
         {
            s.bus.Publish(null, new GunChangeEvent(LoadoutService.Loadout.FirstWeapon.GunType));
         }
         else
         {
            s.bus.Publish(null, new GunChangeEvent(LoadoutService.Loadout.SecondWeapon.GunType));
         }
         if (LoadoutService.Loadout.Trinket != null)
         {
            s.bus.Publish(null, new TrinketShotEvent(null, trinketComponent.TrinketAmount[LoadoutService.Loadout.Trinket.TrinketType], LoadoutService.Loadout.Trinket.TrinketType));
         }
         if (LoadoutService.Loadout.Ability != null)
         {
            s.bus.Publish(null, new AbilityCooldownChangeEvent(LoadoutService.Loadout.Ability.AbilityType, 1, 0));
         }

         return s;
      }

      public static Scene SetupMenuScene(EngineRectangle rectangle)
      {
         AudioService.StopSong();
         Scene s = new Scene();
         s.sceneID = "Main Menu";
         AudioService.StopSong();

         Entity MenuEnitty = s.CreateEntity();
         MainMenuGUI menu = new MainMenuGUI();
         menu.Init(s.bus, s);

         s.AddComponent(MenuEnitty, menu);

         GUISystem GS = new GUISystem();
         InputSystem IS = new InputSystem();
         AchievmentStatSystem AS = new AchievmentStatSystem();
         //TileTwoDRenderSystem ttdrs = new TileTwoDRenderSystem();

         s.AddSystem(IS);
         //s.AddSystem(ttdrs);
         s.AddSystem(GS);
         s.AddSystem(AS);
         

         return s;
      }

      public static Scene SetupLoadoutMenuScene(bool fromCheckpoint)
      {
         AudioService.StopSong();
         Scene s = new Scene();
         s.sceneID = "Loadout Menu";
         AudioService.StopSong();

         Entity MenuEnitty = s.CreateEntity();
         LoadOutSelectionUI menu = new LoadOutSelectionUI();
         menu.FromCheckpoint = fromCheckpoint;
         menu.Init(s.bus, s);

         s.AddComponent(MenuEnitty, menu);

         GUISystem GS = new GUISystem();
         InputSystem IS = new InputSystem();

         s.AddSystem(IS);
         s.AddSystem(GS);

         //TODO: add logic to add loadout menu to scene
         return s;
      }
   }
}
