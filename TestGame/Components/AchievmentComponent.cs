using Base.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestGame.Model;

namespace TestGame.Components
{
   [Serializable]
   public class AchievmentComponent : Component<AchievmentComponent>
   {
      public float TimeRemaining { get; set; }
      public AchievmentModel Model { get; set; }
      public AchievmentComponent(float timeRemaining, AchievmentModel model)
      {
         this.TimeRemaining = timeRemaining;
         this.Model = model;
      }

      public AchievmentComponent()
      {
         this.TimeRemaining = 0;
         this.Model = null;
      }
   }
}
