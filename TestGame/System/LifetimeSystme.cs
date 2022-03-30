using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Base.System;
using Base.Scenes;
using Base.Components;
using TestGame.Components;
using Base.Entities;

namespace TestGame.System
{
   [Serializable]
   public class LifetimeSystem:EngineSystem
   {
      public LifetimeSystem(Scene s)
      {
         systemSignature = (uint)(1 << LifespanComponent.GetFamily());
         registeredEntities = new List<Entity>();
         Init(s);
      }

      public LifetimeSystem()
      {
         systemSignature = (uint)(1 << LifespanComponent.GetFamily());
         registeredEntities = new List<Entity>();
      }

      public override void Init(Scene s)
      {
         RegisterScene(s);
      }

      public override void Update(int dt)
      {
         for (int i = 0; i < registeredEntities.Count(); i++)
         {
            LifespanComponent LC = parentScene.GetComponent<LifespanComponent>(registeredEntities[i]);
            LC.lifespanInMiliseconds -= dt;
            if(LC.lifespanInMiliseconds <= 0)
            {
               parentScene.DestroyEntity(registeredEntities[i]);
            }
         }
      }

   }
}
