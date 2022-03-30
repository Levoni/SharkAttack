using Base.Scenes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestGame.Events
{
   [Serializable]
   public class PlayerDiedEvent
   {
      public Scene scene;

      public PlayerDiedEvent(Scene scene)
      {
         this.scene = scene;
      }
   }
}
