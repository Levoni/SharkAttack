using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestGame.Events
{
   [Serializable]
   public class NewWaveEvent
   {
      public int curWave;

      public NewWaveEvent()
      {
         curWave = 1;
      }

      public NewWaveEvent(int wave)
      {
         curWave = wave;
      }
   }
}
