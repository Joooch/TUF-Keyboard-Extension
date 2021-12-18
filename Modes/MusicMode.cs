using NAudio.Wave;
using System;
using System.Timers;

namespace TUF_Keyboard_Extension.Modes
{
    class MusicMode : BaseMode
    {
        public MusicMode(KeyboardAPI api, Timer timer) : base(api, timer) { }

        private float currentColor;
        private float targetColor; // (animate 'currentColor' this)
        private WasapiLoopbackCapture audioListener;

        protected override void Initialize()
        {
            base.Initialize();

            currentColor = 0;

            audioListener = new WasapiLoopbackCapture();
            audioListener.DataAvailable += (object caller, WaveInEventArgs args) =>
            {
                byte[] buffer = args.Buffer;
                int bytesRecorded = args.BytesRecorded;
                int bufferIncrement = audioListener.WaveFormat.BlockAlign;

                float max = 0;

                for (int index = 0; index < bytesRecorded; index += bufferIncrement)
                {
                    float sample32 = Math.Abs( BitConverter.ToSingle(buffer, index) );

                    if (sample32 > max)
                    {
                        max = sample32;
                    }
                }

                targetColor = (max) * 70;
            };
            audioListener.RecordingStopped += (s, a) =>
            {
                Dispose();
            };

            audioListener.StartRecording();
        }

        static float Lerp(float firstFloat, float secondFloat, float by)
        {
            return firstFloat * (1 - by) + secondFloat * by;
        }

        protected override void Tick(object caller, EventArgs args)
        {
            currentColor = (float)Math.Round(Lerp(currentColor, targetColor, .2f), 1);

            if (currentColor > .2f)
            {
                keyboardAPI.SetLaptopRGBBacklight(KeyboardAPI.EffectType.STATIC, KeyboardAPI.Tempo.NONE, Math.Min((uint)(currentColor * 25), 255), 0, 0);
            }
            else if (currentColor != 0)
            {
                currentColor = 0;
                keyboardAPI.SetLaptopRGBBacklight(KeyboardAPI.EffectType.STATIC, KeyboardAPI.Tempo.NONE, 0, 0, 0);
            }
        }

        public override void Dispose()
        {
            base.Dispose();

            audioListener?.StopRecording();
            audioListener?.Dispose();
            audioListener = null;
        }
    }
}
