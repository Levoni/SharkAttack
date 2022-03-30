using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestGame.Model;
using static TestGame.Components.UpgradeMenu;

namespace TestGame.Events
{
   [Serializable]
   public class ItemPurchesedEvent
   {
      public InventoryItemModel ItemModel { get; set; }
      public shopTypeEnum ItemType { get; set; }

      public ItemPurchesedEvent(InventoryItemModel Item, shopTypeEnum type)
      {
         this.ItemModel = Item;
         this.ItemType = type;
      }

   }
}
