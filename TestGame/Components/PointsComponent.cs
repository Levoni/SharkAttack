using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Base.Components;

namespace TestGame.Components
{
   [Serializable]
   public class PointsComponent:Component<PointsComponent>
   {
      public float pointValue;

      public PointsComponent()
      {
         this.pointValue = 0;
      }

      public PointsComponent(float points)
      {
         this.pointValue = points;
      }
   }
}
