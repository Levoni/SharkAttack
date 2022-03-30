using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestGame.Events
{
   [Serializable]
   public class GameOverEvent
   {
      public float GameScore;
      public int Wave;


      public GameOverEvent(float score, int wave)
      {
         GameScore = score;
         this.Wave = wave;
      }
   }
}
