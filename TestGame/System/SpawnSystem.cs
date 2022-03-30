using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Base.System;
using Base.Entities;
using Base.Components;
using TestGame.Components;
using TestGame.Factories;
using Base.Scenes;
using TestGame.Utility.Services;
using Base.Utility.Services;
using TestGame.Utility;
using Base.Utility;
using TestGame.Events;
using Base.Events;

namespace TestGame.System
{
   //TODO: check if spawn is too cloase to player
   [Serializable]
   public class SpawnSystem:EngineSystem
   {
      public float spawnInterval;
      public float remainingInterval;
      public float Level;
      public int xMin, yMin, xMax, yMax;

      private Random rand;
      private int spawnerEnemyChance;
      private int chargingEnemyChance;
      private int enemiesInWave;
      private int enemiesLeftInWave;
      private bool isEnded;

      Entity finialBossSquid;
      bool isSquidDead;
      Entity finialBossHammer;
      bool isHammerDead;


      public SpawnSystem(Scene s)
      {
         SetDefaults();
         Init(s);
      }

      public SpawnSystem()
      {
         SetDefaults();
      }

      private void SetDefaults()
      {
         systemSignature = (uint)(1 << SpawningComponent.GetFamily() | 1 << Transform.GetFamily());
         registeredEntities = new List<Entity>();
         spawnInterval = remainingInterval = 1000;
         xMin = yMin = 0;
         xMax = yMax = 0;
         Level = 1;
         spawnerEnemyChance = 0;
         chargingEnemyChance = 0;
         enemiesInWave = enemiesLeftInWave = 50;
         rand = new Random();
         isEnded = false;
         isSquidDead = false;
         isHammerDead = false;
      }

      public override void Init(Scene s)
      {
         RegisterScene(s);
         parentScene.bus.Subscribe(new Base.Events.EHandler<GameOverEvent>(new Action<object, GameOverEvent>(HandlePlayerDied)));
         parentScene.bus.Subscribe(new Base.Events.EHandler<ObjectDetroyedEvent>(new Action<object, ObjectDetroyedEvent>(HandleEnemyDeath)));
      }

      public override void Update(int dt)
      {
         if (!isEnded)
         {
            remainingInterval -= dt;
            if (remainingInterval <= 0)
            {
               int tries = 0;
               int xValue = rand.Next(xMin, xMax);
               int yValue = rand.Next(yMin, yMax);
               if (PlayerTransformService.TranformAssigned)
               {
                  Transform t = PlayerTransformService.GetPlayerTransform();
                  while (tries < 5 && xValue < t.X + 400 && xValue > t.X - 400
                     && yValue < t.Y + 400 && yValue > t.Y - 400)
                  {
                     xValue = rand.Next(xMin, xMax);
                     yValue = rand.Next(yMin, yMax);
                     tries++;
                  }
               }
               int spawnChance = rand.Next();
               if (spawnChance % 100 > spawnerEnemyChance + chargingEnemyChance)
               {
                  EnemyFactory.createEnemy(parentScene, xValue, yValue);
               }
               else if (spawnChance % 100 > spawnerEnemyChance)
               {
                  EnemyFactory.CreateChargingEnemy(parentScene, xValue, yValue);
               }
               else
               {
                  EnemyFactory.CreateSpawnerEnemy(parentScene, xValue, yValue);
               }
               remainingInterval = spawnInterval;
               enemiesLeftInWave--;
               if (enemiesLeftInWave <= 0)
                  SetupNextWave(true);
            }

            foreach (Entity e in registeredEntities)
            {
               SpawningComponent SC = parentScene.GetComponent<SpawningComponent>(e);
               SC.remainingTime -= dt;
               if (SC.remainingTime <= 0)
               {

                  Transform t = parentScene.GetComponent<Transform>(e);

                  int xValue = rand.Next(xMin, xMax);
                  int yValue = rand.Next(yMin, yMax);

                  EnemyFactory.createEnemy(parentScene, t.X, t.Y, SC.spawnAmount);
                  SC.remainingTime = SC.spawnTime;
               }
            }
         }
      }

