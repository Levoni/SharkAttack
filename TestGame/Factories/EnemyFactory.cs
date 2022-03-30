using Base.Components;
using Base.Entities;
using Base.Scenes;
using Base.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestGame.Components;
using TestGame.Utility.Services;

namespace TestGame.Factories
{
   static class EnemyFactory
   {
      public static Entity createEnemy(Scene scene, float x, float y, float health = 1, int imgWidth = 15, int imgheight = 15, int damage = 1, float weight = .1f)
      {
         Entity aiEntity = scene.CreateEntity();

         string spriteTextureName = "defaultTexture";
         if(ConfigService.config.SpriteSet == "Donuts and Sharks")
         {
            spriteTextureName = "shark";
         }
         else
         {
            spriteTextureName = "defaultTexture";
         }
         Sprite s = new Sprite(spriteTextureName);
         s.imageWidth = (int)(imgWidth * health + 5);
         s.imageHeight = (int)(imgheight * health + 5);
         s.zOrder = 1;

         Transform t = new Transform(x, y, 1f, 1f, 0);
         t.rotation = -90;

         AIComponent AI = new AIComponent();
         AI.type = Utility.AiType.zombie;
         Transform playerTransform = new Transform();
         foreach (Entity e in scene.entityManager.GetAllEntities())
         {
            PlayerController pc = scene.GetComponent<PlayerController>(e);
            if (pc != null)
            {
               playerTransform = scene.GetComponent<Transform>(e);
               break;
            }
         }
         AI.targetTransform = playerTransform;
         AI.speed = 3;

         t.rotation = (float) new EngineVector2(playerTransform.X - t.X, playerTransform.Y - t.Y).GetDegreeRotation();

         Base.Collision.BoxCollisionBound2D BCBC = new Base.Collision.BoxCollisionBound2D(0, 0, 1, 1, (int)Utility.CombatIdType.enemy);
         ColliderTwoD collider = new ColliderTwoD(new List<Base.Collision.ICollisionBound2D> { BCBC });
         RigidBody2D RB2D = new RigidBody2D(weight * health, false, false, 1);
         HealthComponent HC = new HealthComponent(health);
         CombatComponent CBC = new CombatComponent(damage * health, Utility.CombatIdType.enemy);
         PointsComponent pointsComponent = new PointsComponent(10 * health);
         MergeComponent MSC = new MergeComponent(0,5,new Base.Utility.EngineVector2());

         scene.AddComponent(aiEntity, t);
         scene.AddComponent(aiEntity, s);
         scene.AddComponent(aiEntity, AI);
         scene.AddComponent(aiEntity, collider);
         scene.AddComponent(aiEntity, RB2D);
         scene.AddComponent(aiEntity, HC);
         scene.AddComponent(aiEntity, CBC);
         scene.AddComponent(aiEntity, pointsComponent);
         scene.AddComponent(aiEntity, MSC);

         return aiEntity;
      }

      public static Entity CreateSpawnerEnemy(Scene scene, float x, float y)
      {
         Entity aiEntity = scene.CreateEntity();

         string spriteTextureName = "defaultTexture";
         if (ConfigService.config.SpriteSet == "Donuts and Sharks")
         {
            spriteTextureName = "whirlpool";
         }
         else
         {
            spriteTextureName = "SpawnerEnemy-geometry";
         }
         Sprite s = new Sprite(spriteTextureName);
         s.imageWidth = 50;
         s.imageHeight = 50;
         s.zOrder = 1;

         Transform t = new Transform(x, y, 1f, 1f, 0);
         Base.Collision.BoxCollisionBound2D BCBC = new Base.Collision.BoxCollisionBound2D(0, 0, 1, 1, (int)Utility.CombatIdType.spawnEnemy);
         ColliderTwoD collider = new ColliderTwoD(new List<Base.Collision.ICollisionBound2D> { BCBC });
         RigidBody2D RB2D = new RigidBody2D(.5f, true, false,1);
         HealthComponent HC = new HealthComponent(5);
         CombatComponent CBC = new CombatComponent(1, Utility.CombatIdType.spawnEnemy);
         PointsComponent pointsComponent = new PointsComponent(100);
         SpawningComponent spawner = new SpawningComponent(1, 1000);
         HealthBarComponent HBC = new HealthBarComponent(1);

         scene.AddComponent(aiEntity, t);
         scene.AddComponent(aiEntity, s);
         scene.AddComponent(aiEntity, collider);
         scene.AddComponent(aiEntity, RB2D);
         scene.AddComponent(aiEntity, HC);
         scene.AddComponent(aiEntity, CBC);
         scene.AddComponent(aiEntity, pointsComponent);
         scene.AddComponent(aiEntity, spawner);
         scene.AddComponent(aiEntity, HBC);

         return aiEntity;
      }

