using Base.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestGame.Utility;

namespace TestGame.Events
{
   [Serializable]
   public class GunChangeEvent:Event
   {
      public GunType currentGun;

      public GunChangeEvent()
      {
         currentGun = GunType.pistol;
      }

      public GunChangeEvent(GunType gun)
      {
         this.currentGun = gun;
      }
   }
}
