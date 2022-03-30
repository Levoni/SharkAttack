using Base.Components;
using Base.Entities;
using Base.Events;
using Base.Scenes;
using Base.System;
using Base.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestGame.Components;

namespace TestGame.System
{
   [Serializable]
   public class BombSystem : EngineSystem
   {
      public BombSystem(Scene s)
      {
         systemSignature = (uint)(1 << BombComponent.GetFamily() | 1 << Sprite.GetFamily() | 1 << Transform.GetFamily());
         registeredEntities = new List<Entity>();
         Init(s);
      }

      public BombSystem()
      {
         systemSignature = (uint)(1 << BombComponent.GetFamily() | 1 << Sprite.GetFamily() | 1 << Transform.GetFamily());
         registeredEntities = new List<Entity>();
      }

      public override void Init(Scene s)
      {
         RegisterScene(s);
         parentScene.bus.Subscribe(new EHandler<ControlEvent>(new Action<object, ControlEvent>(HandleModifier1Event)));
      }

      public override void Update(int dt)
      {
         base.Update(dt);
         foreach(Entity e in registeredEntities)
         {
            BombComponent bc = parentScene.GetComponent<BombComponent>(e);
            LifespanComponent lc = parentScene.GetComponent<LifespanComponent>(e);
            if(bc.isInSecondStage && lc == null)
            {
               Transform t = parentScene.GetComponent<Transform>(e);
               Sprite s = parentScene.GetComponent<Sprite>(e);
               t.widthRatio = t.heightRatio = (float)(bc.totalTime - bc.timeRemaining) / (float)bc.totalTime;
               t.X = bc.originalX - ((t.widthRatio * s.imageWidth) / 2f);
               t.Y = bc.originalY - ((t.heightRatio * s.imageHeight) / 2f);
               bc.timeRemaining -= dt;
               if (bc.timeRemaining <= 0)
               {
                  lc = new LifespanComponent(100);
                  parentScene.AddComponent(e, lc);
               }
            }
         }
      }
      private void HandleModifier1Event(object sender, ControlEvent e)
      {
         if (e.controlType == Enums.ControlType.modifier1)
         {
            for (int i = 0; i < registeredEntities.Count(); i++)
            {
               BombComponent bc = parentScene.GetComponent<BombComponent>(registeredEntities[i]);
               if (!bc.isInSecondStage)
               {
                  Transform t = parentScene.GetComponent<Transform>(registeredEntities[i]);
                  Sprite s = parentScene.GetComponent<Sprite>(registeredEntities[i]);


                  TestGame.Factories.ShotFactory.SpawnSecondStageBomb(parentScene, t, s);
                  parentScene.DestroyEntity(registeredEntities[i]);
               }
            }
         }
      }
   }
}
