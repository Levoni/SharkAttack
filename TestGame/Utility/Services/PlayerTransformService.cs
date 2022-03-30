using Base.Components;
using Base.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestGame.Utility.Services
{
   public static class PlayerTransformService
   {
      static public bool TranformAssigned = false;
      static private Transform playerTransform = null;

      public static List<Transform> targetTransforms { get; set; }

      static public void AssignPlayerTransform(Transform transform)
      {
         targetTransforms = new List<Transform>();
         playerTransform = transform;
         targetTransforms.Add(transform);
         TranformAssigned = true;
      }

      static public void AssignTransform(Transform transform)
      {
         targetTransforms.Add(transform);
      }

      static public void UnassignAllTransforms()
      {
         targetTransforms = new List<Transform>();
         playerTransform = null;
         TranformAssigned = false;
      }

      static public void UnassignPlayerTransform(Transform transformToRemove)
      {
         for(int i = targetTransforms.Count - 1; i >= 0; i--)
         {
            if(playerTransform == transformToRemove)
            {
               targetTransforms.RemoveAt(i);
            }
         }
         playerTransform = null;
      }

      static public void UnassignTransform(Transform transformToRemove)
      {
         for (int i = targetTransforms.Count - 1; i >= 0; i--)
         {
            if (targetTransforms[i] == transformToRemove)
            {
               targetTransforms.RemoveAt(i);
            }
         }
      }

      static public Transform GetTransform(EngineVector2 baseLocation)
      {
         Transform targetTransform = null;
         float distance = float.MaxValue;
         foreach(Transform t in targetTransforms)
         {
            float xDiff = t.X - baseLocation.X;
            float yDiff = t.Y - baseLocation.Y;
            float diff = (float)Math.Sqrt(xDiff * xDiff + yDiff * yDiff);
            if (diff < distance)
            {
               targetTransform = t;
               distance = diff;
            }
         }
         return targetTransform;
      }

      static public Transform GetPlayerTransform()
      {
         return playerTransform;
      }
   }
}
