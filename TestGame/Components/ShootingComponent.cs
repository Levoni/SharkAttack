using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Base.Components;
using TestGame.Model;
using TestGame.Utility;

namespace TestGame.Components
{
   [Serializable]
   public class ShootingComponent:Component<ShootingComponent>
   {
      public int currentGunTypeIndex;
      public List<WeaponModel> guns;
      public Dictionary<GunType,float> remainingTimes;

      public ShootingComponent()
      {
         currentGunTypeIndex = 0;
      }

      public ShootingComponent(List<WeaponModel> weapons)
      {
         this.guns = new List<WeaponModel>();
         this.remainingTimes = new Dictionary<GunType, float>();
         foreach(WeaponModel wm in weapons)
         {
            if (wm != null && !this.remainingTimes.ContainsKey(wm.GunType))
            {
               this.guns.Add(wm);
               this.remainingTimes.Add(wm.GunType, wm.CooldownAmount);
            }
         }
         if (guns.Count > 0)
            this.currentGunTypeIndex = 0;
         else
            this.currentGunTypeIndex = -1;
      }
   }
}