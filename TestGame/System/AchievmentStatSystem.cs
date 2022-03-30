using Base.System;
using Base.Scenes;
using Base.Entities;
using Base.Events;

using TestGame.Components;
using TestGame.Events;

using System.IO;
using Base.Utility.Services;
using TestGame.Utility.Services;
using System;
using System.Collections.Generic;
using Base.Components;
using TestGame.Model;
using Microsoft.Xna.Framework.Graphics;
using TestGame.UIComponents;
using Base.Utility;

namespace TestGame.System
{
   [Serializable]
   public class AchievmentStatSystem : EngineSystem
   {
      public bool hasTakenDamage { get; set; }
      private AchievementUI achievementUI { get; set; }

      public AchievmentStatSystem(Scene s)
      {
         Init(s);
      }

      public AchievmentStatSystem()
      {
         systemSignature = (uint)(1 << AchievmentComponent.GetFamily());
         registeredEntities = new List<Entity>();
         hasTakenDamage = false;
         achievementUI = new AchievementUI(new AchievmentModel(), new EngineRectangle());
      }

      public override void Init(Scene s)
      {
         RegisterScene(s);
         parentScene.bus.Subscribe(new EHandler<ShootEvent>(new Action<object, ShootEvent>(HandleShootEvent)));
         parentScene.bus.Subscribe(new EHandler<DamageEvent>(new Action<object, DamageEvent>(HandleDamageEvent)));
         parentScene.bus.Subscribe(new EHandler<HealthChangeEvent>(new Action<object, HealthChangeEvent>(HandleHealthChangeEvent)));
         parentScene.bus.Subscribe(new EHandler<ItemPurchesedEvent>(new Action<object, ItemPurchesedEvent>(HandleItemPurchesedEvent)));
         parentScene.bus.Subscribe(new EHandler<NewWaveEvent>(new Action<object, NewWaveEvent>(HandleNewWaveEvent)));
         parentScene.bus.Subscribe(new EHandler<GameOverEvent>(new Action<object, GameOverEvent>(HandleGameOverEvent)));
      }

      public override void Update(int dt)
      {
         base.Update(dt);
         for (int i = registeredEntities.Count - 1; i >= 0; i--)
         {
            var achievement = parentScene.GetComponent<AchievmentComponent>(registeredEntities[i]);
            achievement.TimeRemaining -= dt;
            if (achievement.TimeRemaining <= 0)
            {
               parentScene.DestroyEntity(registeredEntities[i]);
            }
         }
      }

      public override void Render(SpriteBatch sb)
      {
         base.Render(sb);
         for (int i = 0; i < registeredEntities.Count; i++)
         {
            var achievement = parentScene.GetComponent<AchievmentComponent>(registeredEntities[i]);
            var viewport = ScreenGraphicService.GetViewportBounds();
            if (achievementUI != null)
            {
               achievementUI.SetNewAchievement(achievement.Model);
               achievementUI.SetNewBounds(new EngineRectangle(viewport.Width / 4 * 3 + 10, viewport.Height / 10 * (9 - i), (viewport.Width / 4 - 20), viewport.Height / 10));
               //UiInterface.SetNewBounds(new EngineRectangle((int)viewport.X + (int)viewport.Width / 2, (int)viewport.Y + (int)viewport.Height / 10 * (8 - 2 * i), (int)viewport.Width / 2, (int)viewport.Height / 10 * 2));
               sb.Begin();
               achievementUI.Render(sb);
               sb.End();
            }
         }
      }

      //TODO: add achievments below

      // beat game
      // clear game without taking damage

      private void ShowAchievement(AchievmentModel achievement)
      {
         Entity newEntity = parentScene.CreateEntity();
         AchievmentComponent achievementUIComponent = new AchievmentComponent(5000, achievement);
         parentScene.AddComponent(newEntity, achievementUIComponent);
         achievement.hasAchievement = true;
         AchievmentService.SaveAchievmentsAndStats();
      }

