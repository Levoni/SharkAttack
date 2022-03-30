using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Base.System;
using TestGame.Components;
using Base.Scenes;
using Base.Entities;
using Base.Components;
using Base.Events;
using Base.Utility;

using TestGame.Utility;


namespace TestGame.System
{
   [Serializable]
   public class CombatSystem:EngineSystem
   {
      MaskCollection layerMask;
      //List<ComponentMask> damageMask;
      public CombatSystem(Scene s)
      {
         systemSignature = (uint)(1 << HealthComponent.GetFamily() | 1 << CombatComponent.GetFamily());
         registeredEntities = new List<Entity>();
         SetupMasks();
         Init(s);
      }

      public CombatSystem()
      {
         systemSignature = (uint)(1 << HealthComponent.GetFamily() | 1 << CombatComponent.GetFamily());
         registeredEntities = new List<Entity>();
         SetupMasks();
      }

      private void SetupMasks()
      {
         layerMask = new MaskCollection();
         Mask playerMask = new Mask();
         playerMask.SetBit((int)CombatIdType.enemy);
         playerMask.SetBit((int)CombatIdType.spawnEnemy);
         Mask enemyMask = new Mask();
         enemyMask.SetBit((int)CombatIdType.player);
         enemyMask.SetBit((int)CombatIdType.shot);
         Mask shotMask = new Mask();
         shotMask.SetBit((int)CombatIdType.enemy);
         shotMask.SetBit((int)CombatIdType.spawnEnemy);
         Mask SpawnerMask = new Mask();
         SpawnerMask.SetBit((int)CombatIdType.player);
         SpawnerMask.SetBit((int)CombatIdType.shot);
         layerMask.SetMaskValue((int)CombatIdType.player,playerMask);
         layerMask.SetMaskValue((int)CombatIdType.enemy, enemyMask);
         layerMask.SetMaskValue((int)CombatIdType.shot, shotMask);
         layerMask.SetMaskValue((int)CombatIdType.spawnEnemy, SpawnerMask);
      }

      public override void Init(Scene s)
      {
         RegisterScene(s);
         parentScene.bus.Subscribe(new EHandler<CollisionEvent>(new Action<object, CollisionEvent>(handleCollision)));
      }

      public override void Update(int dt)
      {
         for(int i = 0; i < registeredEntities.Count(); i++)
         {
            HealthComponent health = parentScene.GetComponent<HealthComponent>(registeredEntities[i]);
            if(health.health <= 0)
            {
               CombatComponent cc = parentScene.GetComponent<CombatComponent>(registeredEntities[i]);
               if (cc.id == Utility.CombatIdType.player)
                  parentScene.bus.Publish(this, new TestGame.Events.PlayerDiedEvent(parentScene));
               parentScene.bus.Publish(this, new ObjectDetroyedEvent(registeredEntities[i]));
               parentScene.DestroyEntity(registeredEntities[i]);
            }
         }
      }

      public void handleCollision(object sender, CollisionEvent collisionEvent)
      {
         if(registeredEntities.Contains(collisionEvent.context.entity1) &&
            registeredEntities.Contains(collisionEvent.context.entity2))
         {
            HealthComponent h1 = parentScene.GetComponent<HealthComponent>(collisionEvent.context.entity1);
            HealthComponent h2 = parentScene.GetComponent<HealthComponent>(collisionEvent.context.entity2);
            CombatComponent C1 = parentScene.GetComponent<CombatComponent>(collisionEvent.context.entity1);
            CombatComponent C2 = parentScene.GetComponent<CombatComponent>(collisionEvent.context.entity2);


            if(layerMask.isIdInMask((int)C1.id, (int)C2.id))
            {
               h2.health -= C1.damage;
               var damage = h2.health < 0 ? C1.damage + h2.health : C1.damage;
               parentScene.bus.Publish(this, new Events.DamageEvent(collisionEvent.context.entity1, collisionEvent.context.entity2, damage));
               if (C2.id == Utility.CombatIdType.player)
                  parentScene.bus.Publish(this, new Events.HealthChangeEvent(h2.health + C1.damage, h2.health));
            }
            if (layerMask.isIdInMask((int)C2.id, (int)C1.id))
            {
               h1.health -= C2.damage;
               var damage = h1.health < 0 ? C2.damage + h1.health : C2.damage;
               parentScene.bus.Publish(this, new Events.DamageEvent(collisionEvent.context.entity2, collisionEvent.context.entity1, damage));
               if (C1.id == Utility.CombatIdType.player)
                  parentScene.bus.Publish(this,new Events.HealthChangeEvent(h1.health + C2.damage, h1.health));
            }
         }
      }
   }
}
