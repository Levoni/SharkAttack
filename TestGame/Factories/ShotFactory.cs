using Base.Animations;
using Base.Components;
using Base.Entities;
using Base.Scenes;
using Base.Utility;
using Base.Utility.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestGame.Components;
using TestGame.Events;
using TestGame.Utility;
using TestGame.Utility.Services;

namespace TestGame.Factories
{
   public static class ShotFactory
   {

      public static Entity SpawnBullet(Scene scene, EngineVector2 unitVector, Transform parentTransform, Sprite parentSprite, ShootingComponent parentShooting, float damage, float bulletSpeed = 750)
      {
         Random rand = new Random();
         //AudioService.PlaySoundEffect("bang");
         Entity bullet = scene.CreateEntity();
         Transform t = new Transform(parentTransform.X + (parentTransform.widthRatio * parentSprite.imageWidth * .5f), parentTransform.Y + (parentTransform.heightRatio * parentSprite.imageWidth * .5f), 1, 1, 0);
         float degreeRotation = 0f;
         float randomChance = rand.Next() % 100;
         var accuracyInfo = ShopService.Stats.Find(x => x.ItemName == "Acuraccy");
         var accuracyLevel = ((TestGameSaveFile)SaveService.Save).Stats.Find(x => x.ItemName == "Acuraccy").Level;
         if (randomChance / 100 > accuracyInfo.BaseValue + accuracyInfo.ValueIncrease * accuracyLevel)
         {
            degreeRotation += rand.Next() % 60 - 30;
            unitVector = EngineVector2.RotateAroundPoint(unitVector, new EngineVector2(), degreeRotation).ToUnitVector(); ;
         }
         t.rotation = (float)unitVector.GetDegreeRotation(); ;

         //Determin texture for shot
         string spriteTextureName = "defaultTexture";
         if (ConfigService.config.SpriteSet == "Donuts and Sharks")
         {
            int shotSkinChance = rand.Next() % 3;
            if (shotSkinChance == 0)
            {
               spriteTextureName = "sprinkle-light";
            }
            else if (shotSkinChance == 1)
            {
               spriteTextureName = "sprinkle-medium";
            }
            else
            {
               spriteTextureName = "sprinkle-dark";
            }
         }
         else
         {
            spriteTextureName = "button_default_none";
         }
         Sprite s = new Sprite(spriteTextureName);
         s.imageHeight = 15;
         s.imageWidth = 4;

         CombatComponent CC = new CombatComponent(damage, Utility.CombatIdType.shot);
         HealthComponent HC = new HealthComponent(1);
         BulletComponent BC = new BulletComponent(unitVector, bulletSpeed);
         RigidBody2D RB2D = new RigidBody2D(.1f, false, false, 0);
         RB2D.velocity = unitVector * bulletSpeed;
         Base.Collision.BoxCollisionBound2D BCB = new Base.Collision.BoxCollisionBound2D(0, 0, 1, 1, (int)CombatIdType.shot);
         ColliderTwoD C2D = new ColliderTwoD(new List<Base.Collision.ICollisionBound2D> { BCB });
         LifespanComponent LC = new LifespanComponent(5000);

         scene.AddComponent(bullet, t);
         scene.AddComponent(bullet, s);
         scene.AddComponent(bullet, CC);
         scene.AddComponent(bullet, HC);
         scene.AddComponent(bullet, BC);
         scene.AddComponent(bullet, RB2D);
         scene.AddComponent(bullet, C2D);
         scene.AddComponent(bullet, LC);

         scene.bus.Publish(parentTransform, new ShootEvent());

         return bullet;
      }

