using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestGame.Events
{
   [Serializable]
   public class HealthChangeEvent
   {
      public float oldHealth;
      public float currentHealth;

      public HealthChangeEvent()
      {
         currentHealth = 0;
      }

      public HealthChangeEvent(float oldHealth, float health)
      {
         this.oldHealth = oldHealth;
         this.currentHealth = health;
      }
   }
}
