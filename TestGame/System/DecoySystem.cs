using Base.Components;
using Base.Entities;
using Base.Scenes;
using Base.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestGame.Components;
using TestGame.Utility.Services;

namespace TestGame.System
{
   public class DecoySystem:EngineSystem
   {
      public DecoySystem(Scene s)
      {
         systemSignature = (uint)(1 << DecoyComponent.GetFamily() | 1 << Transform.GetFamily() | 1 << Sprite.GetFamily());
         registeredEntities = new List<Entity>();
         Init(s);
      }

      public DecoySystem()
      {
         systemSignature = (uint)(1 << DecoyComponent.GetFamily() | 1 << Transform.GetFamily() | 1 << Sprite.GetFamily());
         registeredEntities = new List<Entity>();
      }

      public override void Init(Scene s)
      {
         RegisterScene(s);
      }

      public override void Update(int dt)
      {
         base.Update(dt);
         for(int i = registeredEntities.Count - 1; i >= 0; i--)
         {
            DecoyComponent DC = parentScene.GetComponent<DecoyComponent>(registeredEntities[i]);
            Transform t = parentScene.GetComponent<Transform>(registeredEntities[i]);
            DC.RemainingLifeTime -= dt;
            if(DC.RemainingLifeTime <= 0)
            {
               PlayerTransformService.UnassignTransform(t);
               parentScene.DestroyEntity(registeredEntities[i]);
            }
         }
      }
   }
}
