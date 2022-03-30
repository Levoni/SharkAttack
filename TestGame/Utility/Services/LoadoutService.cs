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
   public static class LoadoutService
   {
      public static bool isInitialized = false;
      public static SavedLoadoutModel Loadout { get; set; }

      public static void LoadLoadout()
      {
         isInitialized = true;
         if (DirectoryService.DoesFileExist("loadout.loadout"))
         {
            Loadout = (SavedLoadoutModel)BSerializer.deserializeObject("loadout", "loadout", "");
         }
         else
         {
            Loadout = new SavedLoadoutModel();
         }
      }

      public static void SaveLoadout()
      {
         BSerializer.serializeObject("loadout", "loadout", Loadout);
      }
   }
}
