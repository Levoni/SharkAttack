using Base.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestGame.Model;

namespace TestGame.Utility.Services
{
   public static class ShopService
   {
      public static bool isInitialized = false;
      public static List<WeaponModel> Weapons { get; set; }
      public static List<SecendaryWeaponModel> SecedaryWeapons { get; set; }
      public static List<AbilityModel> Abilities { get; set; }
      public static List<StatModel> Stats { get; set; }

      public static void LoadShopInfo()
      {
         isInitialized = true;
         Weapons = (List<WeaponModel>) JSerializer.Deserialize<List<WeaponModel>>("Weapons", "txt", "./content/");
         SecedaryWeapons = (List<SecendaryWeaponModel>) JSerializer.Deserialize<List<SecendaryWeaponModel>>("SecendaryWeapons", "txt", "./content/");
         Abilities = (List<AbilityModel>) JSerializer.Deserialize<List<AbilityModel>>("Abillities", "txt", "./content/");
         Stats = (List<StatModel>)JSerializer.Deserialize<List<StatModel>>("Stats", "txt", "./content/");
      }
   }
}
