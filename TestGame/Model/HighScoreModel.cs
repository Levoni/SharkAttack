using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestGame.Utility;

namespace TestGame.Model
{
   [Serializable]
   public class HighScoreModel
   {
      public List<Score> ScoreList { get; set; }

      public HighScoreModel(List<Score> scores)
      {
         ScoreList = scores;
      }
   }
}
