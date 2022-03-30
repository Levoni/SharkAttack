using Base.Components;
using Base.Entities;
using Base.Scenes;
using Base.System;
using Base.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestGame.Components;
using Base.Utility;
using Base.Utility.Services;
using TestGame.Events;
using TestGame.Factories;

namespace TestGame.System
{
   public class TrinketSystem : EngineSystem
   {

      EHandler<ControlEvent> keyPressedEvent;
      public TrinketSystem(Scene s)
      {
         systemSignature = (uint)(1 << TrinketComponent.GetFamily() | 1 << Transform.GetFamily());
         registeredEntities = new List<Entity>();
         Init(s);
      }

      public TrinketSystem()
      {
         systemSignature = (uint)(1 << TrinketComponent.GetFamily() | 1 << Transform.GetFamily());
         registeredEntities = new List<Entity>();
      }

      public override void Init(Scene s)
      {
         RegisterScene(s);
         keyPressedEvent = new EHandler<ControlEvent>(new Action<object, ControlEvent>(HandleButtonPress));
         parentScene.bus.Subscribe(keyPressedEvent);
         //parentScene.bus.Subscribe(new EHandler<ControlAction2Event>(new Action<object, ControlAction2Event>(HandleAction2Press)));
         //parentScene.bus.Subscribe(new EHandler<ControlModifier2Event>(new Action<object, ControlModifier2Event>(HandleModifier2Press)));
      }

      public override void Terminate()
      {
         base.Terminate();
         parentScene.bus.Unsubscribe(keyPressedEvent);
      }

      public override void Update(int dt)
      {
      }

      public void HandleButtonPress(object sender, ControlEvent controlEvent)
      {
         if (controlEvent.controlType == Enums.ControlType.attack2 && controlEvent.state == Enums.gameControlState.keyDown)
         {
            foreach (Entity entity in registeredEntities)
            {
               TrinketComponent tc = parentScene.GetComponent<TrinketComponent>(entity);
               if (tc.CurrentIndex != -1)
               {
                  if (tc.TrinketAmount[tc.Trinkets[tc.CurrentIndex].TrinketType] > 0)
                  {
                     if (tc.Trinkets[tc.CurrentIndex].TrinketType == Utility.TrinketType.bomb)
                     {
                        Transform t = parentScene.GetComponent<Transform>(entity);
                        EngineVector2 curMousScreenPos = MouseService.GetMousePosition();
                        var curMouseWorldPos = CameraService.camera.ScreenToWorld(new Microsoft.Xna.Framework.Vector2(curMousScreenPos.X, curMousScreenPos.Y));
                        EngineVector2 unitVector = new EngineVector2(curMouseWorldPos.X - t.X, curMouseWorldPos.Y - t.Y).ToUnitVector();
                        Sprite s = parentScene.GetComponent<Sprite>(entity);

                        Factories.ShotFactory.SpawnFirstStageBomb(parentScene, unitVector, t, s);
                        tc.TrinketAmount[tc.Trinkets[tc.CurrentIndex].TrinketType]--;
                        tc.currentWeight -= tc.Trinkets[tc.CurrentIndex].Weight;
                        parentScene.bus.Publish(this, new TrinketShotEvent(entity, tc.TrinketAmount[tc.Trinkets[tc.CurrentIndex].TrinketType], tc.Trinkets[tc.CurrentIndex].TrinketType));
                     }
                     else if (tc.Trinkets[tc.CurrentIndex].TrinketType == Utility.TrinketType.decoy)
                     {
                        Transform t = parentScene.GetComponent<Transform>(entity);
                        Sprite s = parentScene.GetComponent<Sprite>(entity);
                        TrinketFactory.SpawnDummyEnitty(parentScene, t);
                        
                        tc.TrinketAmount[tc.Trinkets[tc.CurrentIndex].TrinketType]--;
                        tc.currentWeight -= tc.Trinkets[tc.CurrentIndex].Weight;
                        parentScene.bus.Publish(this, new TrinketShotEvent(entity, tc.TrinketAmount[tc.Trinkets[tc.CurrentIndex].TrinketType], tc.Trinkets[tc.CurrentIndex].TrinketType));
                     }
                     else if (tc.Trinkets[tc.CurrentIndex].TrinketType == Utility.TrinketType.lifeFountain)
                     {
                        Transform t = parentScene.GetComponent<Transform>(entity);
                        Sprite s = parentScene.GetComponent<Sprite>(entity);
                        TrinketFactory.SpawnLifeFountain(parentScene, t, entity);
                        tc.TrinketAmount[tc.Trinkets[tc.CurrentIndex].TrinketType]--;
                        tc.currentWeight -= tc.Trinkets[tc.CurrentIndex].Weight;
                        parentScene.bus.Publish(this, new TrinketShotEvent(entity, tc.TrinketAmount[tc.Trinkets[tc.CurrentIndex].TrinketType], tc.Trinkets[tc.CurrentIndex].TrinketType));
                     }
                  }
               }
            }
         }
      }
   }
}
