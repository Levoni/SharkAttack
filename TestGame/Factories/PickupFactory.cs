using Base.Components;
using Base.Entities;
using Base.Scenes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestGame.Components;
using TestGame.Utility;
using TestGame.Utility.Services;

namespace TestGame.Factories
{
   public static class PickupFactory
   {
      public static Entity createPickup(Scene scene, float x, float y, pickupType type, int imgWidth = 30, int imgheight = 30)
      {
         const int pickupCollisionMaskLayer = 10;

         Entity pickup = scene.CreateEntity();

         //TODO: Add image
         string textureName = "defaultTexture";
         if (ConfigService.config.SpriteSet == "Donuts and Sharks")
         {
            if (type == pickupType.health)
               textureName = "health-pickup";
            else if (type == pickupType.bomb)
               textureName = "bomb-pickup";
            else if (type == pickupType.shotgun)
               textureName = "shotgun-powerup";
            else if (type == pickupType.sniper)
               textureName = "sniper-powerup";
         }
         else
         {
            if (type == pickupType.health)
               textureName = "Health-geometry";
            else if (type == pickupType.bomb)
               textureName = "Bomb-geometry";
            else if (type == pickupType.shotgun)
               textureName = "shotgun-powerup-geometry";
            else if (type == pickupType.sniper)
               textureName = "sniper-powerup-geometry";
         }

         Sprite s = new Sprite(textureName);
         s.imageWidth = imgWidth;
         s.imageHeight = imgheight;
         Transform t = new Transform(x, y, 1f, 1f, 0);

         Base.Collision.BoxCollisionBound2D BCBC = new Base.Collision.BoxCollisionBound2D(0, 0, 1, 1, pickupCollisionMaskLayer);
         ColliderTwoD collider = new ColliderTwoD(new List<Base.Collision.ICollisionBound2D> { BCBC });
         RigidBody2D RB2D = new RigidBody2D(1, false, false, 1);
         PickupComponent pickupComponent = new PickupComponent(type, 1);

         scene.AddComponent(pickup, t);
         scene.AddComponent(pickup, s);
         scene.AddComponent(pickup, collider);
         scene.AddComponent(pickup, RB2D);
         scene.AddComponent(pickup, pickupComponent);

         return pickup;
      }
   }
}