      public void SetupNextWave(bool canSpawn)
      {
         Level++;
         if (canSpawn)
         {
            if (Level % 5 == 0)
            {
               if (Level == 5)
               {
                  var location = getValidRandomCoordinates();
                  EnemyFactory.CreateHammerHeadBoss(parentScene, location.X, location.Y);
               }
               if (Level == 10)
               {
                  var location = getValidRandomRightWallCoordinates();
                  EnemyFactory.CreateSquidBoss(parentScene, location.X - 200, location.Y, 3, true);
               }
               if (Level == 15)
               {
                  var location = getValidRandomCoordinates();
                  EnemyFactory.CreateHammerHeadBoss(parentScene, location.X, location.Y);
                  location = getValidRandomCoordinates();
                  EnemyFactory.CreateHammerHeadBoss(parentScene, location.X, location.Y);
               }
               if (Level == 20)
               {
                  var location = getValidRandomRightWallCoordinates();
                  EnemyFactory.CreateSquidBoss(parentScene, location.X - 200, location.Y, 3, true);
                  location = getValidRandomLeftWallCoordinates();
                  EnemyFactory.CreateSquidBoss(parentScene, location.X, location.Y, 3, false);
               }
               if (Level == 25)
               {
                  var location = getValidRandomRightWallCoordinates();
                  finialBossSquid = EnemyFactory.CreateSquidBoss(parentScene, location.X, location.Y, 3, true);
                  location = getValidRandomCoordinates();
                  finialBossHammer =  EnemyFactory.CreateHammerHeadBoss(parentScene, location.X, location.Y);
               }
            }
         }
         if (Level == 26) 
         {
            isEnded = true;
         }
         
         if (Level % 5 == 0)
         {
            TestGameSaveFile save =  (TestGameSaveFile)SaveService.Save;
            if(Level > save.CheckpointReached)
            {
               save.CheckpointReached = (int)Level;
               SaveService.SaveSave();
            }
         }

         parentScene.bus.Publish(this, new TestGame.Events.NewWaveEvent((int)Level));
         spawnerEnemyChance += 2;
         chargingEnemyChance += 3;
         if (spawnerEnemyChance > 20)
            spawnerEnemyChance = 20;
         if (chargingEnemyChance > 30)
            chargingEnemyChance = 30;
         enemiesInWave += 5;
         enemiesLeftInWave = enemiesInWave;
         spawnInterval -= 5;
         if (spawnInterval < 100)
            spawnInterval = 100;
         remainingInterval = 5000;
      }

      private void HandleEnemyDeath(object sender, ObjectDetroyedEvent e)
      {
         if(e.entityToDestroy.id == finialBossHammer.id)
         {
            isHammerDead = true;
         }
         if (e.entityToDestroy.id == finialBossHammer.id)
         {
            isSquidDead = true;
         }
         if(isHammerDead  &&  isSquidDead)
         {
            isEnded = true;
         }
      }

      private EngineVector2 getValidRandomCoordinates()
      {
         int xValue = rand.Next(xMin, xMax);
         int yValue = rand.Next(yMin, yMax);
         int tries = 0;
         if (PlayerTransformService.TranformAssigned)
         {
            Transform t = PlayerTransformService.GetPlayerTransform();
            while (tries < 5 && xValue < t.X + 400 && xValue > t.X - 400
               && yValue < t.Y + 400 && yValue > t.Y - 400)
            {
               xValue = rand.Next(xMin, xMax);
               yValue = rand.Next(yMin, yMax);
               tries++;
            }
         }
         return new EngineVector2(xValue, yValue);
      }

      private EngineVector2 getValidRandomRightWallCoordinates()
      {
         int xValue = rand.Next(xMax - 10, xMax - 10);
         int yValue = rand.Next(yMin, yMax);
         int tries = 0;
         if (PlayerTransformService.TranformAssigned)
         {
            Transform t = PlayerTransformService.GetPlayerTransform();
            while (tries < 5 && xValue < t.X + 400 && xValue > t.X - 400
               && yValue < t.Y + 400 && yValue > t.Y - 400)
            {
               xValue = rand.Next(xMin, xMax);
               yValue = rand.Next(yMin, yMax);
               tries++;
            }
         }
         return new EngineVector2(xValue, yValue);
      }

      private EngineVector2 getValidRandomLeftWallCoordinates()
      {
         int xValue = rand.Next(xMin + 10, xMin + 10);
         int yValue = rand.Next(yMin, yMax);
         int tries = 0;
         if (PlayerTransformService.TranformAssigned)
         {
            Transform t = PlayerTransformService.GetPlayerTransform();
            while (tries < 5 && xValue < t.X + 400 && xValue > t.X - 400
               && yValue < t.Y + 400 && yValue > t.Y - 400)
            {
               xValue = rand.Next(xMin, xMax);
               yValue = rand.Next(yMin, yMax);
               tries++;
            }
         }
         return new EngineVector2(xValue, yValue);
      }

      private void HandlePlayerDied(object sender, GameOverEvent e)
      {
         isEnded = true;
         for (int i = registeredEntities.Count - 1; i >= 0; i--)
         {
            parentScene.DestroyEntity(registeredEntities[i]);
         }   
      }
   }
}
