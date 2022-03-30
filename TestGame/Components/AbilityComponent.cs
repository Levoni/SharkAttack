using Base.Components;
using Base.System;
using Base.Utility.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestGame.Model;
using TestGame.Utility;

namespace TestGame.Components
{
   [Serializable]
   public class AbilityComponent : Component<AbilityComponent>
   {
      //TODO: add max cooldown dictionary so we don't have to calculate it everytime
      public int CurrentIndex { get; set; }
      public List<AbilityModel> abilities { get; set; }
      public Dictionary<AbilityType, float> remainingCooldownp { get; set; }

      public AbilityComponent()
      {
         CurrentIndex = -1;
         abilities = new List<AbilityModel>();
         remainingCooldownp = new Dictionary<AbilityType, float>();
      }

      public AbilityComponent(List<AbilityModel> abilities)
      {
         this.abilities = new List<AbilityModel>();
         remainingCooldownp = new Dictionary<AbilityType, float>();
         foreach(AbilityModel am in abilities)
         {
            if (am != null)
            {
               this.abilities.Add(am);
               var abilityLevel = ((TestGameSaveFile)SaveService.Save).Abilities.Find(x => x.ItemName == am.ItemName);
               float cooldown = 0; //am.BaseCooldown - Math.Min(0, am.CooldownReduction * abilityLevel.Level);
               remainingCooldownp.Add(am.AbilityType, cooldown);
            }
         }
         if(this.abilities.Count > 0)
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
