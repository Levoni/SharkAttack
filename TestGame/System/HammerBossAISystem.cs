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
using TestGame.Events;
using TestGame.Factories;
using TestGame.Utility.Services;

namespace TestGame.System
{
   [Serializable]
   public class HammerBossAISystem:EngineSystem
   {
      public HammerBossAISystem(Scene s)
      {
         systemSignature = (uint)(1 << HammerBossComponent.GetFamily() | 1 << Sprite.GetFamily() | 1 << Transform.GetFamily());
         registeredEntities = new List<Entity>();
         Init(s);
      }

      public HammerBossAISystem()
      {
         systemSignature = (uint)(1 << HammerBossComponent.GetFamily() | 1 << Sprite.GetFamily() | 1 << Transform.GetFamily());
         registeredEntities = new List<Entity>();
      }

      public override void Init(Scene s)
      {
         RegisterScene(s);
         parentScene.bus.Subscribe(new EHandler<GameOverEvent>(new Action<object, GameOverEvent>(HandleGameOver)));
         parentScene.bus.Subscribe(new EHandler<ShockwaveEvent>(new Action<object, ShockwaveEvent>(HandleShockWaveEvent)));
      }

      public override void Update(int dt)
      {
         base.Update(dt);

         foreach (Entity e in registeredEntities)
         {
            Transform t = parentScene.GetComponent<Transform>(e);
            HammerBossComponent ai = parentScene.GetComponent<HammerBossComponent>(e);

            Transform playerTransform = PlayerTransformService.GetTransform(new EngineVector2(t.X, t.Y));//ai.targetTransform;
            EngineVector2 moveVector = new EngineVector2(playerTransform.X - t.X, playerTransform.Y - t.Y);

            ai.RemainingInterval -= dt;
            if(ai.RemainingInterval <= 0)
            {
               ai.IsFast = !ai.IsFast;
               ai.Speed = ai.IsFast ?  ai.BaseSpeed + ai.Speed : ai.BaseSpeed;
               ai.RemainingInterval = ai.IsFast ? ai.FastInterval : ai.SlowInterval;
            }

            ai.StunCooldownRemianing -= dt;
            if(ai.StunCooldownRemianing <= 0)
            {
               Sprite s = parentScene.GetComponent<Sprite>(e);
               ShotFactory.SpawnShockAttack(parentScene, t, s, moveVector.ToUnitVector());
               ai.StunCooldownRemianing = ai.StunCooldown;
            }

            if (moveVector.ToMagnitudeSquared() > 2)
            {
               moveVector = moveVector.ToUnitVector();

               parentScene.bus.Publish(this, new VelocityChangeEvent(e, new EngineVector2(moveVector.X * ai.Speed, moveVector.Y * ai.Speed)));
               t.rotation = (float)moveVector.GetDegreeRotation();
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

      public void HandleShockWaveEvent(object sender, ShockwaveEvent e)
      {
         foreach (Entity entity in registeredEntities)
         {
            var entityTransform = parentScene.GetComponent<Transform>(entity);
            var entityRigidBody = parentScene.GetComponent<RigidBody2D>(entity);
            EngineVector2 diffVector2 = new EngineVector2(entityTransform.X - e.Origin.X, entityTransform.Y - e.Origin.Y);
            float diffDistance = (float)Math.Sqrt(diffVector2.ToMagnitudeSquared());

            if (diffDistance <= e.Radius)
            {
               entityRigidBody.velocity = diffVector2.ToUnitVector() * 750;
            }
         }
      }
   }
}
