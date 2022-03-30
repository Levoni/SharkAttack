using Base.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestGame.Events
{
   [Serializable]
   public class ShockwaveEvent
   {
      public EngineVector2 Origin { get; set; }
      public float Radius { get; set; }

      public ShockwaveEvent()
      {
         Origin = new EngineVector2();
         Radius = 0;
      }

      public ShockwaveEvent(EngineVector2 origin, float radius)
      {
         this.Origin = origin;
         this.Radius = radius;
      }
   }
}
