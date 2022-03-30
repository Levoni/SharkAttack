using Base.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestGame.Components
{
   [Serializable]
   public class HealthBarComponent:Component<HealthBarComponent>
   {
      public float noShowPercentage;

      public HealthBarComponent()
      {
         noShowPercentage = 100;
      }

      public HealthBarComponent(float noShowPercentage)
      {
         if (noShowPercentage >= 0 && noShowPercentage <= 100)
         {
            this.noShowPercentage = noShowPercentage;
         }
         else
         {
            this.noShowPercentage = 100;
         }
      }
   }
}
