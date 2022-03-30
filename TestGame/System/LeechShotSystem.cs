using Base.Components;
using Base.Entities;
using Base.Events;
using Base.Scenes;
using Base.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestGame.Components;
using TestGame.Events;
using TestGame.Model;

namespace TestGame.System
{
   [Serializable]
   public class LeechShotSystem:EngineSystem
   {
      EHandler<DamageEvent> leechDamageEvent;
      public Entity player { get; set; }
      public LeechShotSystem(Scene s)
      {
         systemSignature = (uint)(1 << LeechShotComponent.GetFamily());
         registeredEntities = new List<Entity>();
         Init(s);
      }

      public LeechShotSystem()
      {
         systemSignature = (uint)(1 << LeechShotComponent.GetFamily());
         registeredEntities = new List<Entity>();
      }

      public override void Init(Scene s)
      {
         RegisterScene(s);
         leechDamageEvent = new EHandler<DamageEvent>(new Action<object, DamageEvent>(HandleDamageEvent));
         parentScene.bus.Subscribe(leechDamageEvent);
      }

      public override void Terminate()
      {
         base.Terminate();
         parentScene.bus.Unsubscribe(leechDamageEvent);
      }

      public override void Update(int dt)
      {

      }

      private void HandleDamageEvent(object sender, DamageEvent e)
      {
         foreach(Entity entity in registeredEntities)
         {
            if(entity == e.attacker)
            {
               if (e.damage > 0)
               {
                  var playerHealthComponent = parentScene.GetComponent<HealthComponent>(player);
                  var oldPlayerHealth = playerHealthComponent.health;
                  playerHealthComponent.health += e.damage;
                  if (playerHealthComponent.health > playerHealthComponent.maxHealth)
                  {
                     playerHealthComponent.health = playerHealthComponent.maxHealth;
                  }
                  parentScene.bus.Publish(this, new HealthChangeEvent(oldPlayerHealth, playerHealthComponent.health));
               }
            }
         }
      }
   }
}
