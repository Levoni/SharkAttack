using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Base.System;
using TestGame.Components;
using Base.Scenes;
using Base.Entities;
using Base.Components;
using Base.Events;
using Base.Utility;
using Base.Utility.Services;
using TestGame.Utility;
using TestGame.Events;
using TestGame.Model;
using TestGame.Utility.Services;

namespace TestGame.System
{
   [Serializable]
   public class ShootingSystem:EngineSystem
   {
      Random rand;
      public ShootingSystem(Scene s)
      {
         systemSignature = (uint)(1 << ShootingComponent.GetFamily() | 1 << Transform.GetFamily() | 1 << Sprite.GetFamily());
         registeredEntities = new List<Entity>();
         rand = new Random();
         Init(s);
      }

      public ShootingSystem()
      {
         systemSignature = (uint)(1 << ShootingComponent.GetFamily() | 1 << Transform.GetFamily() | 1 << Sprite.GetFamily());
         registeredEntities = new List<Entity>();
         rand = new Random();
      }

      public override void Init(Scene s)
      {
         RegisterScene(s);
         parentScene.bus.Subscribe(new EHandler<ControlEvent>(new Action<object, ControlEvent>(HandleControlEvent)));
         parentScene.bus.Subscribe(new EHandler<LeechShotEvent>(new Action<object, LeechShotEvent>(HandleLeechShotEvent)));
      }

      public override void Update(int dt)
      {
         foreach(Entity e in registeredEntities)
         {
            ShootingComponent sc = parentScene.GetComponent<ShootingComponent>(e);
            WeaponModel wm = sc.guns[sc.currentGunTypeIndex];
            sc.remainingTimes[wm.GunType] -= dt;
            if (sc.remainingTimes[wm.GunType] < 0)
               sc.remainingTimes[wm.GunType] = 0;
         }
      }


