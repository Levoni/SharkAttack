using Base.Serialization;
using Base.Utility.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestGame.Model;

namespace TestGame.Utility.Services
{
   [Serializable]
   public static class PointsService
   {
      static List<Score> highScores;

      public static void Init()
      {
         highScores = new List<Score>();
         LoadHighScores();
      }

      public static bool CheckForHighScore(float score)
      {
         int highScoresIndex = -1;
         if (highScores.Count < 10)
         {
            highScoresIndex = highScores.Count;
         }
         for (int i = 0; i < highScores.Count - 1; i++)
         {
            if (score > highScores[i].score)
               highScoresIndex = i;
         }
         return highScoresIndex != -1;
      }

      public static void InsertHighScore(float score, string name, int wave)
      {
         int highScoresIndex = -1;
         if (highScores.Count < 10)
         {
            highScoresIndex = highScores.Count;
         }
         for (int i = highScores.Count - 1; i >= 0; i--)
         {
            if (score > highScores[i].score)
               highScoresIndex = i;
         }
         if (highScoresIndex != -1 && highScoresIndex < 10)
         {
            if (highScoresIndex == highScores.Count)
            {
               highScores.Add(new Score(score, name, wave));
            }
            else
            {
               if(highScores.Count < 10)
               {
                  highScores.Add(highScores[highScores.Count - 1]);
               }
               for (int i = highScores.Count - 1; i > highScoresIndex; i--)
               {
                  highScores[i] = highScores[i - 1];
               }
               highScores[highScoresIndex] = new Score(score, name, wave);
            }
         }
      }

      public static List<Score> GetHighScores()
      {
         return highScores;
      }

      public static void LoadHighScores()
      {
         if (DirectoryService.DoesFileExist("HighScores.bin"))
         {
            HighScoreModel scores = (HighScoreModel)BSerializer.deserializeObject("HighScores", "bin");
            highScores = scores.ScoreList;
         } 
      }

      public static void SaveHighScores()
      {
         HighScoreModel highScoresSave = new HighScoreModel(highScores);

         BSerializer.serializeObject("HighScores", "bin", highScoresSave);
      }
   }
}