using Base.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestGame.Events
{
   [Serializable]
   public class LeechShotEvent:Base.Events.Event
   {
      public Entity e { get; set; }

      public LeechShotEvent()
      {

      }

      public LeechShotEvent(Entity entity)
      {
         e = entity;
      }
   }
}
