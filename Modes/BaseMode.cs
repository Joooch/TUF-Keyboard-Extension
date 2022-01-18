using System;
using System.Timers;

namespace TUF_Keyboard_Extension.Modes
{
    abstract class BaseMode
    {
        protected KeyboardAPI keyboardAPI { get; set; }
        private Timer timer { get; set; }

        public BaseMode(KeyboardAPI api, Timer timer)
        {
            keyboardAPI = api;
            this.timer = timer;
        }
        public void Start()
        {
            Dispose();
            Initialize();
        }
        protected virtual void Tick(object caller, EventArgs args)
        {

        }
        protected virtual void Initialize()
        {
            timer.Elapsed += Tick;
        }
        public virtual void Dispose()
        {
            timer.Elapsed -= Tick;
            // delete everything and collect garbage here
        }
    }
}
