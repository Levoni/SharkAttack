using Base.Components;
using Base.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestGame.Components
{
   [Serializable]
   public class ChargingAiComponent : AIComponent
   {
      public AiChargeState state;
      public int chargeTime;
      public int remainingChargeTime;
      public int StunTime;
      public int remainingStunTime;
      public EngineVector2 movement;

      public ChargingAiComponent() : base()
      {
         type = Utility.AiType.charging;
         state = AiChargeState.charging;
         speed = 8;
         chargeTime = remainingChargeTime = 3000;
         StunTime = remainingStunTime = 2000;
         movement = new EngineVector2();
      }
   }

   [Serializable]
   public enum AiChargeState
   {
      ready,
      charging,
      moving,
      stunned
   }
}
