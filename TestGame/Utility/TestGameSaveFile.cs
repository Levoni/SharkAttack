using Base.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestGame.Model;

namespace TestGame.Utility
{
   [Serializable]
   public class TestGameSaveFile : SaveFile
   {
      public int money { get; set; }

      public List<InventoryItemModel> Stats { get; set; }

      public List<InventoryItemModel> Weapons { get; set; }
      public List<InventoryItemModel> SecendaryWeapons { get; set; }
      public List<InventoryItemModel> Abilities { get; set; }

      public int CheckpointReached {get;set;}
      public bool BeatGame { get; set; }

      public TestGameSaveFile()
      {
         money = 58000;
         Stats = new List<InventoryItemModel>();
         Weapons = new List<InventoryItemModel>();
         SecendaryWeapons = new List<InventoryItemModel>();
         Abilities = new List<InventoryItemModel>();
         CheckpointReached = 1;
         BeatGame = false;
      }
   }
}
