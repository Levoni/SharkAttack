using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestGame.Model
{
   [Serializable]
   public class InventoryItemModel
   {
      public string ItemDescription { get; set; }
      public string ImageReference { get; set; }
      public string ItemName { get; set; }
      public int Level { get; set; }
      public override string ToString()
      {
         return ItemName;
      }
   }
}
