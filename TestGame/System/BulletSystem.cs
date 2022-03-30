using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Base.System;
using Base.Scenes;
using Base.Entities;
using Base.Events;
using Base.Components;
using Base.Utility;
using TestGame.Components;


namespace TestGame.System
{
   [Serializable]
   public class BulletSystem:EngineSystem
   {
      public BulletSystem(Scene s)
      {
         systemSignature = (uint)(1 << BulletComponent.GetFamily());
         registeredEntities = new List<Entity>();
         Init(s);
      }

      public BulletSystem()
      {
         systemSignature = (uint)(1 << BulletComponent.GetFamily());
         registeredEntities = new List<Entity>();
      }

      public override void Init(Scene s)
      {
         RegisterScene(s);
         parentScene.bus.Subscribe(new EHandler<BoundryHitEvent>(new Action<object, BoundryHitEvent>(HandleBoundryHitEvent)));
      }

      public override void Update(int dt)
      {
         //for (int i = 0; i < registeredEntities.Count(); i++)
         //{
         //   BulletComponent BC = parentScene.GetComponent<BulletComponent>(registeredEntities[i]);

         //   EngineVector2 vector = new EngineVector2(BC.unitDirection.X * BC.speed, BC.unitDirection.Y * BC.speed);
         //   VelocityChangeEvent e = new VelocityChangeEvent(registeredEntities[i], vector);
         //   parentScene.bus.Publish(this,e);
         //}
      }

      public void HandleBoundryHitEvent(object sender, BoundryHitEvent e)
      {
         for (int i = 0; i < registeredEntities.Count; i++)
         {
            if(registeredEntities[i] == e.entity)
            {
               parentScene.DestroyEntity(registeredEntities[i]);
            }
         }
      }
   }
}