      private void HandleNewWaveEvent(object sender, NewWaveEvent e)
      {
         if(e.curWave > AchievmentService.Stats["Max Wave Taking No Damage"].StatValue && !hasTakenDamage)
         {
            AchievmentService.Stats["Max Wave Taking No Damage"].StatValue = e.curWave;
            AchievmentService.SaveAchievmentsAndStats();
         }
         if (e.curWave == 6)
         {
            if (!AchievmentService.Achievments["Invincibility 1"].hasAchievement && !hasTakenDamage)
            {
               ShowAchievement(AchievmentService.Achievments["Invincibility 1"]);
            }
         }
         else if (e.curWave == 16)
         {
            if (!AchievmentService.Achievments["Invincibility 2"].hasAchievement && !hasTakenDamage)
            {
               ShowAchievement(AchievmentService.Achievments["Invincibility 2"]);
            }
         }
      }

      public void HandleGameOverEvent(object sender, GameOverEvent e)
      {
         AchievmentService.SaveAchievmentsAndStats();
      }

      private void HandleShootEvent(object sender, ShootEvent e)
      {
         AchievmentService.Stats["Shots"].StatValue++;
         if(AchievmentService.Stats["Shots"].StatValue > 100)
         {
            if(!AchievmentService.Achievments["Trigger Happy 1"].hasAchievement)
            {
               ShowAchievement(AchievmentService.Achievments["Trigger Happy 1"]);
            }
            
            if(AchievmentService.Stats["Shots"].StatValue > 1000)
            {
               if (!AchievmentService.Achievments["Trigger Happy 2"].hasAchievement)
               {
                  ShowAchievement(AchievmentService.Achievments["Trigger Happy 2"]);
               }

               if (AchievmentService.Stats["Shots"].StatValue > 10000)
               {
                  if (!AchievmentService.Achievments["Trigger Happy 3"].hasAchievement)
                  {
                     ShowAchievement(AchievmentService.Achievments["Trigger Happy 3"]);
                  }

                  if (AchievmentService.Stats["Shots"].StatValue > 100000)
                  {
                     if (!AchievmentService.Achievments["Trigger Happy 4"].hasAchievement)
                     {
                        ShowAchievement(AchievmentService.Achievments["Trigger Happy 4"]);
                     }
                  }
               }
            }
         }         
      }

      private void HandleHealthChangeEvent(object sender, HealthChangeEvent e)
      {
         var healthChange = e.currentHealth - e.oldHealth;
         if (healthChange > 0)
         {
            AchievmentService.Stats["Heath Healed"].StatValue++;
            if(AchievmentService.Stats["Heath Healed"].StatValue > 10)
            {
               if (!AchievmentService.Achievments["Medic 1"].hasAchievement)
               {
                  ShowAchievement(AchievmentService.Achievments["Medic 1"]);
               }

               if (AchievmentService.Stats["Heath Healed"].StatValue > 100)
               {
                  if (!AchievmentService.Achievments["Medic 2"].hasAchievement)
                  {
                     ShowAchievement(AchievmentService.Achievments["Medic 2"]);
                  }

                  if (AchievmentService.Stats["Heath Healed"].StatValue > 1000)
                  {
                     if (!AchievmentService.Achievments["Medic 3"].hasAchievement)
                     {
                        ShowAchievement(AchievmentService.Achievments["Medic 3"]);
                     }
                  }
               }
            }
         }
         if(healthChange < 0)
         {
            AchievmentService.Stats["Damage Taken"].StatValue++;
            if (AchievmentService.Stats["Damage Taken"].StatValue >= 10)
            {
               if(!AchievmentService.Achievments["Meat Shield 1"].hasAchievement)
               {
                  ShowAchievement(AchievmentService.Achievments["Meat Shield 1"]);
               }

               if (AchievmentService.Stats["Damage Taken"].StatValue >= 100)
               {
                  if (!AchievmentService.Achievments["Meat Shield 2"].hasAchievement)
                  {
                     ShowAchievement(AchievmentService.Achievments["Meat Shield 2"]);
                  }

                  if (AchievmentService.Stats["Damage Taken"].StatValue >= 1000)
                  {
                     if (!AchievmentService.Achievments["Meat Shield 3"].hasAchievement)
                     {
                        ShowAchievement(AchievmentService.Achievments["Meat Shield 3"]);
                     }
                  }
               }

            }
            if (!hasTakenDamage)
            {
               hasTakenDamage = true;
            }
         }
      }

