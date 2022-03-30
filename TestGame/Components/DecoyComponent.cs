using Base.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestGame.Components
{
   [Serializable]
   public class DecoyComponent:Component<DecoyComponent>
   {
      public float LifeTime { get; set; }
      public float RemainingLifeTime { get; set; }

      public DecoyComponent()
      {
         this.LifeTime = RemainingLifeTime = 10000;
      }

      public DecoyComponent(float lifeTime)
      {
         this.LifeTime = this.RemainingLifeTime = lifeTime;
      }
   }
}