      public static Entity SpawnMachineGunBullet(Scene scene, EngineVector2 unitVector, Transform parentTransform, Sprite parentSprite, ShootingComponent parentShooting, float damage, float bulletSpeed = 750)
      {
         Random rand = new Random();
         //AudioService.PlaySoundEffect("bang");
         Entity bullet = scene.CreateEntity();
         Transform t = new Transform(parentTransform.X + (parentTransform.widthRatio * parentSprite.imageWidth * .5f), parentTransform.Y + (parentTransform.heightRatio * parentSprite.imageWidth * .5f), 1, 1, 0);
         float degreeRotation = 0f;
         float randomChance = rand.Next() % 100;
         var accuracyInfo = ShopService.Stats.Find(x => x.ItemName == "Acuraccy");
         var accuracyLevel = ((TestGameSaveFile)SaveService.Save).Stats.Find(x => x.ItemName == "Acuraccy").Level;
         if (randomChance / 100 > accuracyInfo.BaseValue + accuracyInfo.ValueIncrease * accuracyLevel)
         {
            degreeRotation += rand.Next() % 60 - 30;
            unitVector = EngineVector2.RotateAroundPoint(unitVector, new EngineVector2(), degreeRotation).ToUnitVector(); ;
         }
         t.rotation = (float)unitVector.GetDegreeRotation();

         //Determin texture for shot
         string spriteTextureName = "defaultTexture";
         if (ConfigService.config.SpriteSet == "Donuts and Sharks")
         {
            int shotSkinChance = rand.Next() % 3;
            if (shotSkinChance == 0)
            {
               spriteTextureName = "sprinkle-light";
            }
            else if (shotSkinChance == 1)
            {
               spriteTextureName = "sprinkle-medium";
            }
            else
            {
               spriteTextureName = "sprinkle-dark";
            }
         }
         else
         {
            spriteTextureName = "button_default_none";
         }
         Sprite s = new Sprite(spriteTextureName);
         s.imageHeight = 7;
         s.imageWidth = 2;

         CombatComponent CC = new CombatComponent(damage, Utility.CombatIdType.shot);
         HealthComponent HC = new HealthComponent(1);
         BulletComponent BC = new BulletComponent(unitVector, bulletSpeed);
         RigidBody2D RB2D = new RigidBody2D(.5f, false, false, 0);
         RB2D.velocity = unitVector * bulletSpeed;
         Base.Collision.BoxCollisionBound2D BCB = new Base.Collision.BoxCollisionBound2D(0, 0, 1, 1, (int)CombatIdType.shot);
         ColliderTwoD C2D = new ColliderTwoD(new List<Base.Collision.ICollisionBound2D> { BCB });
         LifespanComponent LC = new LifespanComponent(5000);

         scene.AddComponent(bullet, t);
         scene.AddComponent(bullet, s);
         scene.AddComponent(bullet, CC);
         scene.AddComponent(bullet, HC);
         scene.AddComponent(bullet, BC);
         scene.AddComponent(bullet, RB2D);
         scene.AddComponent(bullet, C2D);
         scene.AddComponent(bullet, LC);

         scene.bus.Publish(parentTransform, new ShootEvent());

         return bullet;
      }

      public static Entity SpawnSniperBullet(Scene scene, EngineVector2 unitVector, Transform parentTransform, Sprite parentSprite, ShootingComponent parentShooting,float damage, float bulletSpeed = 750)
      {
         Random rand = new Random();
         //AudioService.PlaySoundEffect("bang");
         Entity bullet = scene.CreateEntity();
         Transform t = new Transform(parentTransform.X + (parentTransform.widthRatio * parentSprite.imageWidth * .5f), parentTransform.Y + (parentTransform.heightRatio * parentSprite.imageWidth * .5f), 1, 1, 0);
         float degreeRotation = 0f;
         float randomChance = rand.Next() % 100;
         var accuracyInfo = ShopService.Stats.Find(x => x.ItemName == "Acuraccy");
         var accuracyLevel = ((TestGameSaveFile)SaveService.Save).Stats.Find(x => x.ItemName == "Acuraccy").Level;
         if (randomChance / 100 > accuracyInfo.BaseValue + accuracyInfo.ValueIncrease * accuracyLevel)
         {
            degreeRotation += rand.Next() % 60 - 30;
            unitVector = EngineVector2.RotateAroundPoint(unitVector, new EngineVector2(), degreeRotation).ToUnitVector(); ;
         }
         t.rotation = (float)unitVector.GetDegreeRotation();

         //Determin texture for shot
         string spriteTextureName = "defaultTexture";
         if (ConfigService.config.SpriteSet == "Donuts and Sharks")
         {
            int shotSkinChance = rand.Next() % 3;
            if (shotSkinChance == 0)
            {
               spriteTextureName = "sprinkle-light";
            }
            else if (shotSkinChance == 1)
            {
               spriteTextureName = "sprinkle-medium";
            }
            else
            {
               spriteTextureName = "sprinkle-dark";
            }
         }
         else
         {
            spriteTextureName = "button_default_none";
         }
         Sprite s = new Sprite(spriteTextureName);
         s.imageHeight = 30;
         s.imageWidth = 8;

         CombatComponent CC = new CombatComponent(damage, Utility.CombatIdType.shot);
         HealthComponent HC = new HealthComponent(10);
         BulletComponent BC = new BulletComponent(unitVector, bulletSpeed);
         RigidBody2D RB2D = new RigidBody2D(.5f, false, false, 0);
         RB2D.velocity = unitVector * bulletSpeed;
         Base.Collision.BoxCollisionBound2D BCB = new Base.Collision.BoxCollisionBound2D(0, 0, 1, 1, (int)CombatIdType.shot);
         ColliderTwoD C2D = new ColliderTwoD(new List<Base.Collision.ICollisionBound2D> { BCB });
         LifespanComponent LC = new LifespanComponent(5000);

         scene.AddComponent(bullet, t);
         scene.AddComponent(bullet, s);
         scene.AddComponent(bullet, CC);
         scene.AddComponent(bullet, HC);
         scene.AddComponent(bullet, BC);
         scene.AddComponent(bullet, RB2D);
         scene.AddComponent(bullet, C2D);
         scene.AddComponent(bullet, LC);

         scene.bus.Publish(parentTransform, new ShootEvent());

         return bullet;
      }