      private void HandleItemPurchesedEvent(object sender, ItemPurchesedEvent e)
      {
         if(e.ItemModel.Level >= 5 && !AchievmentService.Achievments["Master"].hasAchievement)
         {
            ShowAchievement(AchievmentService.Achievments["Master"]);
         }
         if (e.ItemModel.ItemName == "blast_shot" && !AchievmentService.Achievments["Special Delivery"].hasAchievement)
         {
            ShowAchievement(AchievmentService.Achievments["Special Delivery"]);
         }

         if(e.ItemModel.Level == 1)
         {
            if(e.ItemType == UpgradeMenu.shopTypeEnum.weapon)
            {
               if(!AchievmentService.Achievments["Starting A Arsenal"].hasAchievement)
               {
                  ShowAchievement(AchievmentService.Achievments["Starting A Arsenal"]);
               }
            }
            if (e.ItemType == UpgradeMenu.shopTypeEnum.trinket)
            {
               if (!AchievmentService.Achievments["Girls Best Friend"].hasAchievement)
               {
                  ShowAchievement(AchievmentService.Achievments["Girls Best Friend"]);
               }
            }
            if (e.ItemType == UpgradeMenu.shopTypeEnum.ability)
            {
               if (!AchievmentService.Achievments["Training"].hasAchievement)
               {
                  ShowAchievement(AchievmentService.Achievments["Training"]);
               }
            }
         }
      }

      private void HandleDamageEvent(object sender, DamageEvent e)
      {
         var healthComp = parentScene.GetComponent<HealthComponent>(e.defender);
         if(healthComp != null && healthComp.health <= 0)
         {
            if (!AchievmentService.Achievments["HammerHead slayer"].hasAchievement)
            {
               var hammerComponent = parentScene.GetComponent<HammerBossComponent>(e.defender);
               if(hammerComponent != null)
               {
                  ShowAchievement(AchievmentService.Achievments["HammerHead slayer"]);
               }
            }

            if (!AchievmentService.Achievments["Squid Slayer"].hasAchievement)
            {
               var squidComponent = parentScene.GetComponent<SquidBossAiComponent>(e.defender);
               if (squidComponent != null)
               {
                  ShowAchievement(AchievmentService.Achievments["Squid Slayer"]);
               }
            }

            var AI = parentScene.GetComponent<AIComponent>(e.defender);
            if(AI != null)
            {
               AchievmentService.Stats["Kills"].StatValue++;
               if(AchievmentService.Stats["Kills"].StatValue >= 100)
               {
                  if(!AchievmentService.Achievments["Slayer 1"].hasAchievement)
                  {
                     ShowAchievement(AchievmentService.Achievments["Slayer 1"]);
                  }

                  if (AchievmentService.Stats["Kills"].StatValue >= 1000)
                  {
                     if (!AchievmentService.Achievments["Slayer 2"].hasAchievement)
                     {
                        ShowAchievement(AchievmentService.Achievments["Slayer 2"]);
                     }

                     if (AchievmentService.Stats["Kills"].StatValue >= 10000)
                     {
                        if (!AchievmentService.Achievments["Slayer 3"].hasAchievement)
                        {
                           ShowAchievement(AchievmentService.Achievments["Slayer 3"]);
                        }
                     }
                  }
               }
            }
         }

         //var playerComp = parentScene.GetComponent<PlayerController>(e.defender);
         //if(playerComp != null && healthComp != null)
         //{
         //   if(healthComp.health > 0)
         //   {
         //      //add damage to total health lost
               
         //   }
         //   else
         //   {
         //      var absDamage = Math.Abs(e.damage);
         //      var absPlayerOverDamage = Math.Abs(healthComp.health);
         //      var actualHealthLost = absDamage - absPlayerOverDamage;
         //      //add actualHealthLost to total health lost
         //   }
         //}
      }
   }
}
