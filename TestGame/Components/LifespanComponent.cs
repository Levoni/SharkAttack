using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Base.Components;

namespace TestGame.Components
{
   [Serializable]
   public class LifespanComponent : Component<LifespanComponent>
   {
      public int lifespanInMiliseconds;

      public LifespanComponent()
      {
         lifespanInMiliseconds = 1000;
      }

      public LifespanComponent(int lifespanInMiliseconds)
      {
         this.lifespanInMiliseconds = lifespanInMiliseconds;
      }
   }
}
