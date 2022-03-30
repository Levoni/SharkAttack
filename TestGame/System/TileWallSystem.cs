using Base.Components;
using Base.Entities;
using Base.Events;
using Base.Scenes;
using Base.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestGame.Components;

namespace TestGame.System
{
   class TileWallSystem: EngineSystem
   {
      public TileWallSystem(Scene s)
      {
         systemSignature = (uint)(1 << TileSprite.GetFamily());
         registeredEntities = new List<Entity>();
         Init(s);
      }

      public TileWallSystem()
      {
         systemSignature = (uint)(1 << TileSprite.GetFamily());
         registeredEntities = new List<Entity>();
      }

      public override void Init(Scene s)
      {
         RegisterScene(s);
         parentScene.bus.Subscribe(new EHandler<CollisionEvent>(new Action<object, CollisionEvent>(HandleCOllision)));;
      }

      private void HandleCOllision(object sender, CollisionEvent e)
      {
         for (int i = 0; i < registeredEntities.Count(); i++)
         {
            if (e.context.entity1.id == registeredEntities[i].id)
            {
               if (parentScene.GetComponent<BulletComponent>(e.context.entity2) != null)
               {
                  parentScene.DestroyEntity(e.context.entity2);
               }
            }
            else if (e.context.entity2.id == registeredEntities[i].id)
            {
               if (parentScene.GetComponent<BulletComponent>(e.context.entity1) != null)
               {
                  parentScene.DestroyEntity(e.context.entity1);
               }
            }
         }
      }
   }
}
