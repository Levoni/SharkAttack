using Base.Components;
using Base.Entities;
using Base.Scenes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestGame.Components;
using TestGame.Utility.Services;

namespace TestGame.Factories
{
   public static class TrinketFactory
   {
      //TODO: make fountain use an animated sprite for the burst affect
      //TODO: make middle of image spawn in middle of player transofrm (may need palyer sprite info)
      public static Entity SpawnLifeFountain(Scene scene, Transform startTransform, Entity entityToHeal)
      {
         Entity fountain = scene.CreateEntity();

         Transform transform = new Transform(startTransform.X - 10, startTransform.Y - 10, 1, 1, 0); // 10 is half of player size
         Sprite sprite = new Sprite("LifeFountain");
         sprite.imageHeight = 100;
         sprite.imageWidth = 100;
         LifeFountainComponent lifeFountainComponent = new LifeFountainComponent(entityToHeal, 50);
         LifespanComponent lifespanComponent = new LifespanComponent(5000);

         scene.AddComponent(fountain, transform);
         scene.AddComponent(fountain, sprite);
         scene.AddComponent(fountain, lifeFountainComponent);
         scene.AddComponent(fountain, lifespanComponent);

         return fountain;
      }

      public static Entity SpawnDummyEnitty(Scene scene, Transform startTransform)
      {
         Entity dummy = scene.CreateEntity();



         Transform transform = new Transform(startTransform.X - 10, startTransform.Y - 10, 1, 1, 0); // 10 is half of player size
         Sprite sprite = new Sprite("Decoy");
         sprite.imageHeight = 20;
         sprite.imageWidth = 20;
         DecoyComponent DecoyComponent = new DecoyComponent(10000);

         scene.AddComponent(dummy, transform);
         scene.AddComponent(dummy, sprite);
         scene.AddComponent(dummy, DecoyComponent);

         PlayerTransformService.AssignTransform(transform);

         return dummy;
      }
   }
}
