using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Base.Utility;
using Base.Components;

namespace TestGame.Components
{
   [Serializable]
   public class MergeComponent:Component<MergeComponent>
   {
      public float pushSpeed;
      public int stunTime;
      public int remainingStunTime;
      public EngineVector2 pushUnit;


      public MergeComponent()
      {
         stunTime = remainingStunTime = 0;
         pushSpeed = 0;
         pushUnit = new EngineVector2();
      }

      public MergeComponent(int stunTime, int pushSpeed, EngineVector2 pushUnitVector)
      {
         this.remainingStunTime = this.stunTime = stunTime;
         this.pushSpeed = pushSpeed;
         this.pushUnit = pushUnitVector;
      }
   }
}
