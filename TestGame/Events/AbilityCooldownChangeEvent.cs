using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestGame.Utility;

namespace TestGame.Events
{
   [Serializable]
   public class AbilityCooldownChangeEvent
   {
      public AbilityType type { get; set; }
      public float maxCooldown { get; set; }
      public float cooldownLeft { get; set; }

      public AbilityCooldownChangeEvent()
      {
         type = AbilityType.leechShot;
         this.maxCooldown = 1;
         this.cooldownLeft = 0;
      }

      public AbilityCooldownChangeEvent(AbilityType abilityType, float maxCooldown, float cooldown)
      {
         type = abilityType;
         this.maxCooldown = maxCooldown;
         this.cooldownLeft = cooldown;
      }
   }
}
