using Base.Serialization;
using Base.Utility.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestGame.Model;

namespace TestGame.Utility.Services
{
   public static class AchievmentService
   {
      public static bool isInitialized = false;
      public static Dictionary<string, AchievmentModel> Achievments { get; set; }
      public static Dictionary<string, PlayerStatModel> Stats { get; set; }

      public static void LoadAchievmentsAndStats()
      {
         isInitialized = true;
         LoadDefaults();
      }

      public static void LoadDefaults()
      {
         if (!DirectoryService.DoesFileExist("achievments.achievments"))
         {
            List<AchievmentModel> achievmentModels = Base.Serialization.Serializer<List<AchievmentModel>>.DeserializeFromFile("default.achievement", "Content/");
            Achievments = new Dictionary<string, AchievmentModel>();
            foreach (var achievment in achievmentModels)
            {
               Achievments.Add(achievment.Name, achievment);
            }
         } 
         else
         {
             Achievments =  (Dictionary<string,AchievmentModel>)BSerializer.deserializeObject("achievments", "achievments");
         }
         if (!DirectoryService.DoesFileExist("stats.stats"))
         {
            List<PlayerStatModel> statModels = Serializer<List<PlayerStatModel>>.DeserializeFromFile("stats.stat", "Content/");
            Stats = new Dictionary<string, PlayerStatModel>();
            foreach (var stat in statModels)
            {
               Stats.Add(stat.StatName, stat);
            }
         }
         else
         {
            Stats = (Dictionary<string, PlayerStatModel>)BSerializer.deserializeObject("stats", "stats");
         }
      }

      public static void SaveAchievmentsAndStats()
      {
         BSerializer.serializeObject("achievments", "achievments", Achievments);
         BSerializer.serializeObject("stats", "stats", Stats);
      }
   }
}
