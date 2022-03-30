using Base.Components;
using Base.Entities;
using Base.Scenes;
using Base.System;
using Base.UI;
using Base.Utility.Services;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestGame.Components;

namespace TestGame.System
{ 
   [Serializable]
   public class HealthBarSystem:EngineSystem
   {
      StatusBar bar;
      public HealthBarSystem(Scene s)
      {
         systemSignature = (uint)(1 << HealthComponent.GetFamily() | 1 << Transform.GetFamily() | 1 << Sprite.GetFamily() | 1 << HealthBarComponent.GetFamily());
         registeredEntities = new List<Entity>();
         bar = new StatusBar();
         bar.maxValue = "10";
         Init(s);
      }

      public HealthBarSystem()
      {
         systemSignature = (uint)(1 << HealthComponent.GetFamily() | 1 << Transform.GetFamily() | 1 << Sprite.GetFamily() | 1 << HealthBarComponent.GetFamily());
         registeredEntities = new List<Entity>();
         bar = new StatusBar();
         bar.maxValue = "10";
      }

      public override void Init(Scene s)
      {
         RegisterScene(s);
         bar.init();
      }

      public override void Render(SpriteBatch sb)
      {
         sb.Begin(SpriteSortMode.FrontToBack, null, null, null, null, null, CameraService.camera.translationMatrix);
         foreach (Entity e in registeredEntities)
         {
            HealthComponent HC = parentScene.GetComponent<HealthComponent>(e);
            HealthBarComponent HBC = parentScene.GetComponent<HealthBarComponent>(e);
            Transform T = parentScene.GetComponent<Transform>(e);
            Sprite S = parentScene.GetComponent<Sprite>(e);

            if (HC.health < HC.maxHealth * HBC.noShowPercentage)
            {
               bar.maxValue = HC.maxHealth.ToString();
               bar.SetValue(HC.health.ToString());
               bar.bounds.X = (int)T.X;
               bar.bounds.Y = (int)T.Y - 20;
               bar.bounds.Width = (int)(S.imageWidth * T.widthRatio);
               bar.bounds.Height = (int)(10);
               bar.Render(sb);
            }
         }
         sb.End();
      }
   }
}
