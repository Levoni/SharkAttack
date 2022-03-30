using Base.Components;
using Base.Entities;
using Base.Events;
using Base.Scenes;
using Base.System;
using Base.Utility.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestGame.Components;
using TestGame.Events;
using TestGame.Model;
using TestGame.Utility;

namespace TestGame.System
{
   [Serializable]
   public class AbilitySystem:EngineSystem
   {
      public AbilitySystem(Scene s)
      {
         systemSignature = (uint)(1 << AbilityComponent.GetFamily() | 1 << Transform.GetFamily() | 1 << Sprite.GetFamily());
         registeredEntities = new List<Entity>();
         Init(s);
      }

      public AbilitySystem()
      {
         systemSignature = (uint)(1 << AbilityComponent.GetFamily() | 1 << Transform.GetFamily() | 1 << Sprite.GetFamily());
         registeredEntities = new List<Entity>();
      }

      public override void Init(Scene s)
      {
         RegisterScene(s);
         parentScene.bus.Subscribe(new EHandler<ControlEvent>(new Action<object, ControlEvent>(HandleAbilityButtonClick)));
      }

      public override void Update(int dt)
      {
         foreach (Entity e in registeredEntities)
         {
            AbilityComponent ac = parentScene.GetComponent<AbilityComponent>(e);
            if (ac.CurrentIndex != -1)
            {
               AbilityModel am = ac.abilities[ac.CurrentIndex];
               ac.remainingCooldownp[am.AbilityType] -= dt;
               if (ac.remainingCooldownp[am.AbilityType] < 0)
                  ac.remainingCooldownp[am.AbilityType] = 0;
               var abilityLevel = ((TestGameSaveFile)SaveService.Save).Abilities.Find(x => x.ItemName == am.ItemName);
               parentScene.bus.Publish(this, new AbilityCooldownChangeEvent(am.AbilityType, am.BaseCooldown - Math.Min(0, am.CooldownReduction * abilityLevel.Level), ac.remainingCooldownp[am.AbilityType]));
            }
         }
      }
      
      public void HandleAbilityButtonClick(object sender, ControlEvent e) // TODO: make theis a controll press handler so only player sends control
      {
         if(e.controlType == Base.Utility.Enums.ControlType.ability1 && e.state == Base.Utility.Enums.gameControlState.keyDown)
         {
            foreach(Entity entity in registeredEntities)
            {
               if (entity == e.e)
               {
                  AbilityComponent ac = parentScene.GetComponent<AbilityComponent>(entity);
                  if (ac.CurrentIndex != -1)
                  {
                     AbilityModel am = ac.abilities[ac.CurrentIndex];
                     Transform t = parentScene.GetComponent<Transform>(entity);
                     if (ac.remainingCooldownp[am.AbilityType] <= 0)
                     {
                        if (am.AbilityType == AbilityType.leechShot)
                        {
                           parentScene.bus.Publish(this, new LeechShotEvent(e.e));
                           var abilityLevel = ((TestGameSaveFile)SaveService.Save).Abilities.Find(x => x.ItemName == am.ItemName);
                           ac.remainingCooldownp[am.AbilityType] = am.BaseCooldown - Math.Min(0, am.CooldownReduction * abilityLevel.Level);
                           parentScene.bus.Publish(this, new AbilityCooldownChangeEvent(AbilityType.leechShot, ac.remainingCooldownp[am.AbilityType], ac.remainingCooldownp[am.AbilityType]));
                        }
                        else if (am.AbilityType == AbilityType.shockwave)
                        {
                           parentScene.bus.Publish(this, new ShockwaveEvent(new Base.Utility.EngineVector2(t.X,t.Y), 250));
                           var abilityLevel = ((TestGameSaveFile)SaveService.Save).Abilities.Find(x => x.ItemName == am.ItemName);
                           ac.remainingCooldownp[am.AbilityType] = am.BaseCooldown - Math.Min(0, am.CooldownReduction * abilityLevel.Level);
                           parentScene.bus.Publish(this, new AbilityCooldownChangeEvent(AbilityType.shockwave, ac.remainingCooldownp[am.AbilityType], ac.remainingCooldownp[am.AbilityType]));
                        }
                     }
                  }
               }
            }
         }
      }
   }
}
