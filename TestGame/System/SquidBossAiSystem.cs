using Base.Components;
using Base.Entities;
using Base.Events;
using Base.Scenes;
using Base.System;
using Base.Utility;
using Base.Utility.Services;
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
   public class SquidBossAiSystem : EngineSystem
   {
      public EngineRectangle worldBounds;
      public SquidBossAiSystem(Scene s)
      {
         systemSignature = (uint)(1 << SquidBossAiComponent.GetFamily() | 1 << Sprite.GetFamily() | 1 << Transform.GetFamily());
         registeredEntities = new List<Entity>();
         Init(s);
      }

      public SquidBossAiSystem()
      {
         systemSignature = (uint)(1 << SquidBossAiComponent.GetFamily() | 1 << Sprite.GetFamily() | 1 << Transform.GetFamily());
         registeredEntities = new List<Entity>();
      }

      public override void Init(Scene s)
      {
         RegisterScene(s);
         parentScene.bus.Subscribe(new EHandler<GameOverEvent>(new Action<object, GameOverEvent>(HandleGameOver)));
      }

      public override void Update(int dt)
      {
         base.Update(dt);
         foreach(Entity e in registeredEntities)
         {
            SquidBossAiComponent squid = parentScene.GetComponent<SquidBossAiComponent>(e);
            Transform t = parentScene.GetComponent<Transform>(e);
            Sprite s = parentScene.GetComponent<Sprite>(e);

            if(t.Y <= 50 && squid.IsUpward)
            {
               parentScene.bus.Publish(this, new VelocitySetEvent(e, new EngineVector2()));
               squid.IsUpward = !squid.IsUpward;
            }
            else if ((t.Y + s.imageHeight) > (worldBounds.Height - 50) && !squid.IsUpward)
            {
               parentScene.bus.Publish(this, new VelocitySetEvent(e, new EngineVector2()));
               squid.IsUpward = !squid.IsUpward;
            }

            var ySpeed = squid.IsUpward ? squid.Speed * -1 : squid.Speed;
            parentScene.bus.Publish(this, new VelocityChangeEvent(e, new EngineVector2(0, ySpeed)));

            squid.RemainingSpawnDelay -= dt;
            if(squid.RemainingSpawnDelay <= 0)
            {
               squid.RemainingSpawnDelay = squid.SpawnDelayTime;
               for(int i = 0; i < squid.SpawnAmount; i++)
               {
                  float X = t.X;
                  float Y = t.Y + (t.heightRatio * s.imageHeight) / (squid.SpawnAmount - 1) * i;
                  float XDirection = squid.IsRight ? -1 : 1;
                  EngineVector2 unitVectorAngle = new EngineVector2(XDirection, -1f + (2f / (float)(squid.SpawnAmount - 1f) * i));
                  Entity e1 = EnemyFactory.createEnemy(parentScene, X, Y);
                  var rigidBody = parentScene.GetComponent<RigidBody2D>(e1);
                  rigidBody.velocity = (unitVectorAngle.ToUnitVector() * 800);
                  //parentScene.bus.Publish(this, new VelocityChangeEvent(e1, unitVectorAngle.ToUnitVector() * 50));
               }
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
