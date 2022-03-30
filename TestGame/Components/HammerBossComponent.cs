using Base.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestGame.Components
{
   public class HammerBossComponent:Component<HammerBossComponent>
   {
      public Transform TargetTransform { get; set; }
      public float BaseSpeed { get; set; }
      public float Speed { get; set; }
      public float SpeedIncrease { get; set; }
      public int FastInterval { get; set; }
      public int SlowInterval { get; set; }
      public int RemainingInterval { get; set; }
      public bool IsFast { get; set; }
      public float ShockSize { get; set; }
      public int ShockDamage { get; set; }
      public int StunCooldown { get; set; }
      public int StunCooldownRemianing { get; set; }
   }
}
