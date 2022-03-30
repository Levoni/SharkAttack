using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestGame.Utility;

namespace TestGame.Model
{
   [Serializable]
   public class WeaponModel
   {
      public string ItemDescription { get; set; }
      public string ImageReference { get; set; }
      public string ItemName { get; set; }
      public float BaseDamage { get; set; }
      public float BasePrice { get; set; }
      public float DamageIncrease { get; set; }
      public float PriceIncrease { get; set; }
      public float CooldownAmount { get; set; }
      public GunType GunType { get; set; }
        public override string ToString()
        {
            return ItemName;
        }
    }
}
