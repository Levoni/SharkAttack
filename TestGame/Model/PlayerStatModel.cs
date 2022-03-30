using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestGame.Model
{
   [Serializable]
   public class PlayerStatModel
   {
      public string StatName { get; set; }
      public int StatValue { get; set; }
   }
}
