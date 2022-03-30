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
   public static class ConfigService
   {
      public static bool isInitialized = false;
      public static ConfigModel config;

      public static void LoadConfig()
      {
         if (DirectoryService.DoesFileExist("settings.config"))
         {

            config = (ConfigModel)BSerializer.deserializeObject("settings", "config");
         }
         else
         {
            config = new ConfigModel();
            BSerializer.serializeObject("settings", "config", config);
         }
         AudioService.SetBackgroundVolume(config.SoundVolume);
      }

      public static void SaveConfig()
      {
         if (config != null)
         {
            BSerializer.serializeObject("settings", "config", config);
         }
      }
   }
}
