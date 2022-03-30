using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Base.Components;
using Base.UI;
using TestGame.Events;

using Microsoft.Xna.Framework.Graphics;
using Base.Scenes;
using Base.Utility;
using Microsoft.Xna.Framework;
using Base.Utility.Services;

namespace TestGame.Components
{
   [Serializable]
   public class overlayUI:Base.UI.GUI
   {
      Label lblScore;
      Label lblWave;
      Label lblWavePopup;
      //Label lblGunType;
      //Label lblBomb;

      ProgressBar ability;
      Button GunImage;
      Button TrinketImage;

      float popupRemainingInterval;

      public override void Update(int dt)
      {
         updateOverlay(dt);
      }

      public override void Render(SpriteBatch sb)
      {
         renderOverlay(sb);
      }

      public override void Init()
      {
         //lblBomb.init();
         lblScore.init();
         lblWave.init();
         //lblGunType.init();
         ability.init();
         GunImage.init();
         TrinketImage.init();
      }

      public override void Init(Base.Events.EventBus eb, Scene parentScene)
      {
         EngineRectangle viewport = ScreenGraphicService.GetViewportBounds();
         //lblBomb = new Label("lblBomb", "Bombs Remaining: 3", new EngineRectangle(50, 980, 200, 50), Microsoft.Xna.Framework.Color.White);
         //lblGunType = new Label("lblGunType", "Gun Type: Pistol", new EngineRectangle(50, 880, 200, 50), Microsoft.Xna.Framework.Color.White);

         lblScore = new Label("lblScore", "Score: 0", new EngineRectangle(50, 50, 200, 50), Color.White * .75f);
         lblWave = new Label("lblWave", "Wave: 1", new EngineRectangle(50, 100, 200, 50), Color.White * .75f);
         lblWavePopup = new Label("lblWavePopup", "Wave 1", new EngineRectangle(viewport.Width / 20 * 9, viewport.Height / 40 * 19, viewport.Width / 10, viewport.Width / 20), Color.White * .75f);


         ability = new ProgressBar("abilityIamge", "", new EngineRectangle(viewport.Width / 4 * 3, 50, 100,100), Color.White, "black", Color.White * .50f);
         ability.drawColor = Color.White * .75f;
         ability.isHorizontal = false;
         ability.setImageReferences("none");
         ability.barPercent = 0;
         GunImage = new Button("gunImage", "", new EngineRectangle(viewport.Width / 4 * 3 + 125, 50, 100, 100), Color.Black);
         GunImage.drawColor = Color.White * .75f;
         GunImage.setImageReferences("none");
         TrinketImage = new Button("trinketImage", "", new EngineRectangle(viewport.Width / 4 * 3 + 250, 50, 100, 100), Color.White * .75F);
         TrinketImage.drawColor = Color.White * .75f;
         TrinketImage.setImageReferences("none");
         popupRemainingInterval = 5000;

         base.Init(eb, parentScene);
         EventBus.Subscribe(new Base.Events.EHandler<PointsChangedEvent>(new Action<object, PointsChangedEvent>(HandlePointChange)));
         EventBus.Subscribe(new Base.Events.EHandler<NewWaveEvent>(new Action<object, NewWaveEvent>(HandleNextWave)));
         EventBus.Subscribe(new Base.Events.EHandler<TrinketShotEvent>(new Action<object, TrinketShotEvent>(HandleTrinketShot)));
         EventBus.Subscribe(new Base.Events.EHandler<GunChangeEvent>(new Action<object, GunChangeEvent>(handleGunChange)));
         EventBus.Subscribe(new Base.Events.EHandler<AbilityCooldownChangeEvent>(new Action<object, AbilityCooldownChangeEvent>(HandleAbilityCooldownChange)));
      }

      public void updateOverlay(int dt)
      {
         //lblBomb.Update(dt);
         lblScore.Update(dt);
         lblWave.Update(dt);
         //lblGunType.Update(dt);
         ability.Update(dt);
         GunImage.Update(dt);
         TrinketImage.Update(dt);
         lblWavePopup.Update(dt);
         if( popupRemainingInterval > 0)
         {
            popupRemainingInterval -= dt;
            if(popupRemainingInterval < 0)
            {
               lblWavePopup.value = "";
            }
         }
      }
      
      public void renderOverlay(SpriteBatch sb)
      {
         //lblBomb.Render(sb);
         lblScore.Render(sb);
         lblWave.Render(sb);
         //lblGunType.Render(sb);
         ability.Render(sb);
         GunImage.Render(sb);
         TrinketImage.Render(sb);
         lblWavePopup.Render(sb);
      }

      public void HandlePointChange(object sender, PointsChangedEvent e)
      {
         lblScore.value = "Score: " + ((int)e.CurrentPoints).ToString();
      }

      public void HandleNextWave(object sender, NewWaveEvent e)
      {
         if (e.curWave <= 25)
         {
            lblWave.value = "Wave: " + e.curWave.ToString();
            lblWavePopup.value = "Wave: " + e.curWave.ToString();
            popupRemainingInterval = 5000;
         }
      }

      public void HandleTrinketShot(object sender, TrinketShotEvent e)
      {
         if (e.type == Utility.TrinketType.bomb)
            TrinketImage.setImageReferences("bomb-pickup");
         if (e.type == Utility.TrinketType.decoy)
            TrinketImage.setImageReferences("Decoy_powerup");
         if (e.type == Utility.TrinketType.lifeFountain)
            TrinketImage.setImageReferences("LifeFountainPowerup");

         TrinketImage.value = e.BombsRemaining.ToString();
      }

      public void HandleAbilityCooldownChange(object setnder, AbilityCooldownChangeEvent e)
      {
         if (e.type == Utility.AbilityType.leechShot)
            ability.setImageReferences("LeechShot");
         if (e.type == Utility.AbilityType.shockwave)
            ability.setImageReferences("Shockwave");

         ability.barPercent = e.cooldownLeft / e.maxCooldown;
      }

      public void handleGunChange(object sender, GunChangeEvent e)
      {
         if (e.currentGun == Utility.GunType.pistol)
            GunImage.setImageReferences("pistol-powerup");
         if (e.currentGun == Utility.GunType.machinegun)
            GunImage.setImageReferences("machine-gun");
         if (e.currentGun == Utility.GunType.shotgun)
            GunImage.setImageReferences("shotgun-powerup");
         if (e.currentGun == Utility.GunType.sniper)
            GunImage.setImageReferences("sniper-powerup");
         if(e.currentGun == Utility.GunType.burst)
            GunImage.setImageReferences("blast_shot");
      }
   }
}
