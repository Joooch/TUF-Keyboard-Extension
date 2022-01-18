using System;

namespace TUF_Keyboard_Extension.Modes
{
    class ReversedKeyPressMode : KeyPressMode
    {
        public ReversedKeyPressMode(KeyboardAPI api, System.Timers.Timer timer) : base(api, timer) { }

        protected override void OnKeyPress(Keystroke.API.CallbackObjects.KeyPressed key)
        {
            colorValue = Math.Max(colorValue - 150, 0);
        }

        protected override void Tick(object caller, EventArgs args)
        {
            colorValue = (float)Math.Round(Lerp(colorValue, 255, .05f), 2);

            if (colorValue > .05f)
            {
                keyboardAPI.SetLaptopRGBBacklight(KeyboardAPI.EffectType.STATIC, KeyboardAPI.Tempo.NONE, (uint)colorValue, 0, 0);
            }
        }
    }
}
