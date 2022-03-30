using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Base.System;
using Base.Entities;
using Base.Components;
using TestGame.Components;
using Base.Scenes;
using Base.Utility;
using Base.Events;
using Base.Utility.Services;
using TestGame.Utility.Services;
using TestGame.Events;

namespace TestGame.System
{
   [Serializable]
   public class AISystem : EngineSystem
   {

      public AISystem(Scene s)
      {
         systemSignature = (uint)((1 << Transform.GetFamily()) | (1 << AIComponent.GetFamily()));
         registeredEntities = new List<Entity>();
         Init(s);
      }

      public AISystem()
      {
         systemSignature = (uint)((1 << Transform.GetFamily()) | (1 << AIComponent.GetFamily()));
         registeredEntities = new List<Entity>();
      }

      public override void Init(Scene s)
      {
         RegisterScene(s);
         parentScene.bus.Subscribe(new EHandler<BoundryHitEvent>(new Action<object, BoundryHitEvent>(HandleBoundryHitEvent)));
         parentScene.bus.Subscribe(new EHandler<ShockwaveEvent>(new Action<object, ShockwaveEvent>(HandleShockWaveEvent)));
         parentScene.bus.Subscribe(new EHandler<GameOverEvent>(new Action<object, GameOverEvent>(HandleGameOver)));
      }

      public override void Update(int dt)
      {
         foreach (Entity e in registeredEntities)
         {
            Transform t = parentScene.GetComponent<Transform>(e);
            AIComponent ai = parentScene.GetComponent<AIComponent>(e);

            if (ai.type == Utility.AiType.zombie)
            {
               Transform playerTransform = PlayerTransformService.GetTransform(new EngineVector2(t.X,t.Y));//ai.targetTransform;
               EngineVector2 moveVector = new EngineVector2(playerTransform.X - t.X, playerTransform.Y - t.Y);

               if (moveVector.ToMagnitudeSquared() > 2)
               {
                  moveVector = moveVector.ToUnitVector();

                  parentScene.bus.Publish(this, new VelocityChangeEvent(e, new EngineVector2(moveVector.X * ai.speed, moveVector.Y * ai.speed)));
                  t.rotation = (float) moveVector.GetDegreeRotation();
               }
            }
            else if (ai.type == Utility.AiType.charging)
            {
               ChargingAiComponent aiComponent = ai as ChargingAiComponent;
               if (aiComponent.state == AiChargeState.charging)
               {
                  aiComponent.remainingChargeTime -= dt;
                  Transform playerTransform = ai.targetTransform;
                  EngineVector2 target = new EngineVector2(playerTransform.X - t.X, playerTransform.Y - t.Y).ToUnitVector();
                  double rotation = target.GetDegreeRotation();
                  Transform transform = parentScene.GetComponent<Transform>(e);
                  transform.rotation = (float) rotation;
                  if (aiComponent.remainingChargeTime <= 0)
                  {
                     aiComponent.remainingChargeTime = aiComponent.chargeTime;
                     aiComponent.state = AiChargeState.ready;
                  }
               }
               else if (aiComponent.state == AiChargeState.stunned)
               {
                  aiComponent.remainingStunTime -= dt;
                  if (aiComponent.remainingStunTime <= 0)
                  {
                     Sprite s = parentScene.GetComponent<Sprite>(e);
                     if(ConfigService.config.SpriteSet == "Donuts and Sharks")
                     {
                        s.image = ContentService.Get2DTexture("charging-shark-charging");
                     }
                     else
                     {
                        s.image = ContentService.Get2DTexture("ChargingEnemy-Charging-geometry");
                     }
                     aiComponent.remainingStunTime = aiComponent.StunTime;
                     //Change image
                     aiComponent.state = AiChargeState.charging;
                  }
               }
               else if (aiComponent.state == AiChargeState.moving)
               {
                  parentScene.bus.Publish(this, new VelocityChangeEvent(e, new EngineVector2(aiComponent.movement.X * ai.speed, aiComponent.movement.Y * ai.speed)));
               }
               else if (aiComponent.state == AiChargeState.ready)
               {
                  Sprite s = parentScene.GetComponent<Sprite>(e);
                  if (ConfigService.config.SpriteSet == "Donuts and Sharks")
                  {
                     s.image = ContentService.Get2DTexture("charging-shark");
                  }
                  else
                  {
                     s.image = ContentService.Get2DTexture("ChargingEnemy-Moving-geometry");
                  }
                  Transform playerTransform = ai.targetTransform;
                  EngineVector2 target = new EngineVector2(playerTransform.X - t.X, playerTransform.Y - t.Y).ToUnitVector();
                  aiComponent.movement = target;
                  aiComponent.state = AiChargeState.moving;
               }
            }
         }
      }

      public void HandleBoundryHitEvent(object sender, BoundryHitEvent e)
      {
         foreach(Entity entity in registeredEntities)
         {
            if(e.entity.id == entity.id)
            {
               AIComponent aiComponent = parentScene.GetComponent<AIComponent>(entity);
               if(aiComponent.type == Utility.AiType.charging)
               {
                  ChargingAiComponent CAI = aiComponent as ChargingAiComponent;
                  CAI.state = AiChargeState.stunned;
                  Sprite s = parentScene.GetComponent<Sprite>(entity);
                  if (ConfigService.config.SpriteSet == "Donuts and Sharks")
                  {
                     s.image = ContentService.Get2DTexture("charging-shark-stuned");
                  }
                  else
                  {
                     s.image = ContentService.Get2DTexture("ChargingEnemy-Stunned-geometry");
                  }
               }
            }
         }
      }

      public void HandleShockWaveEvent(object sender, ShockwaveEvent e)
      {
         foreach(Entity entity in registeredEntities)
         {
            var entityTransform = parentScene.GetComponent<Transform>(entity);
            var entityRigidBody = parentScene.GetComponent<RigidBody2D>(entity);
            EngineVector2 diffVector2 = new EngineVector2(entityTransform.X - e.Origin.X, entityTransform.Y - e.Origin.Y);
            float diffDistance = (float) Math.Sqrt(diffVector2.ToMagnitudeSquared());

            if(diffDistance <= e.Radius)
            {
               entityRigidBody.velocity = diffVector2.ToUnitVector()  * 750;
            }
         }
      }
      public void HandleGameOver(object sender, GameOverEvent e)
      {
         for (int i = registeredEntities.Count - 1; i >= 0; i--)
         {
            parentScene.DestroyEntity(registeredEntities[i]);
         }
      }
   }
}