      public static Entity CreateChargingEnemy(Scene scene, float x, float y)
      {
         Entity aiEntity = scene.CreateEntity();

         string spriteTextureName = "defaultTexture";
         if (ConfigService.config.SpriteSet == "Donuts and Sharks")
         {
            spriteTextureName = "charging-shark";
         }
         else
         {
            spriteTextureName = "ChargingEnemy-Charging-geometry";
         }
         Sprite s = new Sprite(spriteTextureName);
         s.imageWidth = 30;
         s.imageHeight = 30;
         s.zOrder = 1;

         Transform t = new Transform(x, y, 1f, 1f, 0);

         ChargingAiComponent AI = new ChargingAiComponent();
         Transform playerTransform = new Transform();
         foreach (Entity e in scene.entityManager.GetAllEntities())
         {
            PlayerController pc = scene.GetComponent<PlayerController>(e);
            if (pc != null)
            {
               playerTransform = scene.GetComponent<Transform>(e);
               break;
            }
         }
         AI.targetTransform = playerTransform;

         Base.Collision.BoxCollisionBound2D BCBC = new Base.Collision.BoxCollisionBound2D(0, 0, 1, 1, (int)Utility.CombatIdType.charging);
         ColliderTwoD collider = new ColliderTwoD(new List<Base.Collision.ICollisionBound2D> { BCBC });
         RigidBody2D RB2D = new RigidBody2D(2, false, false, 1);
         HealthComponent HC = new HealthComponent(5);
         CombatComponent CBC = new CombatComponent(3, Utility.CombatIdType.enemy);
         PointsComponent pointsComponent = new PointsComponent(40);
         HealthBarComponent HBC = new HealthBarComponent(1);

         scene.AddComponent(aiEntity, t);
         scene.AddComponent(aiEntity, s);
         scene.AddComponent(aiEntity, AI);
         scene.AddComponent(aiEntity, collider);
         scene.AddComponent(aiEntity, RB2D);
         scene.AddComponent(aiEntity, HC);
         scene.AddComponent(aiEntity, CBC);
         scene.AddComponent(aiEntity, pointsComponent);
         scene.AddComponent(aiEntity, HBC);

         return aiEntity;
      }

      public static Entity CreateHammerHeadBoss(Scene scene, float x, float y)
      {
         Entity aiEntity = scene.CreateEntity();

         Sprite s = new Sprite("defaultTexture");
         s.imageWidth = 45;
         s.imageHeight = 45;
         s.zOrder = 1;

         Transform t = new Transform(x, y, 1f, 1f, 0);

         HammerBossComponent AI = new HammerBossComponent();
         AI.Speed = AI.BaseSpeed = 4;
         AI.SpeedIncrease = 8;
         AI.FastInterval = 2000;
         AI.RemainingInterval = 2000;
         AI.IsFast = false;
         AI.StunCooldown = 5000;
         AI.StunCooldownRemianing = 5000;

         Transform playerTransform = new Transform();
         foreach (Entity e in scene.entityManager.GetAllEntities())
         {
            PlayerController pc = scene.GetComponent<PlayerController>(e);
            if (pc != null)
            {
               playerTransform = scene.GetComponent<Transform>(e);
               break;
            }
         }
         AI.TargetTransform = playerTransform;


         Base.Collision.BoxCollisionBound2D BCBC = new Base.Collision.BoxCollisionBound2D(0, 0, 1, 1, (int)Utility.CombatIdType.boss);
         ColliderTwoD collider = new ColliderTwoD(new List<Base.Collision.ICollisionBound2D> { BCBC });
         RigidBody2D RB2D = new RigidBody2D(2, false, false, 1);
         HealthComponent HC = new HealthComponent(300);
         CombatComponent CBC = new CombatComponent(100, Utility.CombatIdType.enemy);
         PointsComponent pointsComponent = new PointsComponent(400);
         HealthBarComponent HBC = new HealthBarComponent(1);

         scene.AddComponent(aiEntity, t);
         scene.AddComponent(aiEntity, s);
         scene.AddComponent(aiEntity, AI);
         scene.AddComponent(aiEntity, collider);
         scene.AddComponent(aiEntity, RB2D);
         scene.AddComponent(aiEntity, HC);
         scene.AddComponent(aiEntity, CBC);
         scene.AddComponent(aiEntity, pointsComponent);
         scene.AddComponent(aiEntity, HBC);

         return aiEntity;
      }

      public static Entity CreateSquidBoss(Scene scene, float x, float y, int spawnAmount, bool isRight)
      {
         Entity aiEntity = scene.CreateEntity();

         Sprite s = new Sprite("defaultTexture");
         s.imageWidth = 200;
         s.imageHeight = 200;
         s.zOrder = 1;

         Transform t = new Transform(x, y, 1f, 1f, 0);

         SquidBossAiComponent AI = new SquidBossAiComponent();
         AI.Speed = 4;
         AI.IsUpward = true;
         AI.SpawnAmount = spawnAmount;
         AI.SpawnDelayTime = AI.RemainingSpawnDelay = 500;
         AI.IsRight = isRight;

         Base.Collision.BoxCollisionBound2D BCBC = new Base.Collision.BoxCollisionBound2D(0, 0, 1, 1, (int)Utility.CombatIdType.boss);
         ColliderTwoD collider = new ColliderTwoD(new List<Base.Collision.ICollisionBound2D> { BCBC });
         RigidBody2D RB2D = new RigidBody2D(2, true, false, 1);
         HealthComponent HC = new HealthComponent(800);
         CombatComponent CBC = new CombatComponent(100, Utility.CombatIdType.enemy);
         PointsComponent pointsComponent = new PointsComponent(400);
         HealthBarComponent HBC = new HealthBarComponent(1);

         scene.AddComponent(aiEntity, t);
         scene.AddComponent(aiEntity, s);
         scene.AddComponent(aiEntity, AI);
         scene.AddComponent(aiEntity, collider);
         scene.AddComponent(aiEntity, RB2D);
         scene.AddComponent(aiEntity, HC);
         scene.AddComponent(aiEntity, CBC);
         scene.AddComponent(aiEntity, pointsComponent);
         scene.AddComponent(aiEntity, HBC);

         return aiEntity;
      }
   }
}
