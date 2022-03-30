using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestGame.Utility
{
   [Serializable]
   public enum CombatIdType
   {
      player,
      enemy,
      shot,
      spawnEnemy,
      charging,
      boss,
      bomb,
      universal
   }
   [Serializable]
   public enum GunType
   {
      pistol,
      shotgun,
      sniper,
      machinegun,
      burst,
   }

   public enum AbilityType
   {
      leechShot,
      shockwave
   }

   public enum TrinketType
   {
      bomb,
      decoy,
      lifeFountain
   }

   [Serializable]
   public enum AiType
   {
      zombie,
      charging,
      hammerBoss,
      none
   }
   [Serializable]
   public enum pickupType
   {
      health,
      bomb,
      shotgun,
      sniper
   }
}
