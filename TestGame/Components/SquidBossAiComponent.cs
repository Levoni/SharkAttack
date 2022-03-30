using Base.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestGame.Components
{
   public class SquidBossAiComponent : Component<SquidBossAiComponent>
   {
      public bool IsUpward { get; set; }
      public float Speed { get; set; }
      public int SpawnDelayTime { get; set; }
      public int RemainingSpawnDelay { get; set; }
      public int SpawnAmount { get; set; }
      public bool IsRight { get; set; }
   }
}
