using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestGame.Utility
{
   //TODO: make sure wave saves correctly
   [Serializable]
   public class Score
   {
      public float score;
      public int wave;
      public string name;

      public Score()
      {
         score = 0;
         wave = 1;
         name = "anonymous";
      }

      public Score(float score, string name, int wave)
      {
         this.score = score;
         this.name = name;
         this.wave = wave;
      }
   }
}
