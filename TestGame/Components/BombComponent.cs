using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Base.Components;

namespace TestGame.Components
{
   [Serializable]
   public class BombComponent:Component<BombComponent>
   {
      public bool isInSecondStage;
      public int timeRemaining;
      public int totalTime;
      public float originalX;
      public float originalY;
   }
}