      public static void SpawnFirstStageBomb(Scene scene, EngineVector2 unitVector, Transform parentTransform, Sprite parentSprite)
      {
         Entity bomb = scene.CreateEntity();
         Transform t = new Transform(parentTransform.X + (parentTransform.widthRatio * parentSprite.imageWidth * .5f), parentTransform.Y + (parentTransform.heightRatio * parentSprite.imageWidth * .5f), 1, 1, 0);

         //Determin texture
         string spriteTextureName = "defaultTexture";
         if (ConfigService.config.SpriteSet == "Donuts and Sharks")
         {
               spriteTextureName = "bomb-pickup";
         }
         else
         {
            spriteTextureName = "Bomb-geometry";
         }
         Sprite s = new Sprite(spriteTextureName);
         s.imageHeight = 8;
         s.imageWidth = 8;

         BulletComponent BC = new BulletComponent(unitVector, 375);
         BombComponent bombComponent = new BombComponent();
         bombComponent.isInSecondStage = false;
         bombComponent.timeRemaining = 0;
         RigidBody2D RB2D = new RigidBody2D(.5f, false, false, 0);
         RB2D.velocity = unitVector * 375;


         scene.AddComponent(bomb, t);
         scene.AddComponent(bomb, s);
         scene.AddComponent(bomb, BC);
         scene.AddComponent(bomb, bombComponent);
         scene.AddComponent(bomb, RB2D);
      }

      public static void SpawnSecondStageBomb(Scene scene, Transform parentTransform, Sprite parentSprite)
      {
         Entity bomb = scene.CreateEntity();

         Transform t = new Transform(parentTransform.X + (parentTransform.widthRatio * parentSprite.imageWidth * .5f),
                                     parentTransform.Y + (parentTransform.heightRatio * parentSprite.imageHeight * .5f),
                                     0, 0, 0);

         BombComponent bc = new BombComponent();
         bc.isInSecondStage = true;
         bc.totalTime = bc.timeRemaining = 1000;
         bc.originalX = parentTransform.X + (parentTransform.widthRatio * parentSprite.imageWidth * .5f);
         bc.originalY = parentTransform.Y + (parentTransform.heightRatio * parentSprite.imageWidth * .5f);

         Sprite sprite = new Sprite("sprinkle-bomb");

         RigidBody2D RB2D = new RigidBody2D(10, true, false, 1);
         CombatComponent CC = new CombatComponent(100, Utility.CombatIdType.bomb);
         HealthComponent HC = new HealthComponent(10000);
         ColliderTwoD C2D = new ColliderTwoD(new List<Base.Collision.ICollisionBound2D>() { new Base.Collision.CircleCollisionBound2D(.5f, .5f, .5f, (int)CombatIdType.bomb) });

         scene.AddComponent(bomb, bc);
         scene.AddComponent(bomb, C2D);
         scene.AddComponent(bomb, t);
         scene.AddComponent(bomb, sprite);
         scene.AddComponent(bomb, CC);
         scene.AddComponent(bomb, HC);
         scene.AddComponent(bomb, RB2D);
      }

      public static void SpawnShockAttack(Scene scene, Transform parentTransform, Sprite parentSprite, EngineVector2 directionVector)
      {
         Entity shockAttack = scene.CreateEntity();

         Transform t = new Transform(parentTransform.X + (parentTransform.widthRatio * parentSprite.imageWidth * .5f) + ((parentSprite.imageHeight * .5f) * directionVector.X),
                                       parentTransform.Y + (parentTransform.heightRatio * parentSprite.imageHeight * .5f) + ((parentSprite.imageHeight * .5f) * directionVector.Y),
                                       1, 1, 0);

         Sprite sprite = new Sprite("explosion-geometry");
         sprite.imageWidth = 150;
         sprite.imageHeight = 150;
         t.X -= sprite.imageWidth * .5f;
         t.Y -= sprite.imageHeight * .5f;
         

         RigidBody2D RB2D = new RigidBody2D(100, true, false, 1);
         CombatComponent CC = new CombatComponent(2, Utility.CombatIdType.spawnEnemy);
         HealthComponent HC = new HealthComponent(1);
         ColliderTwoD C2D = new ColliderTwoD(new List<Base.Collision.ICollisionBound2D>() { new Base.Collision.CircleCollisionBound2D(.5f,.5f,.5f,(int)CombatIdType.spawnEnemy) });
         LifespanComponent lc = new LifespanComponent(200);

         scene.AddComponent(shockAttack, sprite);
         scene.AddComponent(shockAttack, t);
         scene.AddComponent(shockAttack, RB2D);
         scene.AddComponent(shockAttack, CC);
         scene.AddComponent(shockAttack, HC);
         scene.AddComponent(shockAttack, C2D);
         scene.AddComponent(shockAttack, lc);

      }
   }
}
