using Base.Components;
using Base.Entities;
using Base.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestGame.Components
{
   [Serializable]
   public class LifeFountainComponent:Component<LifeFountainComponent>
   {
      public Entity targetEntity { get; set; }
      public float healAmount { get; set; }
      public float effectiveDistance { get; set; }

      public float burstCooldown { get; set; }
      public float remainingCooldowm { get; set; }

      public LifeFountainComponent()
      {
         targetEntity = null;
         effectiveDistance = 0;
      }

      public LifeFountainComponent(Entity entity, float distance = 50)
      {
         this.targetEntity = entity;
         this.effectiveDistance = distance;
         burstCooldown = remainingCooldowm = 100;
         healAmount = .5f;
      }
   }
}
