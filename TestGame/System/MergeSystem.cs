using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Base.System;
using Base.Scenes;
using Base.Components;
using Base.Entities;
using Base.Events;
using Base.Utility;

using TestGame.Events;
using TestGame.Components;
using TestGame.Factories;

namespace TestGame.System
{
   [Serializable]
   //TODO: seperate seperation part of system and add push apart when they break up.
   public class MergeSystem : EngineSystem
   {
      int logcount = 0;
      public MergeSystem(Scene s)
      {
         systemSignature = (uint)(1 << MergeComponent.GetFamily() | 1 << Sprite.GetFamily() | 1 << RigidBody2D.GetFamily() | 1 << Transform.GetFamily() | 1 << HealthComponent.GetFamily() | 1 << CombatComponent.GetFamily());
         registeredEntities = new List<Entity>();
         Init(s);
      }

      public MergeSystem()
      {
         systemSignature = (uint)(1 << MergeComponent.GetFamily() | 1 << Sprite.GetFamily() | 1 << RigidBody2D.GetFamily() | 1 << Transform.GetFamily() | 1 << HealthComponent.GetFamily() | 1 << CombatComponent.GetFamily());
         registeredEntities = new List<Entity>();
      }

      public override void Init(Scene s)
      {
         RegisterScene(s);
         parentScene.bus.Subscribe(new EHandler<CollisionEvent>(new Action<object, CollisionEvent>(HandleCollisionEvent)));
         parentScene.bus.Subscribe(new EHandler<DamageEvent>(new Action<object, DamageEvent>(HandleDamageEvent)));
      }

      public override void Update(int dt)
      {
         foreach (Entity e in registeredEntities)
         {
            MergeComponent MSC = parentScene.GetComponent<MergeComponent>(e);
            if(MSC.remainingStunTime > 0)
            {
               parentScene.bus.Publish(this, new VelocityChangeEvent(e,MSC.pushUnit * MSC.pushSpeed * ((float)MSC.remainingStunTime / (float)MSC.stunTime)));
               MSC.remainingStunTime -= dt;
            }
         }
      }

      public void HandleCollisionEvent(object sender, CollisionEvent e)
      {
         if (registeredEntities.Contains(e.context.entity1) && registeredEntities.Contains(e.context.entity2))
         {
            HealthComponent h1 = parentScene.GetComponent<HealthComponent>(e.context.entity1);
            HealthComponent h2 = parentScene.GetComponent<HealthComponent>(e.context.entity2);
            MergeComponent MSC1 = parentScene.GetComponent<MergeComponent>(e.context.entity1);
            MergeComponent MSC2 = parentScene.GetComponent<MergeComponent>(e.context.entity2);

            if (h1.health > 0 && h2.health > 0)
            {
               if ((h1.health <= 3 && h2.health <= 3) && MSC1.remainingStunTime <= 0 && MSC2.remainingStunTime <= 0)
               {
                  CombatComponent C1 = parentScene.GetComponent<CombatComponent>(e.context.entity1);
                  CombatComponent C2 = parentScene.GetComponent<CombatComponent>(e.context.entity2);

                  Sprite s1 = parentScene.GetComponent<Sprite>(e.context.entity1);
                  Sprite s2 = parentScene.GetComponent<Sprite>(e.context.entity2);
                  RigidBody2D rigid1 = parentScene.GetComponent<RigidBody2D>(e.context.entity1);
                  RigidBody2D rigid2 = parentScene.GetComponent<RigidBody2D>(e.context.entity2);
                  Transform t1 = parentScene.GetComponent<Transform>(e.context.entity1);
                  Transform t2 = parentScene.GetComponent<Transform>(e.context.entity2);

                  if (h1.health >= h2.health)
                  {
                     Entity newEnemy = EnemyFactory.createEnemy(parentScene, t1.X, t1.Y, h1.health + h2.health);
                     MergeComponent MC = parentScene.GetComponent<MergeComponent>(newEnemy);
                     RigidBody2D newRigid = parentScene.GetComponent<RigidBody2D>(newEnemy);
                     newRigid.velocity = rigid1.velocity;
                     MC.remainingStunTime = 0;
                     parentScene.DestroyEntity(e.context.entity1);
                     parentScene.DestroyEntity(e.context.entity2);
                  }
                  else
                  {
                     Entity newEnemy = EnemyFactory.createEnemy(parentScene, t2.X, t2.Y, h1.health + h2.health);
                     MergeComponent MC = parentScene.GetComponent<MergeComponent>(newEnemy);
                     RigidBody2D newRigid = parentScene.GetComponent<RigidBody2D>(newEnemy);
                     newRigid.velocity = rigid1.velocity;
                     MC.remainingStunTime = 0;
                     parentScene.DestroyEntity(e.context.entity1);
                     parentScene.DestroyEntity(e.context.entity2);
                  }
               }
            }

         }
      }

