using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestGame.Model
{
   [Serializable]
   public class StatModel
   {
      public string ItemName { get; set; }
      public string ItemDescription { get; set; }
      public string ImageReference { get; set; }
      public float BaseValue { get; set; }
      public float ValueIncrease { get; set; }
      public float BasePrice { get; set; }
      public float PriceIncrease { get; set; }
      public override string ToString()
      {
         return ItemName;
      }
   }
}