      private void Shoot(Entity e, Enums.gameControlState controlState, bool leachShot)
      {
         ShootingComponent sc = parentScene.GetComponent<ShootingComponent>(e);
         WeaponModel wm = sc.guns[sc.currentGunTypeIndex];

         if (sc.remainingTimes[wm.GunType] <= 0)
         {
            Transform t = parentScene.GetComponent<Transform>(e);
            EngineVector2 curMousScreenPos = MouseService.GetMousePosition();
            Sprite s = parentScene.GetComponent<Sprite>(e);
            var curMouseWorldPos = CameraService.camera.ScreenToWorld(new Microsoft.Xna.Framework.Vector2(curMousScreenPos.X, curMousScreenPos.Y));
            EngineVector2 startTransform = new EngineVector2(t.X + (t.widthRatio * s.imageWidth * .5f), t.Y + (t.heightRatio * s.imageWidth * .5f));
            EngineVector2 unitVector = new EngineVector2(curMouseWorldPos.X - startTransform.X, curMouseWorldPos.Y - startTransform.Y).ToUnitVector();

            var saveItem = ((TestGameSaveFile)SaveService.Save).Weapons.Find(x => x.ItemName == wm.ItemName);
            float damage = wm.BaseDamage + wm.DamageIncrease * (saveItem.Level - 1);

            if (wm.GunType == GunType.pistol && controlState == Enums.gameControlState.keyDown)
            {
               var bullet = Factories.ShotFactory.SpawnBullet(parentScene, unitVector, t, s, sc, damage);
               if(leachShot)
               {
                  LeechShotComponent leech = new LeechShotComponent();
                  parentScene.AddComponent(bullet, leech);
               }
            }
            else if (wm.GunType == GunType.shotgun)
            {
               List<EngineVector2> directions = new List<EngineVector2>();
               List<float> speeds = new List<float>();
               for (int i = 0; i < 8; i++)
               {
                  float speed = (float)(rand.Next() % 100) + 350;
                  float rotation = (float)(rand.Next() % 89) - 44;
                  speeds.Add(speed);
                  EngineVector2 angle = new EngineVector2(unitVector.X, unitVector.Y);
                  angle.RotateVector(rotation);
                  directions.Add(angle);
               }

               for (int i = 0; i < 8; i++)
               {
                  var bullet = Factories.ShotFactory.SpawnBullet(parentScene, directions[i], t, s, sc, damage, speeds[i]);
                  if (leachShot)
                  {
                     LeechShotComponent leech = new LeechShotComponent();
                     parentScene.AddComponent(bullet, leech);
                  }
               }
               sc.remainingTimes[wm.GunType] = wm.CooldownAmount;
            }
            else if (wm.GunType == GunType.sniper)
            {
               var bullet = Factories.ShotFactory.SpawnSniperBullet(parentScene, unitVector, t, s, sc, damage);
               if (leachShot)
               {
                  LeechShotComponent leech = new LeechShotComponent();
                  parentScene.AddComponent(bullet, leech);
               }
               sc.remainingTimes[wm.GunType] = wm.CooldownAmount;
            }
            else if (wm.GunType == GunType.machinegun)
            {
               var bullet = Factories.ShotFactory.SpawnMachineGunBullet(parentScene, unitVector, t, s, sc, damage);
               if (leachShot)
               {
                  LeechShotComponent leech = new LeechShotComponent();
                  parentScene.AddComponent(bullet, leech);
               }
               sc.remainingTimes[wm.GunType] = wm.CooldownAmount;
            }
            else if (wm.GunType == GunType.burst)
            {
               List<Entity> eList = new List<Entity>(); 
               eList.Add(Factories.ShotFactory.SpawnBullet(parentScene, new EngineVector2(0,-1).ToUnitVector(), t, s, sc, damage));
               eList.Add(Factories.ShotFactory.SpawnBullet(parentScene, new EngineVector2(1, -1).ToUnitVector(), t, s, sc, damage));
               eList.Add(Factories.ShotFactory.SpawnBullet(parentScene, new EngineVector2(1, 0).ToUnitVector(), t, s, sc, damage));
               eList.Add(Factories.ShotFactory.SpawnBullet(parentScene, new EngineVector2(1, 1).ToUnitVector(), t, s, sc, damage));
               eList.Add(Factories.ShotFactory.SpawnBullet(parentScene, new EngineVector2(0, 1).ToUnitVector(), t, s, sc, damage));
               eList.Add(Factories.ShotFactory.SpawnBullet(parentScene, new EngineVector2(-1, 1).ToUnitVector(), t, s, sc, damage));
               eList.Add(Factories.ShotFactory.SpawnBullet(parentScene, new EngineVector2(-1, 0).ToUnitVector(), t, s, sc, damage));
               eList.Add(Factories.ShotFactory.SpawnBullet(parentScene, new EngineVector2(-1, -1).ToUnitVector(), t, s, sc, damage));
               foreach (Entity tempE in eList)
               {
                  if (leachShot)
                  {
                     LeechShotComponent leech = new LeechShotComponent();
                     parentScene.AddComponent(tempE, leech);
                  }
               }
               sc.remainingTimes[wm.GunType] = wm.CooldownAmount;
            }
         }
      }

      private void HandleControlEvent(object sender, ControlEvent e)
      {
         if (e.controlType == Enums.ControlType.attack1)
         {
            foreach (Entity entity in registeredEntities)
            {
               if (e.e == entity)
               {
                  Shoot(e.e, e.state, false);
               }
            }
         }
         else if (e.state == Enums.gameControlState.keyDown && e.controlType == Enums.ControlType.modifier2)
         {
            foreach (Entity entity in registeredEntities)
            {
               if (e.e == entity)
               {
                  ShootingComponent sc = parentScene.GetComponent<ShootingComponent>(entity);
                  sc.currentGunTypeIndex = (sc.currentGunTypeIndex + 1) % sc.guns.Count;
                  WeaponModel wm = sc.guns[sc.currentGunTypeIndex];
                  parentScene.bus.Publish(this, new GunChangeEvent(sc.guns[sc.currentGunTypeIndex].GunType));
               }
            }
         }
      }

      private void HandleLeechShotEvent(object sender, LeechShotEvent e)
      {
         Shoot(e.e, Enums.gameControlState.keyDown, true);
      }
   }
}
