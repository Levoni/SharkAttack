using Base.Components;
using Base.Entities;
using Base.Scenes;
using Base.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestGame.Components;
using TestGame.Events;

namespace TestGame.System
{
   [Serializable]
   public class LifeFountainSystem : EngineSystem
   {

      public LifeFountainSystem(Scene s)
      {
         systemSignature = (uint)(1 << LifeFountainComponent.GetFamily() | 1 << Transform.GetFamily() | 1 << Sprite.GetFamily());
         registeredEntities = new List<Entity>();
         Init(s);
      }

      public LifeFountainSystem()
      {
         systemSignature = (uint)(1 << LifeFountainComponent.GetFamily() | 1 << Transform.GetFamily() | 1 << Sprite.GetFamily());
         registeredEntities = new List<Entity>();
      }

      public override void Init(Scene s)
      {
         RegisterScene(s);
      }

      public override void Update(int dt)
      {
         base.Update(dt);
         foreach (Entity e in registeredEntities)
         {
            LifeFountainComponent fountain = parentScene.GetComponent<LifeFountainComponent>(e);
            Transform transform = parentScene.GetComponent<Transform>(e);
            fountain.remainingCooldowm -= dt;
            if(fountain.remainingCooldowm <= 0)
            {
               Burst(fountain.effectiveDistance, e, fountain.targetEntity, fountain.healAmount);
               fountain.remainingCooldowm = fountain.burstCooldown;
            }

         }
      }

      private void Burst(float distance, Entity entityHealihng, Entity entityToHeal, float healAmount)
      {
         Transform fromTransform = parentScene.GetComponent<Transform>(entityHealihng);
         Transform toTransform = parentScene.GetComponent<Transform>(entityToHeal);
         double xDiff = toTransform.X - fromTransform.X;
         double yDiff = toTransform.Y - fromTransform.Y;
         float diff = (float)Math.Sqrt(xDiff * xDiff + yDiff * yDiff);
         if(diff <= distance)
         {
            HealthComponent HC = parentScene.GetComponent<HealthComponent>(entityToHeal);
            var oldHCAmount = HC.health;
            HC.health += healAmount;
            if(HC.health > HC.maxHealth)
            {
               HC.health = HC.maxHealth;
            }
            parentScene.bus.Publish(this, new HealthChangeEvent(oldHCAmount, HC.health));
         }
      }
   }
}
