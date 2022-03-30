using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestGame.Utility;

namespace TestGame.Model
{
   [Serializable]
   public class AbilityModel
   {
      public string ItemDescription { get; set; }
      public string ImageReference { get; set; }
      public string ItemName { get; set; }
      public float BaseCooldown { get; set; }
      public float BasePrice { get; set; }
      public float CooldownReduction { get; set; }
      public float PriceIncrease { get; set; }
      public AbilityType AbilityType { get; set; }

        public override string ToString()
        {
            return ItemName;
        }
    }
}
