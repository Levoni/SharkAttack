using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Base.Components;
using Base.Utility;
using Base.Entities;

namespace TestGame.Components
{
   [Serializable]
   public class BulletComponent:Component<BulletComponent>
   {
      public EngineVector2 unitDirection;
      public float speed;

      public BulletComponent()
      {
         unitDirection = new EngineVector2();
         speed = 1;
      }

      public BulletComponent(EngineVector2 unitDirection, float speed)
      {
         this.unitDirection = unitDirection;
         this.speed = speed;
      }
   }
}
