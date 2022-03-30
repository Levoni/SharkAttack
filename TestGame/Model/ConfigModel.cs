using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestGame.Model
{
   [Serializable]
   public class ConfigModel
   {
      public float SoundVolume;
      public string SpriteSet;
      public bool GoldenSkin { get; set; }

      public ConfigModel()
      {
         SoundVolume = .05f;
         SpriteSet = "Donuts and Sharks";
         GoldenSkin = false;
      }
   }
}
