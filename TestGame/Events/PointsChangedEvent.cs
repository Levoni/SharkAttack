using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestGame.Events
{
   [Serializable]
   public class PointsChangedEvent
   {
      public float CurrentPoints;

      public PointsChangedEvent()
      {
         CurrentPoints = 0;
      }

      public PointsChangedEvent(float points)
      {
         this.CurrentPoints = points;
      }
   }
}
