using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Base.Entities;
using TestGame.Utility;

namespace TestGame.Events
{
   [Serializable]
   public class TrinketShotEvent
   {
      public Entity shooter;
      public TrinketType type;
      public int BombsRemaining;

      public TrinketShotEvent()
      {
         type = TrinketType.bomb;
         shooter = null;
         BombsRemaining = 0;
      }

      public TrinketShotEvent(Entity shooter, int bombsRemaining, TrinketType trinketType)
      {
         type = trinketType;
         this.shooter = shooter;
         this.BombsRemaining = bombsRemaining;
      }
   }
}
