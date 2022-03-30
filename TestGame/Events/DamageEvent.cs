using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Base.Entities;

namespace TestGame.Events
{
   [Serializable]
   public class DamageEvent
   {
      public Entity attacker;
      public Entity defender;
      public float damage;

      public DamageEvent(Entity attacker, Entity defender, float damage)
      {
         this.attacker = attacker;
         this.defender = defender;
         this.damage = damage;
      }
   }
}