      public void HandleDamageEvent(object sender, DamageEvent e)
      {
         try
         {
            for (int i = 0; i < registeredEntities.Count(); i++)
            {
               if (e.defender.id == registeredEntities[i].id)
               {
                  Random r = new Random();
                  MergeComponent MSC = parentScene.GetComponent<MergeComponent>(e.defender);
                  CombatComponent C = parentScene.GetComponent<CombatComponent>(e.defender);
                  HealthComponent h = parentScene.GetComponent<HealthComponent>(e.defender);
                  Sprite s = parentScene.GetComponent<Sprite>(e.defender);
                  RigidBody2D rigid = parentScene.GetComponent<RigidBody2D>(e.defender);
                  Transform t = parentScene.GetComponent<Transform>(e.defender);

                  PointsComponent pointsComponent = parentScene.GetComponent<PointsComponent>(e.defender);
                  if (pointsComponent != null && h.health >= 0)
                  {
                     pointsComponent.pointValue -= h.health * 10;
                  }


                  if (h.health == 1)
                  {

                     RigidBody2D newRigid =  parentScene.GetComponent<RigidBody2D>(EnemyFactory.createEnemy(parentScene, t.X, t.Y, h.health));
                     newRigid.velocity = rigid.velocity / 2;
                     global::System.Diagnostics.Debug.WriteLine(++logcount + " : " + pointsComponent.pointValue);
                     h.health = 0;

                     parentScene.bus.Publish(this, new ObjectDetroyedEvent(e.defender));
                     parentScene.DestroyEntity(e.defender);
                  }
                  else if (h.health > 1)
                  {

                     if (h.health % 2 == 0)
                     {
                        h.health = h.health / 2;

                        MergeComponent M1 = parentScene.GetComponent<MergeComponent>(EnemyFactory.createEnemy(parentScene, t.X, t.Y, h.health));
                        M1.stunTime = M1.remainingStunTime = 500;
                        M1.pushSpeed = 20;
                        M1.pushUnit = new EngineVector2(r.Next() % 100, r.Next() % 100);
                        M1.pushUnit = M1.pushUnit.ToUnitVector();

                        MergeComponent M2 = parentScene.GetComponent<MergeComponent>(EnemyFactory.createEnemy(parentScene, t.X, t.Y, h.health));
                        M2.stunTime = M1.remainingStunTime = 500;
                        M2.pushSpeed = 20;
                        M2.pushUnit = M1.pushUnit * -1;

                        global::System.Diagnostics.Debug.WriteLine(++logcount + " : " + pointsComponent.pointValue);


                        h.health = 0;
                        parentScene.bus.Publish(this,new ObjectDetroyedEvent(e.defender));
                        parentScene.DestroyEntity(e.defender);
                     }
                     else
                     {
                        h.health = h.health / 2;

                        MergeComponent M1 = parentScene.GetComponent<MergeComponent>(EnemyFactory.createEnemy(parentScene, t.X, t.Y, h.health));
                        M1.stunTime = M1.remainingStunTime = 500;
                        M1.pushSpeed = 20;
                        M1.pushUnit = new EngineVector2(r.Next() % 100, r.Next() % 100);
                        M1.pushUnit = M1.pushUnit.ToUnitVector();

                        MergeComponent M2 = parentScene.GetComponent<MergeComponent>(EnemyFactory.createEnemy(parentScene, t.X, t.Y, h.health + 1));
                        M2.stunTime = M2.remainingStunTime = 500;
                        M2.pushSpeed = 20;
                        M2.pushUnit = new EngineVector2(M1.pushUnit.X * -1, M1.pushUnit.Y * -1);

                        global::System.Diagnostics.Debug.WriteLine(++logcount + " : " + pointsComponent.pointValue);

                        h.health = 0;
                        parentScene.bus.Publish(this,new ObjectDetroyedEvent(e.defender));
                        parentScene.DestroyEntity(e.defender);
                     }
                  }
               }
            }
         }
         catch(Exception ex)
         {
            ;
         }
      }
   }
}
