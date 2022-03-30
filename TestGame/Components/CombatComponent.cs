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
   public class CombatComponent:Component<CombatComponent>
   {
      public float damage;
      public CombatIdType id;

      public CombatComponent()
      {
         this.damage = 1;
         this.id = CombatIdType.universal;
      }

      public CombatComponent(float damage, CombatIdType id)
      {
         this.damage = damage;
         this.id = id;
      }
   }
}
