using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Base.Components;

namespace TestGame.Components
{
   [Serializable]
   public class HealthComponent:Component<HealthComponent>
   {
      public float health;
      public float maxHealth;

      public HealthComponent()
      {
         health = 10;
         maxHealth = 10;
      }

      public HealthComponent(float health)
      {
         this.health = health;
         this.maxHealth = health;
      }
   }
}
