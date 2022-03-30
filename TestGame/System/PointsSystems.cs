using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Base.System;
using Base.Scenes;
using Base.Entities;
using Base.Events;

using TestGame.Components;
using TestGame.Events;

using System.IO;
using Base.Utility.Services;
using TestGame.Utility.Services;

namespace TestGame.System
{
   [Serializable]
   public class PointsSystems : EngineSystem
   {
      float TotalPoints;
      int Wave;
      bool AddScore;

      public PointsSystems(Scene s)
      {
         systemSignature = (uint)(1 << PointsComponent.GetFamily());
         registeredEntities = new List<Entity>();
         AddScore = true;
         Wave = 1;
         Init(s);
      }

      public PointsSystems()
      {
         systemSignature = (uint)(1 << PointsComponent.GetFamily());
         registeredEntities = new List<Entity>();
         AddScore = true;
         Wave = 1;
      }

      public override void Init(Scene s)
      {
         RegisterScene(s);
         parentScene.bus.Subscribe(new EHandler<ObjectDetroyedEvent>(new Action<object, ObjectDetroyedEvent>(HandleObjectDetroyedEvent)));
         parentScene.bus.Subscribe(new EHandler<PlayerDiedEvent>(new Action<object, PlayerDiedEvent>(HandlePlayerDiedEvent)));
         parentScene.bus.Subscribe(new EHandler<NewWaveEvent>(new Action<object, NewWaveEvent>(HandleNextWaveEvent)));
      }

      public void HandleObjectDetroyedEvent(object sender, ObjectDetroyedEvent e)
      {
         if (AddScore)
         {
            foreach (Entity entity in registeredEntities)
            {
               if (entity == e.entityToDestroy)
               {
                  PointsComponent pointsComponent = parentScene.GetComponent<PointsComponent>(entity);
                  if (pointsComponent.pointValue > 0)
                  {
                     TotalPoints += pointsComponent.pointValue;
                     parentScene.bus.Publish(this, new PointsChangedEvent(TotalPoints));
                  }
                  else
                  {
                     ;
                  }
               }
            }
         }
      }

      public void HandlePlayerDiedEvent(object sender, PlayerDiedEvent e)
      {
         if (AddScore)
         {
            AddScore = false;
            ((Utility.TestGameSaveFile)SaveService.Save).money += (int)TotalPoints / 10;
            parentScene.bus.Publish(this, new GameOverEvent(TotalPoints, Wave));
         }
      }

      public void HandleNextWaveEvent(object sender, NewWaveEvent e)
      {
         Wave++;
         if(Wave == 26)
         {
            AddScore = false;
            ((Utility.TestGameSaveFile)SaveService.Save).money += (int)TotalPoints / 10;
            parentScene.bus.Publish(this, new GameOverEvent(TotalPoints, Wave));
         }
      }
   }
}
