using Base.Utility.Services;
using Base.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestGame.Model;
using TestGame.Utility;
using TestGame.Utility.Services;

namespace TestGame.Components
{
   [Serializable]
   public class TrinketComponent:Component<TrinketComponent>
   {
      public int CurrentIndex { get; set; }
      public List<SecendaryWeaponModel> Trinkets { get; set; }
      public Dictionary<TrinketType, int> TrinketAmount { get; set; }
      public float MaxWeight { get; set; }
      public float currentWeight { get; set; }

      public TrinketComponent()
      {
         CurrentIndex = -1;
         Trinkets = new List<SecendaryWeaponModel>();
         //remainingCooldownp = new Dictionary<TrinketType, float>();
      }

      public TrinketComponent(List<SecendaryWeaponModel> secendaryWeapons)
      {
         Trinkets = new List<SecendaryWeaponModel>();
         TrinketAmount = new Dictionary<TrinketType, int>();
         var weightStat = ((TestGameSaveFile)SaveService.Save).Stats.Find(x => x.ItemName == "Secenday Weapon Limit");
         var weightLimitInfo = ShopService.Stats.Find(x => x.ItemName == "Secenday Weapon Limit");
         var maxWeightLimit = weightLimitInfo.BaseValue + weightLimitInfo.ValueIncrease * weightStat.Level;
         MaxWeight = maxWeightLimit;
         currentWeight = 0;
         //remainingCooldownp = new Dictionary<TrinketType, float>();
         foreach (SecendaryWeaponModel swm in secendaryWeapons)
         {
            if (swm != null)
            {
               Trinkets.Add(swm);
               TrinketAmount.Add(swm.TrinketType, 0);
               while (currentWeight < maxWeightLimit)
               {
                  TrinketAmount[swm.TrinketType] += 1;
                  currentWeight += swm.Weight;
               }
               if(currentWeight > maxWeightLimit)
               {
                  TrinketAmount[swm.TrinketType] -= 1;
                  currentWeight -= swm.Weight;
               }
            }
         }
         if (Trinkets.Count > 0)
         {
            CurrentIndex = 0;
         }
         else
         {
            CurrentIndex = -1;
         }
      }
   }
}
