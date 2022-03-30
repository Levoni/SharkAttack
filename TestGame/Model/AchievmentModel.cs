using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestGame.Model
{
   [Serializable]
   public class AchievmentModel
   {
      public string Name { get; set; }
      public string Description { get; set; }
      public string ImageReference { get; set; }
      public bool hasAchievement { get; set; }
   }
}
