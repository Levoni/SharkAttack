using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestGame.Utility;

namespace TestGame.Model
{
   [Serializable]
   public class SecendaryWeaponModel
   {
      public string ItemDescription { get; set; }
      public string ImageReference { get; set; }
      public string ItemName { get; set; }
      public float BasePrice { get; set; }
      public float PriceIncrease { get; set; }
      public float Weight { get; set; }
      public TrinketType TrinketType { get; set; }
        public override string ToString()
        {
            return ItemName;
        }
    }
}
