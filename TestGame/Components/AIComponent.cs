using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Base.Components;
using TestGame.Utility;

namespace TestGame.Components
{
   [Serializable]
   public class AIComponent:Component<AIComponent>
   {
      public Transform targetTransform;
      public float speed;
      public AiType type;

      public AIComponent():base()
      {
         targetTransform = new Transform();
         speed = 1;
         type = AiType.none;
      }

      public AIComponent(Transform target, float speed, AiType type)
      {
         this.targetTransform = target;
         this.speed = speed;
         this.type = type;
      }
   }
}
