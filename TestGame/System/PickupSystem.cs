using Base.Components;
using Base.Entities;
using Base.Events;
using Base.Scenes;
using Base.System;
using System;
using System.Collections.Generic;
using TestGame.Components;
using TestGame.Events;
using TestGame.Factories;
using TestGame.Utility;

namespace TestGame.System
{
   [Serializable]
   public class PickupSystem : EngineSystem
   {
      public float spawnInterval;
      public float remainingInterval;
      public int xMin, yMin, xMax, yMax;

      private Random rand;
      private int spawnChance;
      private bool isEnded;

      public PickupSystem(Scene s)
      {
         systemSignature = (uint)(1 << PickupComponent.GetFamily() | 1 << RigidBody2D.GetFamily() | 1 << ColliderTwoD.GetFamily() | 1 << Transform.GetFamily() | 1 << Sprite.GetFamily());
         registeredEntities = new List<Entity>();
         SetDefaults();
         Init(s);
      }

      public PickupSystem()
      {
         systemSignature = (uint)(1 << PickupComponent.GetFamily() | 1 << RigidBody2D.GetFamily() | 1 << ColliderTwoD.GetFamily() | 1 << Transform.GetFamily() | 1 << Sprite.GetFamily());
         registeredEntities = new List<Entity>();
         SetDefaults();
      }

      public override void Init(Scene s)
      {
         RegisterScene(s);
         parentScene.bus.Subscribe(new EHandler<CollisionEvent>(new Action<object, CollisionEvent>(HandleCollision)));
         parentScene.bus.Subscribe(new EHandler<GameOverEvent>(new Action<object, GameOverEvent>(HandleGameOver)));
      }

      public void SetDefaults()
      {
         spawnInterval = remainingInterval = 1000;
         xMin = yMin = 0;
         xMax = yMax = 0;
         rand = new Random();
         spawnChance = 10;
         isEnded = false;
      }

      public override void Update(int dt)
      {
         if (!isEnded)
         {
            remainingInterval -= dt;
            if (remainingInterval <= 0)
            {
               if (spawnChance > rand.Next() % 100)
               {
                  int x = rand.Next() % (xMax - xMin);
                  int y = rand.Next() % (yMax - yMin);
                  if (rand.Next() % 100 < 80)
                  {
                     PickupFactory.createPickup(parentScene, x, y, pickupType.health);
                  }
                  else
                  {
                     PickupFactory.createPickup(parentScene, x, y, pickupType.bomb);
                  }
               }
               remainingInterval = spawnInterval;
            }
         }
      }

      private void HandleCollision(object sender, CollisionEvent e)
      {
         for (int i = 0; i < registeredEntities.Count; i++)
         {
            if (e.context.entity1 == registeredEntities[i])
            {
               applyPickup(parentScene.GetComponent<PickupComponent>(registeredEntities[i]), e.context.entity2);
               parentScene.DestroyEntity(e.context.entity1);
            }
            else if (e.context.entity2 == registeredEntities[i])
            {
               applyPickup(parentScene.GetComponent<PickupComponent>(registeredEntities[i]), e.context.entity1);
               parentScene.DestroyEntity(e.context.entity2);
            }
         }
      }

      private void applyPickup(PickupComponent PUC, Entity player)
      {
         PlayerController PC = parentScene.GetComponent<PlayerController>(player);
         if (PC != null)
         {
            if (PUC.type == pickupType.health)
            {
               HealthComponent HC = parentScene.GetComponent<HealthComponent>(player);
               var oldHCAmount = HC.health;
               HC.health += PUC.amount;
               if (HC.health > HC.maxHealth)
               {
                  HC.health = HC.maxHealth;
               }
               parentScene.bus.Publish(this, new HealthChangeEvent(oldHCAmount, HC.health));

            }
            else if (PUC.type == pickupType.bomb)
            {
               TrinketComponent TC = parentScene.GetComponent<TrinketComponent>(player);
               if (TC.CurrentIndex != -1)
               {
                  TC.TrinketAmount[TC.Trinkets[TC.CurrentIndex].TrinketType] += PUC.amount;
                  TC.currentWeight += TC.Trinkets[TC.CurrentIndex].Weight;
                  if (TC.currentWeight > TC.MaxWeight)
                  {
                     TC.currentWeight -= PUC.amount;
                     TC.TrinketAmount[TC.Trinkets[TC.CurrentIndex].TrinketType]--;
                  }
                  parentScene.bus.Publish(this, new TrinketShotEvent(player, TC.TrinketAmount[TC.Trinkets[TC.CurrentIndex].TrinketType],TC.Trinkets[TC.CurrentIndex].TrinketType));
               }
            }
         }
      }
      public void HandleGameOver(object sender, GameOverEvent e)
      {
         isEnded = true;
         for (int i = registeredEntities.Count - 1; i >= 0; i--)
         {
            parentScene.DestroyEntity(registeredEntities[i]);
         }
      }
   }
}
