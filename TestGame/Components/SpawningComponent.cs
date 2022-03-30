using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Base.Components;

namespace TestGame.Components
{
   [Serializable]
   public class SpawningComponent:Component<SpawningComponent>
   {
      public int spawnTime;
      public int remainingTime;
      public int spawnAmount;

      public SpawningComponent()
      {
         spawnAmount = 1;
         spawnTime = remainingTime = 1000;
      }

      public SpawningComponent(int spawnAmount, int spawnTime)
      {
         this.spawnAmount = spawnAmount;
         this.spawnTime = this.remainingTime = spawnTime;
      }
   }
}
