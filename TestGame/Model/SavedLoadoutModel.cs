using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestGame.Model
{
   [Serializable]
   public class SavedLoadoutModel
   {
      public WeaponModel FirstWeapon { get; set; }
      public WeaponModel SecondWeapon { get; set; }
      public AbilityModel Ability { get; set; }
      public SecendaryWeaponModel Trinket { get; set; }
   }
}
