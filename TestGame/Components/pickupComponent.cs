using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Base.Components;
using TestGame.Utility;

namespace TestGame.Components
{
   [Serializable]
   public class PickupComponent:Component<PickupComponent>
   {
      public pickupType type;
      public int amount;

      public PickupComponent()
      {
         this.type = pickupType.health;
         this.amount = 0;
      }

      public PickupComponent(pickupType type, int amount)
      {
         this.type = type;
         this.amount = amount;
      }
   }
}
