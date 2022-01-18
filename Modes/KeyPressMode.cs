using Keystroke.API;
using Keystroke.API.CallbackObjects;
using System;
using System.Timers;

namespace TUF_Keyboard_Extension.Modes
{
    class KeyPressMode : BaseMode
    {
        KeystrokeAPI keystrokeAPI;
        public KeyPressMode(KeyboardAPI api, Timer timer) : base(api, timer) { }

        protected float colorValue;

        protected virtual void OnKeyPress(KeyPressed key)
        {
            colorValue = Math.Min(colorValue + 100, 255);
        }

        protected override void Initialize()
        {
            base.Initialize();

            colorValue = 0;

            keystrokeAPI = new KeystrokeAPI();
            keystrokeAPI.CreateKeyboardHook(OnKeyPress);
        }


        protected float Lerp(float firstFloat, float secondFloat, float by)
        {
            return firstFloat * (1 - by) + secondFloat * by;
        }

        protected override void Tick(object caller, EventArgs args)
        {
            colorValue = (float)Math.Round(Lerp(colorValue, 0, .05f), 2);

            if (colorValue > .05f)
            {
                keyboardAPI.SetLaptopRGBBacklight(KeyboardAPI.EffectType.STATIC, KeyboardAPI.Tempo.NONE, (uint)colorValue, 0, 0);
            }
        }

        public override void Dispose()
        {
            base.Dispose();

            keystrokeAPI?.Dispose();
            keystrokeAPI = null;
        }
    }
}
