using Base.UI;
using Base.Events;
using Base.Utility;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Base.Utility.Enums;
using Base.Scenes;
using Base.Utility.Services;

namespace TestGame.UIComponents
{
   [Serializable]
   public class ControlSelector:Control
   {
      Label lblControlDescription;
      Button btnControlChange;
      public bool isSelectingControl { get; set; }
      public ControlType ControlType { get; set; }

      EHandler<KeyPressedEvent> keyPressEvent;
      EHandler<MousePressedEvent> mousePressEvent;

      public ControlSelector(string name, string value, EngineRectangle bounds, Color color, ControlType controlType, string controlDescription,Scene scene):base(name,value,bounds,color)
      {
         setImageReferences("none");
         this.ControlType = controlType;
         isSelectingControl = false;
         lblControlDescription = new Label("lblControlDescription", controlDescription,new EngineRectangle(bounds.X,bounds.Y,bounds.Width / 4 * 3, bounds.Height), color);
         lblControlDescription.minFontScale = 0f;
         string controlValue = "select control";
         if(ControlService.isInitialized && ControlService.controls.ContainsKey(controlType))
         {
            controlValue = ControlService.controls[ControlType].currentControlValue();
            if (controlValue == mouseButton.leftClick.ToString())
            {
               controlValue = "Left Mouse";
            }
            else if (controlValue == mouseButton.rightClick.ToString())
            {
               controlValue = "Right Mouse";
            }
            else if (controlValue == mouseButton.x1Click.ToString())
            {
               controlValue = "Mouse 4";
            }
            else if (controlValue == mouseButton.x2Click.ToString())
            {
               controlValue = "Mouse 5";
            }
         }
         btnControlChange = new Button("btnControlChange", controlValue, new EngineRectangle(bounds.X + bounds.Width / 4 * 3, bounds.Y, bounds.Width / 4, bounds.Height), color);
         btnControlChange.minFontScale = 0;
         btnControlChange.padding = new int[]
         {
            5,(int)btnControlChange.bounds.Width / 20,5,(int)btnControlChange.bounds.Width / 20
         };
         btnControlChange.onClick = new EHandler<OnClick>(HandleChangeClick);

         keyPressEvent = new EHandler<KeyPressedEvent>(new Action<object, KeyPressedEvent>(HandleKeyPressEvent));
         mousePressEvent = new EHandler<MousePressedEvent>(new Action<object, MousePressedEvent>(HandleMousePressEvent));
         scene.bus.Subscribe(keyPressEvent);
         scene.bus.Subscribe(mousePressEvent);
         init();
      }

      public override void Update(int dt)
      {
         base.Update(dt);
         lblControlDescription.Update(dt);
         btnControlChange.Update(dt);
      }

      public override void Render(SpriteBatch sb)
      {
         base.Render(sb);
         lblControlDescription.Render(sb);
         btnControlChange.Render(sb);
      }

      public void HandleKeyPressEvent(object sender, KeyPressedEvent e)
      {
         if(isSelectingControl && e.keyState == gameControlState.keyDown)
         {
            GameControl newControl = new GameControl(e.key);
            ControlService.AddOrUpdateControl(ControlType, newControl);
            btnControlChange.value = newControl.currentControlValue();
            ControlService.SaveControls();
            isSelectingControl = false;
         }
      }

      public void HandleMousePressEvent(object sender, MousePressedEvent e)
      {
         if (isSelectingControl && e.controlState == gameControlState.keyDown)
         {
            GameControl newControl = new GameControl(e.button);
            ControlService.AddOrUpdateControl(ControlType, newControl);
            if (newControl.currentControlValue() == mouseButton.leftClick.ToString())
            {
               btnControlChange.value = "Left Mouse";
            }
            else if (newControl.currentControlValue() == mouseButton.rightClick.ToString())
            {
               btnControlChange.value = "Right Mouse";
            }
            else if (newControl.currentControlValue() == mouseButton.x1Click.ToString())
            {
               btnControlChange.value = "Mouse 4";
            }
            else if (newControl.currentControlValue() == mouseButton.x2Click.ToString())
            {
               btnControlChange.value = "Mouse 5";
            }
            else
            {
               btnControlChange.value = newControl.currentControlValue();
            }
            ControlService.SaveControls();
            isSelectingControl = false;
         }
      }

      public void HandleChangeClick(object sender, OnClick click)
      {
         isSelectingControl = true;
         btnControlChange.value = "Press Control";
      }


   }
}
